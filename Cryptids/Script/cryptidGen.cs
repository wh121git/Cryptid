using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionary.Scripts;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class cryptidGen : MonoBehaviour
{
    public UserManager UserManager;
    public int randomChance;

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

        createCryptid();
    }

    public void createCryptid()
    {
        Debug.Log("test");
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

        currentCryptid = Instantiate(cryptid, transform);
    }    
}
