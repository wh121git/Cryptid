using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class HTMLtest : MonoBehaviour
{
    public Text test;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fetch(1f,1f));
    }

    private IEnumerator fetch(float x, float y)
    {
        UnityWebRequest wr = new UnityWebRequest("https://localhost:7078/Instance?x=" + x + "&y=" + y);
        wr.downloadHandler = new DownloadHandlerBuffer();
        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            test.text = wr.error.ToString();
        }
        else
        {
            // Show results as text
            CryptidInstance data = JsonUtility.FromJson<CryptidInstance>(wr.downloadHandler.text);
            test.text = data.init;
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
