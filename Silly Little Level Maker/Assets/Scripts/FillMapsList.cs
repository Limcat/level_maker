using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FillMapsList : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    void Start()
    {
        var db = new Database();
        var mapNames = db.GetMapNames();
        dropdown.ClearOptions();

        foreach (string mapName in mapNames)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = mapName;
            dropdown.options.Add(item);
        }
    }

    public void LoadLevelButton()
    {
        Dropdown.OptionData item = dropdown.options[dropdown.value];
        Database.mapName = item.text;

        Debug.Log("Active item: " + item.text);

        SceneManager.LoadScene(1);
    }
}
