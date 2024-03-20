using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace KinglandStudio.NeosKosmos.Controller
{
    public class Backlog : MonoBehaviour
    {
        private GameObject[] logs = new GameObject[4];
        private List<Dialogue> queue = new();
        private Coroutine coroutine;
        private SceneShowingManager sceneManager;
        public BacklogScrollbar scrollbar;
        public int maxCnt = 0;
        public int nowCnt = 0;
        public List<Dialogue> allPass;
        public GameObject scroll;
        public GameObject bar;
        public void ButtonDown(int step)
        {
            coroutine = StartCoroutine(Coroutines(step));
        }
        public void ButtonUp()
        {
            StopCoroutine(coroutine);
        }
        private IEnumerator Coroutines(int step)
        {
            float allTime = 0f, waitTime  = 0f;
            while (true)
            {
                scrollbar.Set(nowCnt - 4 - step);
                waitTime = BasicValueInGame.GetLongPressInterval(allTime);
                allTime += waitTime;
                yield return new WaitForSeconds(waitTime);
            }
        }
        public void NewOne(int addIndex)
        {
            if(addIndex < 0)
            {
                OldOne(-addIndex);
                return;
            }
            while (addIndex != 0)
            {
                if(nowCnt <= 4)
                {
                    break;
                }
                Dialogue dia = queue[^1].precursor;
                while(dia.text == "" || dia.displayType == "Button")
                {
                    dia = dia.precursor;
                }
                queue.Add(dia);
                queue.RemoveAt(0);
                addIndex--;
                nowCnt--;
            }
            Show();
        }
        private void OldOne(int addIndex)
        {
            while (addIndex != 0)
            {
                if (nowCnt == maxCnt)
                {
                    break;
                }
                Dialogue dia = queue[0].subsequent;
                while (dia.text == "" || dia.displayType == "Button")
                {
                    dia = dia.subsequent;
                }
                queue.Insert(0, dia);
                queue.RemoveAt(queue.Count - 1);
                addIndex--;
                nowCnt++;
            }
            Show();
        }
        private void Show()
        {
            if(queue.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    logs[3 - i].GetComponent<Log>().SetInfor(queue[i]);
                }
            }
            else
            {
                for(int i = 0; i < queue.Count; i++)
                {
                    logs[queue.Count - i - 1].GetComponent<Log>().SetInfor(queue[i]);
                }
                for(int i = queue.Count; i < 4; i++)
                {
                    logs[i].GetComponent<Log>().SetInfor(Dialogue.empty);
                }
            }
        }
        private void OnEnable()
        {
            queue.Clear();
            nowCnt = 0;
            Dialogue nowDialogue = sceneManager.nowDialogue;
            maxCnt = GameObject.Find("VisualManager").GetComponent<DialogueShowingManager>().dialogueCnt;
            while (nowDialogue != null)
            {
                if (queue.Count == 4 || nowCnt == maxCnt)
                {
                    break;
                }
                while(nowDialogue.text == "" || nowDialogue.displayType == "Button")
                {
                    nowDialogue = nowDialogue.precursor;
                }
                nowCnt++;
                queue.Add(nowDialogue);
                nowDialogue = nowDialogue.precursor;
            }
            nowCnt = maxCnt;
            Show();
        }
        private void Awake()
        {
            sceneManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
            for(int i = 0; i < 4; i++)
            {
                logs[i] = scroll.transform.GetChild(i).gameObject;
            }
        }
    }
}
