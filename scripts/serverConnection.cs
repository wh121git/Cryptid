using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class serverConnection : MonoBehaviour
{
    public DebugDisplay debugDisplay;
    public UserManager userManager;

    // library of cryptid ids
    public List<GameObject> library = new List<GameObject>();

    // buffer for new cryptid
    public int cryptidBuffer;

    // location X, location Y, cryptid, time innit
    private List<(float, float, int, string)> cryptids = new List<(float,float, int, string)>();

    private void Start()
    {
        String now = DateTime.Now.ToString();
        
        debugDisplay.addOut("Start print : ");

        // iterate through saved values and populate server rep
        int i = 0;
        while (PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            cryptids.Add((PlayerPrefs.GetFloat("loc" + i + "x"),
            PlayerPrefs.GetFloat("loc"+i+"y"),
            PlayerPrefs.GetInt("id"+i),
            PlayerPrefs.GetString("init" + i)));

            // debug out
            debugDisplay.addOut(cryptids[i].Item1 + "," + cryptids[i].Item2 + "," + cryptids[i].Item3 + "," + cryptids[i].Item4);

            i++;

            
        }
    }



    // create buffer cryptid
    public void addToBuffer(string name)
    {
        // find correct id
        int i = 0;
        foreach(GameObject c in library)
        {
            if(c.name == name)
            {
                break;
            }

            i++;
        }

        cryptidBuffer = i;

        StartCoroutine(SaveData());
       
    }

    private IEnumerator SaveData()
    {
       
        bool saved = false;

        while(userManager.userLocation.Item1 == 0)
        {
            yield return new WaitForSeconds(1f);
        }

        int i = 0;
        while (PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            if (PlayerPrefs.GetFloat("loc" + i + "x") == userManager.userLocation.Item1)
            {
                saved = true;
                debugDisplay.addOut("loc");
                PlayerPrefs.SetInt("id" + i, cryptidBuffer);
                break;
            }
            i++;
        }

        if (saved == false)
        {
            PlayerPrefs.SetFloat("loc" + i + "x", userManager.userLocation.Item1);
            PlayerPrefs.SetFloat("loc" + i + "y", userManager.userLocation.Item2);
            PlayerPrefs.SetInt("id" + i, cryptidBuffer);
            PlayerPrefs.SetString("init" + i, DateTime.Now.ToString());
            saved = true;
        }
    }

    public (GameObject, String) findCryptid((float, float) userLocation)
    {
        // iterate over server
        int i = 0;
        while(PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            if(PlayerPrefs.GetFloat("loc" + i + "x") == userLocation.Item1 && PlayerPrefs.GetFloat("loc" + i + "y") == userLocation.Item2)
            {
                return (library[PlayerPrefs.GetInt("id" + i)], PlayerPrefs.GetString("init" + i));
            }
            i++;
        }

        return (null,"null");
    }
}