using AspNetCoreDemoApp.Gameplay;
using Xunit;

namespace AspNetCoreDemoApp.UnitTests
{
    public class NonRecurringWordsRepositoryTests
    {
        [Fact(DisplayName = "NonRecurringWordsRepository[index] returns specified word and avoids its recurrence")]
        public void GetByIndex_AvoidsWordRecurrence()
        {
            // Arrange
            var arr = new[] { "word1", "word2", "word3" };
            var words = new NonRecurringWordsRepository(arr);

            // Act
            var word = words[0];

            // Assert
            Assert.Contains(word, arr);
            Assert.DoesNotContain(word, words);
        }
    }
}