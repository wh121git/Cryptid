using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreDis;
    public UserManager userManager;

    public void scoreSet()
    {
        scoreDis.text = userManager.displayScore().ToString();
    }

    private void Update()
    {
        scoreSet();
    }
}
