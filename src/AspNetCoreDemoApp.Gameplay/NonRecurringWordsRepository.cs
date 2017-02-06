using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreDemoApp.Gameplay
{
    public class NonRecurringWordsRepository : IWordsRepository
    {
        private readonly List<string> words;
        private readonly List<string> usedWords = new List<string>();

        public NonRecurringWordsRepository(IEnumerable<string> words)
        {
            this.words = words.ToList();
        }

        public string this[int index]
        {
            get
            {
                var word = this.words[index];

                this.words.RemoveAt(index);
                this.usedWords.Add(word);

                return word;
            }
        }

        public int Count => this.words.Count;
        public IEnumerator<string> GetEnumerator()
        {
            return this.words.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}