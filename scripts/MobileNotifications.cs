using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class MobileNotifications : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // create notification channel
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notif Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void sendNotif()
    {
        var notification = new AndroidNotification();
        notification.Title = "Cryptid Regeneration";
        notification.Text = "A new Cryptid has appeared!";
        notification.FireTime = System.DateTime.Now.AddHours(1);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
