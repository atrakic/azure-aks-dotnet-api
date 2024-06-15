using Xunit;

using DotnetApi.Data;


namespace UnitTests.Data
{
    public class UserGeneratorTests
    {
        [Fact]
        public void GenerateSql_WhenCalled_ReturnsValidSql()
        {
            // Arrange
            var users = UserGenerator.Generate();

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
