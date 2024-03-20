using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.UI
{
    public class IngameButton : MonoBehaviour
    {
        public Dialogue dialogue;
        public TMP_Text text;
        public void Trig()
        {
            foreach (var item in dialogue.actions)
            {
                StartCoroutine(item.Trig());
            }
            GameObject.Find("VisualManager").GetComponent<DisplayManager>().RemoveButtons();
        }
        public void Set()
        {
            text.text = dialogue.text;
        }
    }
}