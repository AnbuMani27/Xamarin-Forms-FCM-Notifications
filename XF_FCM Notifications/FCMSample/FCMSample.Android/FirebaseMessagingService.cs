using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;
using System;
using System.Text.RegularExpressions;
namespace FCMSample.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {       
        const string TAG = "MyFirebaseMsgService";
        public const string PRIMARY_CHANNEL = "default";          
        // [START receive_message]
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            { 
                Android.Util.Log.Debug(TAG, "From: " + message.From);
                Android.Util.Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                SendNotifications(message);
            }
            catch (Exception ex)
            {               
            }
        }
        // [END receive_message]

        public void SendNotifications(RemoteMessage message)
        {
            try
            {
                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
                int id = new Random(seed).Next(000000000, 999999999);
                var push = new Intent();
                var fullScreenPendingIntent = PendingIntent.GetActivity(this, 0,
                        push, PendingIntentFlags.CancelCurrent);
                NotificationCompat.Builder notification;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var chan1 = new NotificationChannel(PRIMARY_CHANNEL,
                       new Java.Lang.String("Primary"), NotificationImportance.High);
                    chan1.LightColor = Color.Green;
                    manager.CreateNotificationChannel(chan1);
                    notification = new NotificationCompat.Builder(this, PRIMARY_CHANNEL);
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    notification = new NotificationCompat.Builder(this);
#pragma warning restore CS0618 // Type or member is obsolete
                }
                notification.SetContentIntent(fullScreenPendingIntent)
                         .SetContentTitle(message.GetNotification().Title)
                         .SetContentText(message.GetNotification().Body)
                         .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon))
                         .SetSmallIcon(Resource.Drawable.icon)
                         .SetStyle((new NotificationCompat.BigTextStyle()))
                         .SetPriority(NotificationCompat.PriorityHigh)
                         .SetColor(0x9c6114)
                         .SetAutoCancel(true);
                manager.Notify(id, notification.Build());
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}