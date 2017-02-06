using System;

namespace AspNetCoreDemoApp.Gameplay
{
    internal interface IDomainEntity
    {
        Guid Id { get; }
    }
}