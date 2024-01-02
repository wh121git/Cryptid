using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class serverRep : MonoBehaviour
{
    public List<GameObject> cryptids = new List<GameObject>();

    public List<(float, float)> locations = new List<(float, float)>();

    private void Start()
    {
        if (PlayerPrefs.GetFloat("loc" + 0 + "x", 0f) != 0f)
        {
            int i = 0;
            while (PlayerPrefs.GetFloat("loc" + i + "x", 0f) != 0f)
            {
                locations.Add((PlayerPrefs.GetFloat("loc" + i + "x"), PlayerPrefs.GetFloat("loc" + i + "y")));
                i++;
            }
        }
        else
        {
            setObjects();
            saveLoc();
            savePower();
        }

    }

    public void setObjects()
    {
        locations.Add((51.98000f, -2.06000f));
        locations.Add((51.99000f, -2.06000f));
        locations.Add((51.97000f, -2.06000f));

        for(int i = 0; i < cryptids.Count; i++)
        {
            cryptids[i].GetComponent<cryptid>().setLocation(locations[i].Item1, locations[i].Item2);
            cryptids[i].GetComponent<cryptid>().setPower(PlayerPrefs.GetInt("pow" + i));
        }
    }

    public void saveLoc()
    {
        int i = 0;

        foreach ((float, float) loc in locations)
        {
            PlayerPrefs.SetFloat("loc" + i + "x", loc.Item1);
            PlayerPrefs.SetFloat("loc" + i + "y", loc.Item2);
            i++;
        }
    }

    public void savePower()
    {
        int i = 0;

        foreach(GameObject cryptid in cryptids)
        {
            PlayerPrefs.SetInt("pow" + i, cryptid.GetComponent<cryptid>().getPower());
        }
    }

    public void addCryptid(GameObject cryptid)
    {
        cryptids.Add(cryptid);
        locations.Add(cryptid.GetComponent<cryptid>().getLocation());

        saveLoc();
        savePower();
    }

    public GameObject findCryptid((float, float) location)
    {
        foreach(GameObject cryptid in cryptids)
        {
            if(cryptid.GetComponent<cryptid>().getLocation() == location)
            {
                return cryptid;
            }
        }

        return null;
    }

    public void removeCryptid((float,float) location)
    {
        for(int i = 0; i < cryptids.Count; i++)
        {
            if (locations[i] == location)
            {
                cryptids.RemoveAt(i);
                locations.RemoveAt(i);

                saveLoc();
                savePower();
                break;
            }
        }
    }
}
