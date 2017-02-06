using System;

namespace AspNetCoreDemoApp.Gameplay
{
    public class Message
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }
        public string Text { get; }

        public Message(Player player, string text)
        {
            this.PlayerId = player.Id;
            this.PlayerName = player.Name;
            this.Text = text;
        }
    }
}