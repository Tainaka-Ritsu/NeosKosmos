using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.UI.Settings
{
    public class ResolutionDropdown : MonoBehaviour
    {
        SettingManager settingManager;
        List<Position2> allResolution = new();
        TMP_Dropdown dropdown;
        public void Set()
        {
            settingManager.ResetValueInGame("width", allResolution[dropdown.value].x);
            settingManager.ResetValueInGame("height", allResolution[dropdown.value].y);
        }
        private void Awake()
        {
            int val = 0;
            int setVal = 0;
            dropdown = GetComponent<TMP_Dropdown>();
            settingManager = GameObject.Find("VisualManager").GetComponent<SettingManager>();
            foreach (Resolution res in Screen.resolutions)
            {
                dropdown.options.Add(new(res.ToString()));
                allResolution.Add(new(res.width, res.height));
                if (Screen.width == res.width && Screen.height == res.height)
                {
                    setVal = val;
                }
                val++;
            }
            dropdown.value = setVal;
        }
    }
}
