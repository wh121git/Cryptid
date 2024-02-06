using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NativeGalleryNamespace;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Android;
using System;


/*
 * Code sources:
 * [1] Basic save and load functionality: https://gamedev.stackexchange.com/questions/115863/how-to-save-variables-into-a-file-unity
 * [2] Android save function using JSON:  https://stackoverflow.com/questions/52491198/how-to-save-manage-game-progress-in-mobile-devices-unity
 * [3] Date time string converting: https://stackoverflow.com/questions/10798980/convert-c-sharp-date-time-to-string-and-back
 * [4] Unity Android Gallery support: https://assetstore.unity.com/packages/tools/integration/native-gallery-for-android-ios-112630
 * [5] Gain image from Gallery at yasirkula, Apr 25, 2018: https://forum.unity.com/threads/native-gallery-for-android-ios-open-source.519619/
 */
public class UserManager : MonoBehaviour
{
    // get cryptid class
    public cryptidGen cryptidGen;
    public MobileNotifications mobileNotifications;
    public serverRep serverRep;
    public DebugDisplay debugDisplay;

    // score functionality
    private int score;

    public (float, float) userLocation;

    // make permenant through reloads
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {

        //[3]
        score = PlayerPrefs.GetInt("score", 0);


        GetPlayerLocation();

        if(score == 0)
        {
            PlayerPrefs.SetInt("score", 0);
        }

    }
    private void initCryptid()
    {

        DateTime now = DateTime.Now;

        // check that an hour has passed
        DateTime.TryParse(PlayerPrefs.GetString("time", "0"), out DateTime then);

        // check if there is a cryptid at this location
        (GameObject, int) serverOut = serverRep.findCryptid(userLocation);
        if (serverOut.Item1 != null)
        {
            cryptidGen.createExistingCryptid(serverOut.Item1, serverOut.Item2);
        }
        // otherwise randomly generate after an hour
        else
        {
            if (then.AddHours(1) < now)
            if (then.AddHours(1) < now)
            {
                cryptidGen.createCryptid();
            }
        }
    }
    private void OnApplicationQuit()
    {
        // save data for check later
        PlayerPrefs.SetInt("score", score);

        DateTime now = DateTime.Now;
        string nowStr = now.ToString();
        PlayerPrefs.SetString("time", nowStr);
    }


    // Score variable

    public void setScore(int score)
    {
        this.score = score;
    }
    public void increaseScore(int inc)
    {
        // score increase
        score += inc;
        PlayerPrefs.SetInt("score", score);
    }
    public int displayScore()
    {
        return score;
    }
    public int getScore()
    {
        return score;
    }



    // picture functionality
    public void capture()
    {
        if (cryptidGen.exists)
        {
            // allow co-routine to ensure end of frame
            StartCoroutine(takePictureEn());
        }
        else
        {
            StartCoroutine(GetPictureEn());
        }
    }

    private IEnumerator takePictureEn()
    {
        if (cryptidGen.exists)
        {
            // end of frame ensured
            yield return new WaitForEndOfFrame();


            // Create image file
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            RenderTexture pre = Camera.main.targetTexture;
            Camera.main.targetTexture = rt;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            // render and save
            Camera.main.Render();
            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();

            // saves image as: Cryptid_Capture_[name of cryptid](Clone)_[date].png
            string imgName = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "_" + cryptidGen.getCurrentCryptidName(), System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            
            //[4]
            Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(screenshot, Application.productName + " Captures", imgName));

            Camera.main.targetTexture = pre;
            cryptidGen.DeleteCryptid();
            increaseScore(1);
            mobileNotifications.sendNotif();
        }
    }

    private IEnumerator GetPictureEn()
    {
        // end frame 
        yield return new WaitForEndOfFrame();

        //[5]
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                var pathArray = path.Split('/');
                string output = "";

                foreach (string seg in pathArray)
                {
                    output = seg;
                }

                // get cryptid name specifically
                output = output.Substring(16, output.Length - 47);
                
                cryptidGen.createSpecificCryptid(output);

                // delete image
                File.Delete(path);
            }
        });

        

    }

    public void GetPlayerLocation()
    {
        StartCoroutine(Location());
    }

    private IEnumerator Location()
    {
        

        // permissions from the user
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // Check permissions
        if (!Input.location.isEnabledByUser)
            debugDisplay.addOut("Location not enabled on device or app does not have permission to access location");

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
            debugDisplay.addOut("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugDisplay.addOut("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location, returning a rounded version to the user location variables
            userLocation.Item1 = Input.location.lastData.latitude;
            userLocation.Item1 = (float)Math.Ceiling(userLocation.Item1 * 100f) / 100f;
            userLocation.Item2 = Input.location.lastData.longitude;
            userLocation.Item2 = (float)Math.Ceiling(userLocation.Item2 * 100f) / 100f;
            debugDisplay.addOut("current Location: " + userLocation.ToString());
        }

        initCryptid();

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
}
