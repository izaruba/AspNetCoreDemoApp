using System;
using System.Collections.Generic;

namespace AspNetCoreDemoApp.Gameplay
{
    internal interface IConcurrentCollection<T> : IEnumerable<T> where T : class, IDomainEntity
    {
        void Add(T item);
        void Clear();
        bool Contains(Guid id);
        bool Remove(Guid id);
        int Count { get; }
        T this[Guid id] { get; }
    }
}