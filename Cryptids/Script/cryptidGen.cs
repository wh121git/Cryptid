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

/*
 * Code sources:
 * [1] Basic save and load functionality: https://gamedev.stackexchange.com/questions/115863/how-to-save-variables-into-a-file-unity
 */
public class cryptidGen : MonoBehaviour
{
    public UserManager UserManager;
    public int randomChance;

    public Transform BL;
    public Transform TR;
    public ARPlaneManager planeManager;

    public List<GameObject> list = new List<GameObject>();
    public List<GameObject> upgradeKey = new List<GameObject>();
    public List<GameObject> upgradeItem = new List<GameObject>();
 
    private Dictionary<GameObject, GameObject> upgrades = new Dictionary<GameObject, GameObject>();
    private GameObject currentCryptid = null;

    //[1]
    [Serializable]
    public class SaveFile
    {
        // Time since last open
        private DateTime time;
        // In game score
        private int saveScore;


        // Gets and sets
        public DateTime getTime(){ return time;}
        public void setTime(DateTime newTime) {  time = newTime; }

        public int getSaveScore() { return saveScore;}
        public void setSaveScore(int newSaveScore) { saveScore = newSaveScore; }
    }

    public void Save(SaveFile saveFile)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, saveFile);
    }

    public SaveFile Load()
    {
        if(File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveFile saveFile = (SaveFile)bf.Deserialize(file);
            file.Close();
            return saveFile;
        }
        else
        {
            Debug.Log("No File");
            return null;
        }
    }

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
    }    
}
