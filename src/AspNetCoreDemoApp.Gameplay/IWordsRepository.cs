using System.Collections.Generic;

namespace AspNetCoreDemoApp.Gameplay
{
    public interface IWordsRepository : IEnumerable<string>
    {
        string this[int index] { get; }
        int Count { get; }
    }
}