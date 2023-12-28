using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionary.Scripts;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class cryptidGen : MonoBehaviour
{
    public UserManager UserManager;
    public serverRep serverRep;
    public int randomChance;
    public bool exists;

    public Transform BL;
    public Transform TR;
    public ARPlaneManager planeManager;

    public List<GameObject> list = new List<GameObject>();
    public List<GameObject> upgradeKey = new List<GameObject>();
    public List<GameObject> upgradeItem = new List<GameObject>();
 
    private Dictionary<GameObject, GameObject> upgrades = new Dictionary<GameObject, GameObject>();
    private GameObject currentCryptid = null;

    private void Start()
    {
        for(int i = 0; i < upgradeKey.Count; i++)
        {
            upgrades[upgradeKey[i]] = upgradeItem[i];
        }
    }

    public void createCryptid()
    {
        if (currentCryptid)
        {
            GameObject.Destroy(currentCryptid);
        }

        int i = UnityEngine.Random.Range(0, list.Count);
        GameObject cryptid = list[i];

        while (cryptid.GetComponent<cryptid>().scoreCheck < UserManager.getScore())
        {
            if (upgrades.ContainsKey(cryptid))
            {
                int upgradeRandom = UnityEngine.Random.Range(0, 100);

                if (upgradeRandom > randomChance)
                {
                    cryptid = upgrades[cryptid];
                }
            }
            else
            {
                break;
            }
        }

        cryptid.GetComponent<cryptid>().setLocationFromCurrent();

        if (!cryptid.GetComponent<cryptid>().floor)
        {
            cryptid.transform.position = new Vector3(UnityEngine.Random.Range(BL.position.x, TR.position.x), UnityEngine.Random.Range(BL.position.y, TR.position.y), UnityEngine.Random.Range(BL.position.z, TR.position.z));

            currentCryptid = Instantiate(cryptid, transform);
        }
        else
        {
            Vector3 floor = transform.position;
            foreach (var plane in planeManager.trackables)
            {
                floor = plane.transform.position;
            }

            cryptid.transform.position = floor;
            currentCryptid = Instantiate(cryptid, transform);
        }

        exists = true;


    }

    public void createSpecificCryptid(string name)
    {
        GameObject cryptid = null;

        foreach(GameObject nextCryptid in list)
        {
            if(nextCryptid.name == name)
            {
                cryptid = nextCryptid;
                break;
            }
        }


        if (!cryptid.GetComponent<cryptid>().floor)
        {
            cryptid.transform.position = new Vector3(UnityEngine.Random.Range(BL.position.x, TR.position.x), UnityEngine.Random.Range(BL.position.y, TR.position.y), UnityEngine.Random.Range(BL.position.z, TR.position.z));

            currentCryptid = Instantiate(cryptid, transform);
        }
        else
        {
            Vector3 floor = transform.position;
            foreach (var plane in planeManager.trackables)
            {
                floor = plane.transform.position;
            }

            cryptid.transform.position = floor;
            currentCryptid = Instantiate(cryptid, transform);
        }

        serverRep.addCryptid(cryptid);
        exists = true;
    }
    
    public void DeleteCryptid()
    {
        GameObject.Destroy(currentCryptid);
        exists = false;
    }

    public string getCurrentCryptidName()
    {
        return currentCryptid.name;
    }    
}
