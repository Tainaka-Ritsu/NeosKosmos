using KinglandStudio.NeosKosmos.Data.BasicValue;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.BasicDataType
{
    [Serializable]
    [SerializeField]
    public class Scenes
    {
        public static Scenes empty = new(-114514, "EmptySceneSample", new());
        //private int readId = 0;
        //private bool allClear = false;
        public int sceneId;
        public int playDialogueId = -1;
        public string sceneName;
        public List<Dialogue> dialogues = new();
        public Dialogue NextDialogue()
        {
            if(playDialogueId == -1)
            {
                playDialogueId = 0;
                return dialogues[playDialogueId];
            }
            Dialogue next = dialogues[playDialogueId].subsequent;
            if(next == null)
            {
                return Dialogue.empty;
            }
            playDialogueId = next.inSceneId;
            return next;
        }
        public Dialogue JumpDialogue(int dialogueInSceneCount, Dialogue pre) //按钮的操作事件
        {
            while(pre.displayType == "Button")
            {
                pre = pre.precursor;
            }
            playDialogueId = dialogueInSceneCount;
            Dialogue dialogue = dialogues[playDialogueId];
            dialogue.precursor = pre;
            pre.subsequent = dialogue;
            return dialogue;
        }
        public bool Equals(Scenes a)
        {
            return sceneId == a.sceneId && sceneName == a.sceneName;
        }
        public Scenes(int sceneId, string sceneName, List<Dialogue> dialogues) //神奇の构造函数
        {
            this.sceneId = sceneId;
            this.sceneName = sceneName;
            this.dialogues = dialogues;
        }
    }
}