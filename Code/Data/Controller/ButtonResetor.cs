using KinglandStudio.NeosKosmos.Data.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.Controller
{
    public class ButtonResetor : MonoBehaviour
    {
        GameObject managers;
        Button button;
        void Start()
        {
            button = GetComponent<Button>();
            managers = GameObject.Find("VisualManager");
            switch (name)
            {
                case "Setting":
                    button.onClick.AddListener(managers.GetComponent<UserOperatingManager>().ToSetting);
                    break;
                case "Mask":
                    button.onClick.AddListener(managers.GetComponent<UserOperatingManager>().NextDialogue);
                    break;
                case "Return":
                    button.onClick.AddListener(managers.GetComponent<SettingManager>().ReturnToGameScene);
                    break;
                case "Quit":
                    button.onClick.AddListener(managers.GetComponent<SettingManager>().Quit);
                    break;
                default:
                    break;
            }
        }
    }
}
