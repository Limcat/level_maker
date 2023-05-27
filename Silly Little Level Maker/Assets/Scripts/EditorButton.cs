using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorButton : MonoBehaviour
{
    public GameObject chooseObjectsUI;

    public void OpenMenu()
    {
        chooseObjectsUI.SetActive(!chooseObjectsUI.activeSelf);
    }
}
