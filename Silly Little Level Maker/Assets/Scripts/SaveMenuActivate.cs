using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuActivate : MonoBehaviour
{
    public GameObject saveMenuUI;
    public void SaveMenu()
    {
        saveMenuUI.SetActive(!saveMenuUI.activeSelf);
        //Time.timeScale = 0f;
    }
}
