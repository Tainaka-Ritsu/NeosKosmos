using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI.Settings;
using System;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.UI.Settings
{
    public class CheckButton : SettingUiBasic
    {
        Toggle toggle;
        public override void ChangeValue()
        {
            settingManager.ResetValueInGame(variable, toggle.isOn);
        }
        public override void InitStatus()
        {
            toggle = GetComponent<Toggle>();
            toggle.isOn = (bool)val;
        }
    }

}