using KinglandStudio.NeosKosmos.Controller;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.UI
{
    [Serializable]
    [SerializeField]
    public class PopUp : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text describtion;
        public TMP_Text acceptText;
        public bool isClick;
        private Coroutine fadeOut;
        public void SetTrue()
        {
            isClick = true;
        }
        public IEnumerator Init(string title, string describtion, string accept)
        {
            this.title.text = title;
            this.describtion.text = describtion;
            this.acceptText.text = accept;
            while (!isClick)
            {
                yield return new WaitForSecondsRealtime(0.05f);
            }
            Destroy(gameObject);
            Application.Quit();
        }
    }
}