using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.BasicDataType
{
    [Serializable]
    [SerializeField]
    public class Actions//任何修改背景，修改背景音乐和角色立绘的动作的触发器的基类
    {
        public Dialogue parent;
        public ResoucesManager resourcesManager;
        public DialogueShowingManager dialogueShowingManager;
        public float trigWaitTime; //从此次对白开始播放后多久触发此触发器 (>= 0)
        public int index;
        public virtual IEnumerator Trig() { yield return new WaitForSecondsRealtime(114514f); } //处理实际效果の函数
        public virtual AbnormalStatus QuickEnd() { return AbnormalStatus.Regular; } //快速终结，适用于跳过等场景
        public virtual AbnormalStatus SetArgument() { return AbnormalStatus.Regular;}
        public bool TypeEquls(System.Object a, System.Object b)
        {
            return a.GetType() == b.GetType();
        }
        //运算符重载，方便就时间进行排序
        public static bool operator <(Actions first, Actions second)
        {
            return first.trigWaitTime < second.trigWaitTime;
        }
        public static bool operator >(Actions first, Actions second)
        {
            return second.trigWaitTime > first.trigWaitTime;
        }
        public void SetStatusFinish()
        {
            if (index >= dialogueShowingManager.trigEnd.Count)
            {
                return;
            }
            dialogueShowingManager.trigEnd[index] = true;
        }
        public void FindOurManager()
        {
            resourcesManager = GameObject.Find("VisualManager").GetComponent<ResoucesManager>();
            dialogueShowingManager = GameObject.Find("VisualManager").GetComponent<DialogueShowingManager>();
        }
    }
}