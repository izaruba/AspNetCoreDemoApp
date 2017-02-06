namespace AspNetCoreDemoApp.Gameplay
{
    public class GameRoomEventArgs
    {
        public GameRoom Room { get; }

        public GameRoomEventArgs(GameRoom room)
        {
            this.Room = room;
        }
    }
}