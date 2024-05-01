using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Mocks
{
    public static class LoggerMock
    {
        public static Mock<ILogger<T>> Create<T>()
        {
            return new Mock<ILogger<T>>();
        }

        public static Mock<ILogger<T>> SetupLogLevel<T>(this Mock<ILogger<T>> loggerMock, LogLevel logLevel)
        {
            loggerMock.Setup(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    (Func<object, Exception, string>)It.IsAny<object>()
                )
            );

            return loggerMock;
        }
    }
}
