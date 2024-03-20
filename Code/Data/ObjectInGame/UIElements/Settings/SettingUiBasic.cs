using KinglandStudio.NeosKosmos.Data.Manager;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.UI.Settings
{
    public class SettingUiBasic : MonoBehaviour
    {
        protected object val;
        protected string variable;
        protected SettingManager settingManager;
        public virtual void ChangeValue() { }
        public virtual void InitStatus() { }
        private void Awake()
        {
            settingManager = GameObject.Find("VisualManager").GetComponent<SettingManager>();
            variable = gameObject.name;
            val = GameObject.Find("VisualManager").GetComponent<ResoucesManager>().basicValues.GetType().GetField(variable).GetValue(GameObject.Find("VisualManager").GetComponent<ResoucesManager>().basicValues);
            InitStatus();
        }
    }
}