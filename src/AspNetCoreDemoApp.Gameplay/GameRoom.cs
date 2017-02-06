using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemoApp.Gameplay
{
    public class GameRoom : IDomainEntity
    {
        private readonly ILogger<GameRoom> logger;
        private readonly IConcurrentCollection<Player> players = new ConcurrentCollection<Player>();
        private readonly ICollection<Message> messages = new List<Message>();
        private readonly GameEngine engine;
        private readonly Random random = new Random();
        private Timer timer;

        internal GameStatus GameStatus { get; private set; }

        protected int GameStartingTimeout { get; } = 10; // seconds
        protected int GameDuration { get; } = 5 * 60; // seconds

        public Guid Id { get; } = Guid.NewGuid();
        public IEnumerable<Player> Players => this.players;
        public IEnumerable<Message> Messages => this.messages;
        public Player GameLead { get; private set; }
        public int MaxPlayers { get; protected set; } = 10;
        public bool IsFull => this.players.Count >= this.MaxPlayers;

        public event EventHandler<GameStartingEventArgs> GameStarting;
        public event EventHandler<GameStartedEventArgs> GameStarted;
        public event EventHandler<GameEndedEventArgs> GameEnded;

        public GameRoom(ILogger<GameRoom> logger, IWordsRepository wordsRepository)
        {
            this.logger = logger;
            this.engine = new GameEngine(wordsRepository);
        }

        public void Join(Player player)
        {
            Contract.Requires(() => player != null, $"Argument is null: {nameof(player)}");
            Contract.Requires(() => this.players.Count < this.MaxPlayers, $"Room {this.Id} is full");

            this.players.Add(player);

            this.LogInfo($"Player {player.Name} joined room {this.Id}");

            this.OnPlayerAdded();
        }

        public void Leave(Player player)
        {
            Contract.Requires(() => player != null, "Player not found");
            
            this.players.Remove(player.Id);

            this.LogInfo($"Player {player.Name} has left room {this.Id}");
        }

        public void StartGame()
        {
            var word = this.engine.StartNewGame();
            var ids = this.players.Select(p => p.Id).ToArray();
            var randomId = ids[this.random.Next(0, ids.Length)];

            this.GameLead = this.players[randomId];
            this.GameStatus = GameStatus.Started;
            this.countDown = this.GameDuration; // 5 minutes
            this.timer = new Timer(obj => this.VerifyTimeLeft(), null, 0, 1000);

            this.OnGameStarted(word);
        }

        public void StopGame(Player winner = null)
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.GameStatus = GameStatus.Finished;
            this.OnGameEnded(winner);

            this.messages.Clear();
            this.GameLead = null;
        }

        public void Say(Message message)
        {
            Contract.Requires(() => this.players.Contains(message.PlayerId), "Player not found");

            this.messages.Add(message);

            this.LogInfo($"{message.PlayerName} says: {message.Text}");

            var guessed = this.engine.Check(message.Text);

            if (guessed)
            {
                this.StopGame(this.players[message.PlayerId]);
            }
        }

        protected virtual void OnGameStarting(GameStartingEventArgs e)
        {
            this.GameStarting?.Invoke(this, e);
        }

        protected virtual void OnGameStarted(string word)
        {
            this.GameStarted?.Invoke(this, new GameStartedEventArgs(this, word));
        }

        protected virtual void OnGameEnded(Player winner)
        {
            this.GameEnded?.Invoke(this, new GameEndedEventArgs(this, winner));
        }

        protected virtual async void OnPlayerAdded()
        {
            if (this.GameStatus > GameStatus.ReadyToStart || this.players.Count <= 1) return;

            this.GameStatus = GameStatus.Starting;

            this.OnGameStarting(new GameStartingEventArgs(this, this.GameStartingTimeout));

            await Task.Delay(this.GameStartingTimeout * 1000);

            this.StartGame();
        }

        private int countDown;

        private void VerifyTimeLeft()
        {
            Debug.WriteLine($"Time left: {this.countDown} seconds");

            this.countDown--;

            if (this.countDown > 0) return;

            this.StopGame();
            this.countDown = 0;
        }
        
        private void LogInfo(string message)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation(message);
            }
        }
    }
}