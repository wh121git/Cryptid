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
}
