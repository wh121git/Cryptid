using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class cryptid : MonoBehaviour
{
    public int powerCap;
    public bool floor;

    private int power = 0;
    private (float, float) location;

    public void setLocationFromCurrent()
    {
        StartCoroutine(Location());
    }

    public void setLocation(float x, float y)
    {
        location = (x, y);
    }

    public (float,float) getLocation()
    {
        return location;
    }

    private IEnumerator Location()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            location.Item1 = Input.location.lastData.latitude;
            location.Item1 = (float)Math.Ceiling(location.Item1 * 100f) / 100f;
            location.Item2 = Input.location.lastData.longitude;
            location.Item2 = (float)Math.Ceiling(location.Item2 * 100f) / 100f;
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

    public void increasePower()
    {
        power++;
    }

    public void setPower(int newPower)
    {
        power = newPower;
    }

    public bool checkPower()
    {
        return power > powerCap;
    }

    public int getPower()
    {
        return power;
    }
}
