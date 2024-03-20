using KinglandStudio.NeosKosmos.Data.BasicValue;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.BasicDataType
{
    [Serializable]
    [SerializeField]
    public class Dialogue
    {
        public static Dialogue empty = new("", "", new(), null);
        public string who = null; //可以为null，使用时需检查
        public string text; //显示的对白内容
        public string displayType; //增加或重启或其他
        public string sayingImage = ""; //左显示头像
        public int inSceneId;
        public int targetIndex;
        internal Dialogue precursor = null;
        internal Dialogue subsequent = null;
        public List<Actions> actions = new(); //设置于此对白上的所有触发器
        public List<string> objects = new(); //一个对话的所有参数组
        public int objectsUsedCount = -1;
        public string GetNewArgument()
        {
            if (objectsUsedCount == objects.Count - 1)
            {
                return null;
            }
            objectsUsedCount++;
            return objects[objectsUsedCount];
        }
        public void SetSayingImage()
        {
            string[] a = who.Split(BasicValueInGame.actBetween);
            if(a.Length == 3)
            {
                who = a[0];
                sayingImage = a[1];
                targetIndex = Convert.ToInt32(a[2]);
            }
        }
        public Dialogue(string displayType, string theText, List<Actions> act, string who = null) //构造函数 theText->对白内容 act->触发器们
        {
            this.displayType = displayType;
            this.who = who;
            text = theText;
            actions = act;
        }
    }
}