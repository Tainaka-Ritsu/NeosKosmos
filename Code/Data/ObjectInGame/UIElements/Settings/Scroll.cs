using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI.Settings;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.UI.Settings
{
    public class Scroll : SettingUiBasic
    {
        public int aftpoint;
        public TMP_Text valShowing;
        Slider slider;
        public override void ChangeValue()
        {
            settingManager.ResetValueInGame(variable, Math.Round(slider.value, aftpoint));
            valShowing.text = Math.Round(slider.value, aftpoint).ToString();
        }
        public override void InitStatus()
        {
            slider = GetComponent<Slider>();
            slider.value = (float)val;
            valShowing.text = val.ToString();
        }
    }
}
