using System;
using System.Linq;
using AspNetCoreDemoApp.Gameplay;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AspNetCoreDemoApp.UnitTests
{
    public class GameRoomTests
    {
        #region GameRoom.Join

        [Fact(DisplayName = "GameRoom.Join throws exception if player is `null`")]
        public void Join_NullPlayer_ThrowsException()
        {
            // Arrange
            var arr = new[] { "word1", "word2", "word3" };
            var words = new NonRecurringWordsRepository(arr);
            var loggerFactory = new LoggerFactory();
            var gameRoom = new GameRoom(loggerFactory.CreateLogger<GameRoom>(), words);

            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                gameRoom.Join(null);
            });
        }

        [Fact(DisplayName = "GameRoom.Join adds new player to the room")]
        public void Join_Player_AddsPlayerToRoom()
        {
            // Arrange
            var arr = new[] { "word1", "word2", "word3" };
            var words = new NonRecurringWordsRepository(arr);
            var loggerFactory = new LoggerFactory();
            var gameRoom = new GameRoom(loggerFactory.CreateLogger<GameRoom>(), words);
            var id = Guid.NewGuid();
            var player = new Player(id, "name");

            // Act
            gameRoom.Join(player);

            // Assert
            Assert.Equal(1, gameRoom.Players.Count());
            Assert.NotNull(gameRoom.Players.Single(p => p.Id == player.Id && p.Name == player.Name));
            Assert.Contains(player, gameRoom.Players);
        }

        [Fact(DisplayName = "GameRoom.Join throws exception if the room is full")]
        public void Join_PlayerToFullRoom_ThrowsException()
        {
            // Arrange
            var arr = new[] { "word1", "word2", "word3" };
            var words = new NonRecurringWordsRepository(arr);
            var loggerFactory = new LoggerFactory();
            var gameRoom = new GameRoom(loggerFactory.CreateLogger<GameRoom>(), words);

            // Act
            for (var i = 1; i <= gameRoom.MaxPlayers; i++)
            {
                gameRoom.Join(new Player(Guid.NewGuid(), $"Player{i}"));
            }

            // Assert
            Assert.True(gameRoom.IsFull);
            Assert.Equal(gameRoom.MaxPlayers, gameRoom.Players.Count());
            Assert.Throws<Exception>(() =>
            {
                gameRoom.Join(new Player(Guid.NewGuid(), $"Player{gameRoom.MaxPlayers + 1}"));
            });
        }

        #endregion
    }
}