namespace AspNetCoreDemoApp.Gameplay
{
    public class GameStartingEventArgs : GameRoomEventArgs
    {
        public int StartTimeout { get; }

        public GameStartingEventArgs(GameRoom room, int startTimeoutSeconds) 
            : base(room)
        {
            this.StartTimeout = startTimeoutSeconds;
        }
    }
}