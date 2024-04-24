using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class cryptid : MonoBehaviour
{
    public int powerCap;
    public bool floor;
    public bool rotate;

    public GameObject objUpgrade;


    private int power = 0;

    public void increasePower()
    {
        power++;
    }

    public void setPower(int newPower)
    {
        if (newPower > 0)
        {
            power = power + newPower;
        }
    }

    public bool checkPower()
    {
        return power > powerCap;
    }

    public int getPower()
    {
        return power;
    }

    private Vector3 randRot = new Vector3();

    // Speed Multyplier
    public float rotateSpeed;

    void Start()
    {
        // give random rotation
        randRot = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        // object upgrade
        if(objUpgrade != null)
        {
            if(PlayerPrefs.GetInt(this.name, 0) == 0)
            {
                power++;
                PlayerPrefs.SetInt(this.name, power);
                if (checkPower())
                {
                    objUpgrade.SetActive(true);
                    this.transform.gameObject.SetActive(false);
                }
            }
            else
            {
                PlayerPrefs.SetInt(this.name, 1);
            }
        }
    }

    void Update()
    {
        if(rotate)
        {
            // increase by time
            var step = rotateSpeed * Time.deltaTime;

            // rotate towards
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(randRot.x,randRot.y,randRot.z), step);
        }
    }
}
