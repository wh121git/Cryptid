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

    public List<GameObject> list = new List<GameObject>();

    [SerializeField]
    private Dictionary<GameObject, GameObject> upgrades = new Dictionary<GameObject, GameObject>();
   
    private bool created = false;

    private void Update()
    {
        if(!created)
        {
            int i = UnityEngine.Random.Range(0, list.Count);
            GameObject cryptid = list[i];

            while(cryptid.GetComponent<cryptid>().scoreCheck <  UserManager.getScore())
            {
                if (upgrades.ContainsKey(cryptid))
                {
                    cryptid = upgrades[cryptid];
                }
                else
                {
                    break;
                }
            }

            Instantiate(cryptid,transform);
            created = true;
        }
    }
}
