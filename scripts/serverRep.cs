using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class serverRep : MonoBehaviour
{
    public DebugDisplay debugDisplay;
    public UserManager userManager;


    // library of cryptid ids
    public List<GameObject> library = new List<GameObject>();

    // buffer for new cryptid
    public (int, int) cryptidBuffer;

    // location X, location Y, cryptid, cryptid power
    private List<(float, float, int, int)> cryptids = new List<(float,float, int, int)>();

    private void Start()
    {
        debugDisplay.addOut("Start print : " + PlayerPrefs.GetString("test", "unsaved"));

        // iterate through saved values and populate server rep
        int i = 0;
        while (PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            cryptids.Add((PlayerPrefs.GetFloat("loc" + i + "x"),
            PlayerPrefs.GetFloat("loc"+i+"y"),
            PlayerPrefs.GetInt("id"+i),
            PlayerPrefs.GetInt("pow" + i)));

            // debug out
            debugDisplay.addOut(cryptids[i].Item1 + "," + cryptids[i].Item2 + "," + cryptids[i].Item3 + "," + cryptids[i].Item4);

            i++;

            
        }
    }



    // create buffer cryptid
    public void addToBuffer(string name, int power)
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

        cryptidBuffer.Item1 = i;
        cryptidBuffer.Item2 = power;

        StartCoroutine(SaveData());
       
    }

    private IEnumerator SaveData()
    {
        while(userManager.userLocation.Item1 == 0)
        {
            yield return new WaitForSeconds(1f);
        }

        int i = 0;
        while (PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            i++;
        }

        PlayerPrefs.SetFloat("loc" + i + "x", userManager.userLocation.Item1);
        PlayerPrefs.SetFloat("loc" + i + "y", userManager.userLocation.Item2);
        PlayerPrefs.SetInt("id" + i, cryptidBuffer.Item1);
        PlayerPrefs.SetInt("pow" + i, cryptidBuffer.Item2);
    }

    public (GameObject, int) findCryptid((float, float) userLocation)
    {
        // iterate over server
        int i = 0;
        while(PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
        {
            if(PlayerPrefs.GetFloat("loc" + i + "x") == userLocation.Item1 && PlayerPrefs.GetFloat("loc" + i + "y") == userLocation.Item2)
            {
                return (library[PlayerPrefs.GetInt("id" + i)], PlayerPrefs.GetInt("pow" + i));
            }
            i++;
        }

        return (null,0);
    }
}
