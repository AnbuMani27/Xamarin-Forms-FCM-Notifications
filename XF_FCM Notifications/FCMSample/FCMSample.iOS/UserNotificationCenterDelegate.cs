using System;
using UserNotifications;

namespace FCMSample.iOS
{
    internal class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {

        public UserNotificationCenterDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);
            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

    }
}