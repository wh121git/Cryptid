using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AndroidSaving : MonoBehaviour
{
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("test", "this is a test");
    }

    // Update is called once per frame
    void Update()
    {
        UI.GetComponent<Text>().text = PlayerPrefs.GetString("test");
    }
}
