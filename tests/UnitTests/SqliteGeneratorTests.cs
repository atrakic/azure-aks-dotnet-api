using Xunit;

using DataSqlite;

namespace UnitTests
{
    public class SqliteGeneratorTests
    {
        [Fact]
        public void GenerateSql_WhenCalled_ReturnsValidSql()
        {
            // Arrange
            var users = SqliteGenerator.GenerateUsers();

            // Act

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.True(users.Count() > 0);
            Assert.Contains(users, u => u.Name == "Admin");
            Assert.DoesNotContain(users, u => u.Name == "Demo");
            Assert.All(users, item => Assert.False(string.IsNullOrWhiteSpace(item.Name)));
        }
    }
}