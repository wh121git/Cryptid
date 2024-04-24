using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class reparent : MonoBehaviour
{
    public GameObject originalParent;
    public GameObject oldChild;

    public GameObject newChild;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");
        newChild.transform.position = oldChild.transform.position;
        newChild.GetComponent<Transform>().SetParent(originalParent.GetComponent<Transform>());
        GameObject.Destroy(oldChild);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
