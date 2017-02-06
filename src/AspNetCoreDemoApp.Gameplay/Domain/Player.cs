using System;

namespace AspNetCoreDemoApp.Gameplay
{
    public class Player : IDomainEntity
    {
        public Guid Id { get; }
        public string Name { get; }

        public Player(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}