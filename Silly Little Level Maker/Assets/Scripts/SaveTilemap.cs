using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTilemap : MonoBehaviour
{
    [SerializeField] BuildingCreator buildingCreator;
    [SerializeField] InputField mapNameInput;
    [SerializeField] Dropdown background;
    public GameObject saveMenuUI;

    public void Save()
    {
        Database db = new Database();
        Debug.Log("Text field: " + mapNameInput.text);

        string backgroundName = background.options[background.value].text;

        db.SaveMap(mapNameInput.text, buildingCreator.GetTiles(), backgroundName);
        saveMenuUI.SetActive(false);
    }
    public void Start()
    {
        mapNameInput.text = Database.mapName;
    }
}
