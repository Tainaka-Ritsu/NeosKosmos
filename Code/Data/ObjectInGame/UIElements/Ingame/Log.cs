using KinglandStudio.NeosKosmos.Data.BasicDataType;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.UI
{
    public class Log : MonoBehaviour
    {
        public Dialogue dialogue;
        public GameObject sayingImage;
        public TMP_Text who;
        public TMP_Text text;
        public void SetInfor(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            if (dialogue.sayingImage != "")
            {

            }
            else
            {
                sayingImage.SetActive(false);
            }
            if(dialogue.who == null)
            {
                who.text = "";
            }
            else
            {
                who.text = dialogue.who;
            }
            text.text = dialogue.text;
        }
    }
}