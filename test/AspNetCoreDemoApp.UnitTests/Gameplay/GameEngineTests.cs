using System.Linq;
using Xunit;
using AspNetCoreDemoApp.Gameplay;

namespace AspNetCoreDemoApp.UnitTests
{
    public class GameEngineTests
    {
        [Fact (DisplayName = "GameEngine.StartNewGame returns new random word to guess")]
        public void StartNewGame_ReturnsNewWordToGuess()
        {
            // Arrange
            var arr = new[] {"word1", "word2", "word3"};
            var words = new NonRecurringWordsRepository(arr);
            var engine = new GameEngine(words);

            // Act
            var randomWord = engine.StartNewGame();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(randomWord));
            Assert.Contains(randomWord, arr);
        }

        [Fact (DisplayName = "GameEngine.Check returns `false` if the hidden word was not guessed")]
        public void Check_WrongWord_ReturnsFalse()
        {
            // Arrange
            var words = new NonRecurringWordsRepository(new[] { "word1", "word2" });
            var engine = new GameEngine(words);
            var hiddenWord = engine.StartNewGame();
            var wrongWord = words.First(w => w != hiddenWord);

            // Act
            var @false = engine.Check(wrongWord);

            // Assert
            Assert.False(@false);
        }

        [Fact (DisplayName = "GameEngine.Check returns `true` if the hidden word was guessed")]
        public void Check_CorrectWord_ReturnsTrue()
        {
            // Arrange
            var words = new NonRecurringWordsRepository(new[] { "hidden", "wrong" });
            var engine = new GameEngine(words);
            var hiddenWord = engine.StartNewGame();

            // Act
            var @true = engine.Check(hiddenWord);

            // Assert
            Assert.True(@true);
        }
    }
}