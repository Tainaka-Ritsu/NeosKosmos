using KinglandStudio.NeosKosmos.Controller;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.UI
{
    public class BacklogScrollbar : MonoBehaviour
    {
        private int nint;
        private Slider slider;
        private Backlog log;
        public void Set(int a)
        {
            slider.value = a;
        }
        public void ValueChange(Single a)
        {
            log.NewOne(-(((int)slider.value) - nint));
            nint = (int)slider.value;
        }
        private void OnEnable()
        {
            log = GameObject.Find("Backlog").GetComponent<Backlog>();
            slider = GetComponent<Slider>();
            slider.maxValue = (log.maxCnt - 4) >= 1 ? log.maxCnt - 4 : 0;
            nint = (int)slider.maxValue;
            slider.value = nint;
        }
    }
}
