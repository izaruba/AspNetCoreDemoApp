using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AspNetCoreDemoApp.Gameplay
{
    internal class ConcurrentCollection<T> : IConcurrentCollection<T>
        where T : class, IDomainEntity
    {
        private readonly ConcurrentDictionary<Guid,T> items = new ConcurrentDictionary<Guid, T>();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        public void Add(T item)
        {
            this.items.AddOrUpdate(item.Id, item, (id, i) => i);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(Guid id)
        {
            return this.items.Keys.Contains(id);
        }

        public bool Remove(Guid id)
        {
            return this.items.TryRemove(id, out var item);
        }

        public int Count => this.items.Count;

        public T this[Guid id]
        {
            get
            {
                Contract.Requires(() => id != Guid.Empty, $"Invalid ID {id}");

                this.items.TryGetValue(id, out var item);

                return Contract.Ensure.NotNull(item, $"Item not found (ID={id}).");
            }
        }
    }
}