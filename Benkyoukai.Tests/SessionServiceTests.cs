using Benkyoukai.Api.Services.Sessions;
using Moq;

namespace Benkyoukai.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var mockRepo = new Mock<ISessionService>();
        mockRepo.Setup(repo => repo.GetSessionAsync(1))

        // Act

        // Assert
    }
}
