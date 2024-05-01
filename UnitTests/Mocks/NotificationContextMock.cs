using Dominio.Interfaces.Notification;
using Moq;

namespace UnitTests.Mocks
{
    public static class NotificationContextMock
    {
        public static Mock<INotificationContext> Create()
        {
            var notificationContextMock = new Mock<INotificationContext>();
            notificationContextMock.Setup(nc => nc.AddNotification(It.IsAny<string>()));
            return notificationContextMock;
        }

        public static void SetupHasNotifications(this Mock<INotificationContext> mock, bool hasNotifications)
        {
            mock.Setup(nc => nc.HasNotifications).Returns(hasNotifications);
        }

    }
}
