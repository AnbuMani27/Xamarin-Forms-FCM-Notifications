using System;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace FCMSample.iOS
{ 
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    { 
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            RegisterForRemoteNotifications();

            Messaging.SharedInstance.Delegate = this;
            if (UNUserNotificationCenter.Current != null)
            {
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }

            Firebase.Core.App.Configure();

            return base.FinishedLaunching(app, options);
        }
        private void RegisterForRemoteNotifications()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>
                {
                    Console.WriteLine(granted);
                    await System.Threading.Tasks.Task.Delay(500);
                    //await AcessPermissionsAsync();
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
            //base.RegisteredForRemoteNotifications(application, deviceToken);
        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            base.FailedToRegisterForRemoteNotifications(application, error);
        }
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Print full message.

            completionHandler(UIBackgroundFetchResult.NewData);
        }
        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
            System.Diagnostics.Debug.WriteLine($"######Token######  :  {fcmToken}");
            Console.WriteLine(fcmToken);
            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }
    }
}
