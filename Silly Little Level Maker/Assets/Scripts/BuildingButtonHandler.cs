using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour
{
    [SerializeField] BuildingObjectBase item;
    Button button;
    [SerializeField] BuildingCreator buildingCreator;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        //buildingCreator = BuildingCreator.GetInstance();
    }

    private void ButtonClicked()
    {
        Debug.Log(item.name + " clicked.");
        buildingCreator.ObjectSelected(item);
    }
}
