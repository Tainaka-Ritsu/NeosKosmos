using KinglandStudio.NeosKosmos.Data.BasicValue;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class EditorVariableManager : MonoBehaviour
    {
        [Serialize]
        private Hashtable hash = new();
        public System.Object GetValue(string valueName)
        {
            if (hash.Contains(valueName))
            {
                return hash[valueName];
            }
            return AbnormalStatus.Value_Not_Found;
        }
        public AbnormalStatus AddValue(string valueName, string value)
        {
            if(valueName == null)
            {
                return AbnormalStatus.Value_Not_Found;
            }
            if (hash.Contains(valueName))
            {
                hash[valueName] = value;
            }
            else
            {
                hash.Add(valueName, value);
            }
            return AbnormalStatus.Regular;
        }
    }
}