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
using System.Diagnostics;
using Unity.Services.Core;
using System.Linq;


public class cryptidGen : MonoBehaviour
{
    // Fetch scripts and functions 
    public DebugDisplay debugDisplay;
    public UserManager UserManager;
    public serverRep serverRep;
    public ARPlaneManager planeManager;

    // public editables
    public int randomChance;
    public Transform BL;
    public Transform TR;
    public List<GameObject> list = new List<GameObject>();
    public List<GameObject> upgradeKey = new List<GameObject>();
    public List<GameObject> upgradeItem = new List<GameObject>();
    
    // Object Cryptid functionality
    private GameObject objTarget;
    private GameObject objNewCryptid;
    private GameObject objOldCryptid;
    
    // Internal function
    public bool exists;

    private Dictionary<GameObject, GameObject> upgrades = new Dictionary<GameObject, GameObject>();
    private GameObject currentCryptid = null;

    private void Start()
    {
        for(int i = 0; i < upgradeKey.Count; i++)
        {
            upgrades[upgradeKey[i]] = upgradeItem[i];
        }
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

    public void setObjTarget(GameObject objTarget)
    {
        
        this.objTarget = objTarget;
    }

    public void setObjNewCryptid(GameObject objNewCryptid)
    {
        this.objNewCryptid = objNewCryptid;
    }

    public void setObjOldCryptid(GameObject objOldCryptid)
    {
        this.objOldCryptid = objOldCryptid;
    }

    public GameObject upgrade(GameObject cryptid)
    {
        // upgrade cryptid
        while (cryptid.GetComponent<cryptid>().checkPower())
        {
            if (upgrades.ContainsKey(cryptid))
            {
                

                int upgradeRandom = UnityEngine.Random.Range(0, 100);

                if (/*upgradeRandom > randomChance*/ true)
                {
                    
                    return upgrades[cryptid];
                }
            }
            else
            {
                return cryptid;
            }
        }

        return cryptid;
    }

    public void createCryptid()
    {
        UnityEngine.Debug.Log("default created");

        // delete current cryptid
        if (currentCryptid)
        {
            GameObject.Destroy(currentCryptid);
        }

        // randomly generate new cryptid
        int i = UnityEngine.Random.Range(0, list.Count);
        GameObject cryptid = list[i];

        // upgrade cryptid
        cryptid = upgrade(cryptid);

        // non-floor cryptid placement
        if (!cryptid.GetComponent<cryptid>().floor)
        {
            cryptid.transform.position = new Vector3(UnityEngine.Random.Range(BL.position.x, TR.position.x), UnityEngine.Random.Range(BL.position.y, TR.position.y), UnityEngine.Random.Range(BL.position.z, TR.position.z));

            currentCryptid = Instantiate(cryptid, transform);
        }
        // floor cyptid placement
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

        serverRep.addToBuffer(cryptid.name);
        debugDisplay.addOut("Default Cryptid Created");
    }

    public void createSpecificCryptid(string name)
    {
        

        // empty space for cryptid
        GameObject cryptid = null;

        // delete current cryptid
        if (currentCryptid)
        {
            GameObject.Destroy(currentCryptid);
        }

        // find cryptid name in list
        foreach (GameObject nextCryptid in list)
        {
            UnityEngine.Debug.Log(nextCryptid.name + " " + name);
            if (nextCryptid.name.Equals(name))
            {
                cryptid = nextCryptid;
                
                break;
            }
        }
        
        //non-floor cryptid placement
        if (!cryptid.GetComponent<cryptid>().floor)
        {
            cryptid.transform.position = new Vector3(UnityEngine.Random.Range(BL.position.x, TR.position.x), UnityEngine.Random.Range(BL.position.y, TR.position.y), UnityEngine.Random.Range(BL.position.z, TR.position.z));

            currentCryptid = Instantiate(cryptid, transform);
        }
        // floor cryptid placement
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

        serverRep.addToBuffer(cryptid.name);
        debugDisplay.addOut("Summon Crytpid from name");
    }

    public void createExistingCryptid(GameObject cryptid, int power)
    {
        UnityEngine.Debug.Log("existing made");
        // increase power of cryptid
        cryptid.GetComponent<cryptid>().setPower(power);
        

        // delete current cryptid
        if (currentCryptid)
        {
            GameObject.Destroy(currentCryptid);
        }

        // upgrade cryptid
        cryptid = upgrade(cryptid);

        //non-floor cryptid placement
        if (!cryptid.GetComponent<cryptid>().floor)
        {
            cryptid.transform.position = new Vector3(UnityEngine.Random.Range(BL.position.x, TR.position.x), UnityEngine.Random.Range(BL.position.y, TR.position.y), UnityEngine.Random.Range(BL.position.z, TR.position.z));

            currentCryptid = Instantiate(cryptid, transform);
        }
        // floor cryptid placement
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

        serverRep.addToBuffer(cryptid.name);
        exists = true;

    }

    public void createObjCryptid()
    {
        StartCoroutine(createObjCryptidEn());
    }

    public IEnumerator createObjCryptidEn()
    {
        yield return new WaitForSeconds(1f);
        
        debugDisplay.addOut(PlayerPrefs.GetInt(objNewCryptid.name).ToString());
        // update power
        int power = PlayerPrefs.GetInt(objNewCryptid.name, 0);
        if(power == 0) { PlayerPrefs.SetInt(objNewCryptid.name, 1); }
        power++;
        PlayerPrefs.SetInt(objNewCryptid.name, power);

        // increase power of cryptid
        objNewCryptid.GetComponent<cryptid>().setPower(power);

        // delete current cryptid
        if (currentCryptid)
        {
            GameObject.Destroy(currentCryptid);
        }

        // upgrade cryptid
        objNewCryptid = upgrade(objNewCryptid);

        currentCryptid = Instantiate(objNewCryptid, objOldCryptid.transform);
        currentCryptid.transform.parent = objTarget.transform;

        debugDisplay.addOut(currentCryptid.name);

        exists = true;
    }
}
