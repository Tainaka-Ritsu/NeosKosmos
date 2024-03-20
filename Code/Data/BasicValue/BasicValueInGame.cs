using KinglandStudio.NeosKosmos.Data.BasicDataType;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.BasicValue
{
    [Serializable]
    [SerializeField]
    public class BasicValueInGame
    {
        public static float GetLongPressInterval(float x)
        {
            float GetConst(float x)
            {
                return (float)(0.1 / (x + 1.5));
            }
            return (float)(0.1f / (3 * x + 0.25) + GetConst(x));
        }
        public void SetArg(string target, object obj)
        {
            var type = basic.GetType().GetField(target);
            type.SetValue(basic, Convert.ChangeType(obj, type.FieldType));
        }
        public BasicValueInGame basic;
        //在用户端可修改的变量
        public int width;//程序宽度
        public int height; //程序高度
        public float mainVolume; //程序主音量
        public bool fullScreen = false;
        public bool jumpVoiceAfterChangeDialogue = false; //下一段文本使是否继续播放上一段语音
        public bool quickEndVoiceWithAction = true; //当正在显示文字时，回车(或左键)是否直接使动作完结
        public float wordAppearInterval = 0.04f; //每个文字出现的间隔
        public float wordFadeInTime = 0.2f; //淡入效果持续时间
        //在Editor里面可修改的变量
        public static int wordMaxAlpha = 255;
        public static float skipIntervalTime = 0.02f; //每次跳过对话的间隔时间
        public static int logMaxDialogueCount = 100; //文本记录中最多可以存放的对白数量
        public static bool jumpAble; //是否可以跳转对白
        //引擎底层变量，你最好别乱动（指）
        public static float enterKeepDownIntervalTime = 0.25f;
        public static int dialoguePreloadMaxCount = 1000;
        public static char divideChara = '¡';
        public static char actBetween = ';';
        public static char nameAndArgDevide = '|';
        public static char argDevide = ',';
        public BasicValueInGame()
        {
            width = 1280;
            height = 720;
            mainVolume = 0.5f;
            fullScreen = false;
            jumpVoiceAfterChangeDialogue = false;
            quickEndVoiceWithAction = true;
            wordAppearInterval = 0.04f;
            wordFadeInTime = 0.2f;
        }
    }
}