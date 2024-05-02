using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class HTTP_Request : MonoBehaviour
{
    public Text output;
    public DebugDisplay debugDisplay;

    public CryptidInstance data = null;

    public CryptidInstance Get(float x, float y)
    {
        IEnumerator getFunc = GetE(x,y);
        StartCoroutine(getFunc);
        try
        {
            while(getFunc.MoveNext())
            {

                if(getFunc.Current is CryptidInstance) 
                {
                    return data; 
                }
            }
        }
        catch(Exception e)
        {
            return null;
        }

        return null;
    }

    private IEnumerator GetE(float x, float y)
    {
        UnityWebRequest wr = new UnityWebRequest("https://localhost:7078/Instance?x=" + x + "&y=" + y);
        wr.downloadHandler = new DownloadHandlerBuffer();
        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            output.text = wr.error;
            debugDisplay.addOut("Error spit: " + wr.error);
        }
        else
        {
            // Show results as text
            data = JsonUtility.FromJson<CryptidInstance>(wr.downloadHandler.text);
            debugDisplay.addOut("Correct spit: " + wr.downloadHandler.text);
            yield return data;
        }
    }

    public void Post(int id, string init, float x, float y)
    {
        StartCoroutine(PostE(id, init, x, y));
    }

    private IEnumerator PostE(int id, string init, float x, float y)
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
    
    public void Put(float x, float y, int _id)
    {
        StartCoroutine(PutE(x, y, _id));
    }

    private IEnumerator PutE(float x, float y, int _id)
    {
        UnityWebRequest wr = new UnityWebRequest("https://localhost:7078/Instance?x=" + x + "&y=" + y + "&_id=" + _id);
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
