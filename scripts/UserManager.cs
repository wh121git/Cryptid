using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NativeGalleryNamespace;
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class UserManager : MonoBehaviour
{
    public cryptidGen cryptidGen;

    // score functionality
    private int score;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        score = 0;
    }

    public void increaseScore(int inc)
    {
        // score increase
        score += inc;
    }
    public int displayScore()
    {
        return score;
    }

    // picture functionality
    public void takePicture()
    {
        // allow co-routine to ensure end of frame
        StartCoroutine(takePictureEn());
        increaseScore(1);
    }

    private IEnumerator takePictureEn()
    {
        // end of frame ensured
        yield return new WaitForEndOfFrame();


        // Create image file
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture pre = Camera.main.targetTexture;
        Camera.main.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width,Screen.height, TextureFormat.RGB24, false);

        // render and save
        Camera.main.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        string imgName = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(screenshot, Application.productName + " Captures", imgName));

        Camera.main.targetTexture = pre;
        cryptidGen.createCryptid();
    }

    public int getScore()
    {
        return score;
    }    
}
