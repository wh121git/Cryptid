using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cryptidGen : MonoBehaviour
{
    //public List<GameObject> list = new List<GameObject>();

    public List<GameObject> list = new List<GameObject>();

    private bool created = false;

    private void Update()
    {
        if(!created)
        {
            Instantiate(list[0],transform);
            created = true;
        }
    }
}
