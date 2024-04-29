using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class serverConnection : MonoBehaviour
{
    public DebugDisplay debugDisplay;
    public UserManager userManager;

    public HTTP_Request HTTP_Request;

    // library of cryptid ids
    public List<GameObject> library = new List<GameObject>();

    // buffer for new cryptid
    public int cryptidBuffer;

    private (GameObject, string) fetchedCryptid;




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

        StartCoroutine(SaveData(i));
       
    }

    private IEnumerator SaveData(int i)
    {
       
        bool saved = false;

        while(userManager.userLocation.Item1 == 0)
        {
            yield return new WaitForSeconds(1f);
        }

        // upgrade Cryptid
        if(HTTP_Request.Get(userManager.userLocation.Item1, userManager.userLocation.Item2) != null)
        {
            HTTP_Request.Put(userManager.userLocation.Item1, userManager.userLocation.Item2, i);
            saved = true;
        }

        if (saved == false)
        {
            HTTP_Request.Put(i, DateTime.Now.ToString(), userManager.userLocation.Item1, userManager.userLocation.Item2);
            saved = true;
        }
    }

    public (GameObject, String) findCryptid((float, float) userLocation)
    {
        fetchC(userLocation);
        return (fetchedCryptid);
    }

    private IEnumerator fetchC((float, float) loc)
    {
        HTTP_Request.Get(loc.Item1, loc.Item2);
        yield return new WaitForSeconds(0.5f);
        fetchedCryptid = (library[HTTP_Request.data.id], HTTP_Request.data.loc);
    } 
}