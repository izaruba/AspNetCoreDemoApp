namespace AspNetCoreDemoApp.Gameplay
{
    public class GameEndedEventArgs : GameRoomEventArgs
    {
        public Player Winner { get; }

        public GameEndedEventArgs(GameRoom room, Player winner) : base(room)
        {
            this.Winner = winner;
        }
    }
}