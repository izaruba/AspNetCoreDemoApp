using System;

namespace AspNetCoreDemoApp.Gameplay
{
    internal class GameEngine
    {
        private readonly IWordsRepository wordsRepository;
        private string hiddenWord;
        private readonly Random random = new Random();

        public GameEngine(IWordsRepository wordsRepository)
        {
            this.wordsRepository = wordsRepository;
        }

        public string StartNewGame()
        {
            var index = this.random.Next(0, this.wordsRepository.Count);
            this.hiddenWord = this.wordsRepository[index];

            return this.hiddenWord;
        }

        public bool Check(string input)
        {
            return input.Equals(this.hiddenWord, StringComparison.OrdinalIgnoreCase);
        }
    }
}