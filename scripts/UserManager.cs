using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NativeGalleryNamespace;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System;

/*
 * Code sources:
 * [1] Basic save and load functionality: https://gamedev.stackexchange.com/questions/115863/how-to-save-variables-into-a-file-unity
 * [2] Android save function using JSON:  https://stackoverflow.com/questions/52491198/how-to-save-manage-game-progress-in-mobile-devices-unity
 *[3] Date time string converting: https://stackoverflow.com/questions/10798980/convert-c-sharp-date-time-to-string-and-back
 */
public class UserManager : MonoBehaviour
{
    // get cryptid class
    public cryptidGen cryptidGen;
    public MobileNotifications mobileNotifications;

    // score functionality
    private int score;

    // make permenant through reloads
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

        //[3]
        score = PlayerPrefs.GetInt("score", 0);

        DateTime now = DateTime.Now;

        DateTime.TryParse(PlayerPrefs.GetString("time", "0"), out DateTime then);

        if(then.AddHours(1) <  now )
        {
            cryptidGen.createCryptid();
        }
  


        if(score == 0)
        {
            PlayerPrefs.SetInt("score", 0);
        }

    }
    private void OnApplicationQuit()
    {
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
    public void takePicture()
    {
        // allow co-routine to ensure end of frame
        StartCoroutine(takePictureEn());
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

            string imgName = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(screenshot, Application.productName + " Captures", imgName));

            Camera.main.targetTexture = pre;
            cryptidGen.DeleteCryptid();
            increaseScore(1);
            mobileNotifications.sendNotif();
        }
    }
}
