using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class ResoucesManager : MonoBehaviour
    {
        private List<Actions> actions;
        private int loadCount;
        [SerializeField]
        private string[] res;
        public BasicValueInGame basicValues = new();
        public GameObject popupWindows;
        public List<Character> allCharacters = new();
        public static List<T> GetClassInsBySuperType<T>()
        {
            var type = typeof(T);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(s => s.GetTypes());
            var allChildClass = allTypes.Where(t => ((t != type) && type.IsAssignableFrom(t)));
            var allIns = allChildClass.Select(t => (T)Activator.CreateInstance(t)).ToList();
            return allIns;
        }
        public void ShowErrorMessage(AbnormalStatus status)
        {
            GetComponent<UserOperatingManager>().enabled = false;
            GameObject window = GameObject.Instantiate(popupWindows, GameObject.Find("CanvasUI").transform);
            StartCoroutine(window.GetComponent<PopUp>().Init("非常抱歉(っ °Д °;)っ", $"一些错误已经发生\n错误类型:{status}\n错误代码:0x{Convert.ToInt64(status):X}\n程序将终止，请在查找问题后重新启动", "终止程序"));
        }
        public void GetNewScene(int sceneId, Dialogue lastSceneLastDialogue)
        {
            List<Dialogue> dia = new();
            string a = Resources.Load<TextAsset>($"Scenario/{sceneId}").text;
            res = a.Split('\n');
            string name = res[0];
            string id = res[1];
            loadCount = 1;
            for (int i = 2; i < res.Length; i++)
            {
                if(i == 2)
                {
                    dia.Add(InitDialogue(res[i]));
                    dia[^1].precursor = lastSceneLastDialogue;
                    if (lastSceneLastDialogue != null)
                    {
                        lastSceneLastDialogue.subsequent = dia[^1];
                    }
                }
                else
                {
                    dia.Add(InitDialogue(res[i]));
                    dia[^1].precursor = dia[^2];
                    dia[^2].subsequent = dia[^1];
                }
                if (dia[^1] == Dialogue.empty)
                {
                    GetComponent<SceneShowingManager>().nowScene = Scenes.empty;
                    return;
                }
                dia[^1].inSceneId = i - 2;
                loadCount++;
            }
            GetComponent<SceneShowingManager>().nowScene = new(Convert.ToInt32(id), name, dia);
            GetComponent<SceneShowingManager>().ResetUsingId();
            GetComponent<SceneShowingManager>().OnNewSceneStart();
            return;
        }
        public Dialogue InitDialogue(string arg)
        {
            int dis = 0;
            string displayType, saidPerson, text, act;
            string[] lastRow, row;
            List<Actions> trigger = new();
            List<string> obj = new();
            string[] thr = arg.Split(BasicValueInGame.divideChara);
            if(thr.Length != 4)
            {
                return Dialogue.empty;
            }
            displayType = thr[0];
            saidPerson = thr[1];
            if(saidPerson == "Null")
            {
                saidPerson = "";
            }
            text = thr[2];
            if(text == "Null")
            {
                text = "";
            }
            act = thr[3];
            Dialogue dia = new(displayType, text, null, saidPerson);
            dia.SetSayingImage();
            string[] all = act.Replace("\r", "").Replace("\n", "").Split(BasicValueInGame.actBetween);
            foreach (var ac in all)
            {
                lastRow = ac.Split(BasicValueInGame.nameAndArgDevide);
                if(lastRow[0] != "Null")
                {
                    Type type = GetActions(lastRow[0]).GetType();
                    if (type == null)
                    {
                        return Dialogue.empty;
                    }
                    trigger.Add(Activator.CreateInstance(type) as Actions);
                    trigger[^1].trigWaitTime = (float)Convert.ToDouble(lastRow[2]);
                    trigger[^1].parent = dia;
                    trigger[^1].index = dis;
                    row = lastRow[1].Split(BasicValueInGame.argDevide);
                    foreach (var item in row)
                    {
                        obj.Add(item);
                    }
                }
                dis++;
            }
            dia.actions = trigger;
            dia.objects = obj;
            return dia;
        }
        public Character NewCharacter(int id)
        {
            return allCharacters.Find(x => x.id == id);
        }
        public void SetValueBasic()
        {
            basicValues.basic = basicValues;
        }
        public void ResetInformation()
        {
            Screen.SetResolution(basicValues.width, basicValues.height, basicValues.fullScreen);
        }
        private Actions GetActions(string arg)
        {
            foreach(var act in actions)
            {
                if(act.GetType().Name == arg)
                {
                    return act;
                }
            }
            return null;
        }
        private void Awake()
        {
            if (GameObject.Find("VisualManager") == gameObject)
            {
                actions = GetClassInsBySuperType<Actions>();
                GetComponent<SceneShowingManager>().Init();
            }
        }
    }
}