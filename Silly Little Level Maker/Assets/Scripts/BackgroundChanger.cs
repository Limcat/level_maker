using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] GameObject gameObjectBackground;
    [SerializeField] Sprite dayBackground, nightBackground;
    [SerializeField] Dropdown dropdown;

    public void Start()
    {
        Database db = new Database();
        if (Database.mapName != null)
        {
            string background = db.LoadBackground(Database.mapName);
            switch (background)
            {
                case "Day":
                    dropdown.value = 0;
                    gameObjectBackground.GetComponent<SpriteRenderer>().sprite = dayBackground;
                    break;
                case "Night":
                    dropdown.value = 1;
                    gameObjectBackground.GetComponent<SpriteRenderer>().sprite = nightBackground;
                    break;
                default:
                    break;
            }
        }
    }

    public void Update()
    {
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        switch (dropdown.options[dropdown.value].text)
        {
            case "Day":
                gameObjectBackground.GetComponent<SpriteRenderer>().sprite = dayBackground;
                break;
            case "Night":
                gameObjectBackground.GetComponent<SpriteRenderer>().sprite = nightBackground;
                break;
            default:
                break;
        }
    }
}
