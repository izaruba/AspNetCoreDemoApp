namespace AspNetCoreDemoApp.Gameplay
{
    public class GameStartedEventArgs : GameRoomEventArgs
    {
        public string WordToGuess { get; }

        public GameStartedEventArgs(GameRoom room, string wordToGuess) : base(room)
        {
            this.WordToGuess = wordToGuess;
        }
    }
}