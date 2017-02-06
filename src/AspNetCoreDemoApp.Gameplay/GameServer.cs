using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemoApp.Gameplay
{
    public class GameServer
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IWordsRepository wordsRepository;
        private readonly ILogger<GameServer> logger;
        private readonly IConcurrentCollection<GameRoom> rooms = new ConcurrentCollection<GameRoom>();

        public event EventHandler<GameStartingEventArgs> GameStarting;
        public event EventHandler<GameStartedEventArgs> GameStarted;
        public event EventHandler<GameRoomEventArgs> GameEnded;

        public GameServer(ILoggerFactory loggerFactory, IWordsRepository wordsRepository)
        {
            this.loggerFactory = loggerFactory;
            this.wordsRepository = wordsRepository;
            this.logger = loggerFactory.CreateLogger<GameServer>();
        }

        public Guid Connect(Player player)
        {
            var room = this.GetAvailableRoom();

            new Task(() => room.Join(player)).Start();

            return room.Id;
        }

        public void Disconnect(Player player)
        {
            var room = this.GetAvailableRoom();

            room.Leave(player);
        }

        public GameRoom GetRoom(Guid id)
        {
            lock (this.rooms)
            {
                return this.rooms[id];
            }
        }

        protected virtual void OnGameStarting(GameStartingEventArgs e)
        {
            this.LogInfo($"Game starting in {e.StartTimeout} seconds\n\tRoom ID: {e.Room.Id}");
            this.GameStarting?.Invoke(this, e);
        }

        protected virtual void OnGameStarted(GameStartedEventArgs e)
        {
            var players = string.Join(", ", e.Room.Players.Select(p => $"{(p == e.Room.GameLead ? "[LEAD] " : "")}{p.Name}"));

            this.LogInfo($"Game started\n\tRoom ID: {e.Room.Id}\n\tPlayers: {players}\n\tWord: {e.WordToGuess}");
            this.GameStarted?.Invoke(this, e);
        }

        protected virtual void OnGameEnded(GameRoomEventArgs e)
        {
            this.LogInfo($"Game ended\n\tID: {e.Room.Id}");

            this.GameEnded?.Invoke(this, e);

            lock (this.rooms)
            {
                this.rooms.Remove(e.Room.Id);
            }
        }

        private GameRoom GetAvailableRoom()
        {
            lock (this.rooms)
            {
                var room = (from r in this.rooms.Where(x => !x.IsFull)
                            let min = this.rooms.Min(x => x.Players.Count())
                            where r.Players.Count() == min
                            select r).FirstOrDefault();

                if (room != null) return room;

                room = new GameRoom(this.loggerFactory.CreateLogger<GameRoom>(), this.wordsRepository);
                room.GameStarting += (s, e) => this.OnGameStarting(e);
                room.GameStarted += (s, e) => this.OnGameStarted(e);
                room.GameEnded += (s, e) => this.OnGameEnded(e);

                this.rooms.Add(room);

                return room;
            }
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