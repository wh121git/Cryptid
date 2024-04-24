using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTP_Request : MonoBehaviour
{
    public Text output;

    void Start()
    {
        //StartCoroutine(test());
    }

    public CryptidInstance data;

    public void Get(float x, float y)
    {
        StartCoroutine(GetE(x,y));
    }

    IEnumerator GetE(float x, float y)
    {
        UnityWebRequest wr = new UnityWebRequest("https://localhost:7078/Instance?x=" + x + "&y=" + y);
        wr.downloadHandler = new DownloadHandlerBuffer();
        yield return wr.SendWebRequest();

        UnityEngine.Debug.Log(output.text);

        if (wr.result != UnityWebRequest.Result.Success)
        {
            output.text = wr.error;
        }
        else
        {
            // Show results as text
            data = JsonUtility.FromJson<CryptidInstance>(wr.downloadHandler.text);
        }
    }

    public void Post(int id, string init, float x, float y)
    {
        StartCoroutine(PostE(id, init, x, y));
    }

    IEnumerator PostE(int id, string init, float x, float y)
    {
        UnityWebRequest wr = new UnityWebRequest("https://localhost:7078/Instance?id=" + id + "&init=" + init + "&x=" + x + "&y=" + y);
        wr.downloadHandler = new DownloadHandlerBuffer();
        yield return wr.SendWebRequest();

        UnityEngine.Debug.Log(output.text);

        if (wr.result != UnityWebRequest.Result.Success)
        {
            output.text = wr.error;
        }
        else
        {
            // Show results as text
            data = JsonUtility.FromJson<CryptidInstance>(wr.downloadHandler.text);
        }      
    }

    public class CryptidInstance
    {
        public int id;
        public string init;
        public float x;
        public float y;
    }
}
