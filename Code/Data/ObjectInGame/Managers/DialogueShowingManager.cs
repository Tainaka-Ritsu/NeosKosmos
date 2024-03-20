using KinglandStudio.NeosKosmos.Controller;
using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.DetailedAction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class DialogueShowingManager : MonoBehaviour
    {
        public GameObject sayingImage;
        public GameObject text;
        public SceneShowingManager showingManager;
        public ResoucesManager resoucesManager;
        public Dialogue playing;
        public List<Coroutine> waitForCoroutine = new();
        public List<bool> trigEnd = new();
        public List<float> trigTimes = new();
        public TMP_Text textFrame;
        public TMP_Text sayingCharacterName;
        public Sprite backGroundImage;
        public Color backGroundColor;
        public int executeIndex;
        public int dialogueCnt = 0;
        public Coroutine outputCouroutine;
        private Coroutine actionTrigCoroutine;
        [SerializeField]
        private List<float> alpha = new();
        private float remainTime;
        private int headChara = -1;
        private int tailChara = -1;
        private bool hasDestroyed = false;
        public AbnormalStatus NextDialogue(bool oversee = true)
        {
            if(playing.displayType != "Button")
            {
                if (outputCouroutine != null)
                {
                    return ShowAllText();
                }
                StopAllCoroutines();
                actionTrigCoroutine = null;
                for (int i = 0; i < playing.actions.Count && playing.actions != null; i++)
                {
                    if (!trigEnd[i])
                    {
                        playing.actions[i].QuickEnd();
                    }
                }
            }
            else
            {
                if (oversee)
                {
                    return AbnormalStatus.Regular;
                }
            }
            AbnormalStatus status = showingManager.Next();
            if (status == AbnormalStatus.Regular)
            {
                dialogueCnt++;
                playing = showingManager.nowDialogue;
                if (playing.displayType == "Button")
                {
                    dialogueCnt--;
                    InitButton(playing);
                    return status;
                }
                if (playing.text == "")
                {
                    dialogueCnt--;
                }
                StartNewDialogue();
            }
            return status;
        }
        public void JumpNextDialogue()
        {
            if (playing.displayType == "Button")
            {
                return;
            }
            if (outputCouroutine != null)
            {
                ShowAllText();
                return;
            }
            for (int i = 0; i < playing.actions.Count; i++)
            {
                if (!trigEnd[i])
                {
                    playing.actions[i].QuickEnd();
                }
            }
            AbnormalStatus status = showingManager.Next();
            if (status == AbnormalStatus.Regular)
            {
                dialogueCnt++;
                trigEnd.Clear();
                playing = showingManager.nowDialogue;
                if (playing.text == "")
                {
                    dialogueCnt--;
                }
                if (playing.displayType == "Button")
                {
                    dialogueCnt--;
                    InitButton(playing);
                    return;
                }
                textFrame.text = playing.text;
                textFrame.maxVisibleCharacters = playing.text.Length;
                SetWhoSaidFrame();
                for (int i = 0; i < playing.actions.Count; i++)
                {
                    trigEnd.Add(true);
                    playing.actions[i].QuickEnd();
                }
            }
        }
        public void SetWhenTransmationStart()
        {
            textFrame.text = playing.text;
            SetWhoSaidFrame();
        }
        private void InitButton(Dialogue but)
        {
            List<Dialogue> buttons = new();
            Dialogue next = but;
            buttons.Add(but);
            while (true)
            {
                next = next.subsequent;
                next.precursor = but.precursor;
                buttons.Add(next);
                if(next.who == "ButtonEnd")
                {
                    break;
                }
            }
            showingManager.displayManager.SetButtons(buttons);
        }
        private void StartNewDialogue()
        {
            trigEnd.Clear();
            alpha.Clear();
            waitForCoroutine.Clear();
            trigTimes.Clear();
            foreach (var item in playing.actions)
            {
                trigTimes.Add(Time.time + item.trigWaitTime);
                trigEnd.Add(false);
            }
            remainTime = 0;
            textFrame.maxVisibleCharacters = playing.text.Length;
            textFrame.text = playing.text;
            executeIndex = 0;
            headChara = tailChara = -1;
            SetWhoSaidFrame();
            outputCouroutine = StartCoroutine(OutPutCharecter());
            actionTrigCoroutine = StartCoroutine(TrigActions());
        }
        private AbnormalStatus ShowAllText()
        {
            StopAllCoroutinesByMyself();
            textFrame.text = playing.text;
            textFrame.maxVisibleCharacters = playing.text.Length;
            for (int i = 0; i < textFrame.text.Length; i++)
            {
                SetCharacterAlpha(i, 255);
            }
            textFrame.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            if (resoucesManager.basicValues.quickEndVoiceWithAction)
            {
                StopAllCoroutines();
                actionTrigCoroutine = null;
                for (int i = 0; i < playing.actions.Count; i++)
                {
                    if (!trigEnd[i])
                    {
                        playing.actions[i].QuickEnd();
                    }
                }
            }
            return AbnormalStatus.Regular;
        }
        private void SetWhoSaidFrame()
        {
            if (playing.who != null && playing.who != "")
            {
                sayingCharacterName.gameObject.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                sayingCharacterName.text = playing.who;
            }
            else
            {
                sayingCharacterName.gameObject.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                sayingCharacterName.text = "";
            }
            if(playing.text == null || playing.text == "")
            {
                text.SetActive(false);
            }
            else
            {
                text.SetActive(true);
            }
            if(playing.sayingImage != null && playing.sayingImage != "")
            {
                sayingImage.GetComponent<CanvasGroup>().alpha = 1;
                sayingImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Picture/Character/{playing.sayingImage}/{playing.targetIndex}");
            }
            else
            {
                sayingImage.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        private IEnumerator TrigActions()
        {
            while(!(trigTimes.Count == 0))
            {
                for (int i = executeIndex; i < trigTimes.Count; i++)
                {
                    if (Time.time >= trigTimes[i])
                    {
                        yield return StartCoroutine(playing.actions[i].Trig());
                        executeIndex = i + 1;
                        continue;
                    }
                    break;
                }
                yield return null;
            }
            yield break;
        }
        private IEnumerator OutPutCharecter()
        {
            textFrame.ForceMeshUpdate();
            if (playing.text.Length == 0)
            {
                outputCouroutine = null;
                yield break;
            }
            for (int i = 0; i < playing.text.Length; i++)
            {
                alpha.Add(0);
                SetCharacterAlpha(i, 0);
            }
            while (!(tailChara + 1 == textFrame.text.Length && alpha[playing.text.Length - 1] == BasicValueInGame.wordMaxAlpha))
            {
                if(playing.text != textFrame.text)
                {
                    textFrame.text = playing.text;
                }
                textFrame.ForceMeshUpdate();
                float lastFrameTIme = remainTime + Time.deltaTime;
                while(lastFrameTIme >= resoucesManager.basicValues.wordAppearInterval && tailChara < playing.text.Length - 1)
                {
                    tailChara++;
                    lastFrameTIme -= resoucesManager.basicValues.wordAppearInterval;
                }
                float t = Time.deltaTime / resoucesManager.basicValues.wordFadeInTime;
                for (int i = 0; i < textFrame.text.Length; i++)
                {
                    if (i > headChara && i <= tailChara)
                    {
                        alpha[i] += t * BasicValueInGame.wordMaxAlpha;
                    }
                    SetCharacterAlpha(i, playing.text[i] != ' ' ? Convert.ToByte(Mathf.Clamp(alpha[i], 0, 255)) : (byte)0);
                    if (alpha[i] >= BasicValueInGame.wordMaxAlpha && textFrame.text[i] != ' ')
                    {
                        headChara = i;
                        alpha[i] = BasicValueInGame.wordMaxAlpha;
                    }
                }
                textFrame.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                remainTime = lastFrameTIme;
                yield return null;
            }
            for (int i = 0; i < textFrame.text.Length; i++)
            {
                SetCharacterAlpha(i, 255);
            }
            textFrame.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            outputCouroutine = null;
            yield break;
        }
        private void ResetScene()
        {
            textFrame.maxVisibleCharacters = playing.text.Length;
            textFrame.text = playing.text;
            SetWhoSaidFrame();
            textFrame.ForceMeshUpdate();
            for (int i = 0; i < playing.text.Length; i++)
            {
                SetCharacterAlpha(i, playing.text[i] != ' ' ? (byte)255 : (byte)0);
            }
            textFrame.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            playing.objectsUsedCount = -1;
            showingManager.CompareCharacterToGameobject();
            if (backGroundImage != null)
            {
                GameObject.Find("0").GetComponent<Image>().sprite = backGroundImage;
            }
            else if(backGroundColor != new Color(0, 0, 0, 0))
            {
                GameObject.Find("0").GetComponent<Image>().color = backGroundColor;
            }
            for (int i = 0; i < playing.actions.Count && playing.actions != null; i++)
            {
                Position2 start;
                if (playing.actions.GetType().Name == "SmoothTranslate")
                {
                    start = (playing.actions[i] as SmoothTranslate).start;
                    playing.actions[i].SetArgument();
                    (playing.actions[i] as SmoothTranslate).start = start;
                    playing.actions[i].QuickEnd();
                    continue;
                }
                else
                {
                    playing.actions[i].SetArgument();
                }
                if (trigEnd[i])
                {
                    continue;
                }
                else if( playing.actions.GetType().Name == "CreateCharacter" && !trigEnd[i])
                {
                    playing.actions[i].SetArgument();
                    continue;
                }
                playing.actions[i].QuickEnd();
            }
            //GetComponent<DisplayManager>().UpdateCharacterStatus();
        }
        private void SetCharacterAlpha(int index, byte alpha)
        {
            var materialIndex = textFrame.textInfo.characterInfo[index].materialReferenceIndex;
            var vertexColors = textFrame.textInfo.meshInfo[materialIndex].colors32;
            var vertexIndex = textFrame.textInfo.characterInfo[index].vertexIndex;
            vertexColors[vertexIndex].a = alpha;
            vertexColors[vertexIndex + 1].a = alpha;
            vertexColors[vertexIndex + 2].a = alpha;
            vertexColors[vertexIndex + 3].a = alpha;
        }
        public void StopAllCoroutinesByMyself()
        {
            StopAllCoroutines();
            outputCouroutine = null;
            actionTrigCoroutine = null;
        }
        private void Awake()
        {
            if(GameObject.Find("VisualManager") != gameObject && gameObject != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                hasDestroyed = true;
            }
        }
        private void Start()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }
        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name == "BasicInGame" && !hasDestroyed && GameObject.Find("VisualManager") == gameObject)
            {
                textFrame = GameObject.Find("Dailogue").GetComponent<TMP_Text>();
                sayingCharacterName = GameObject.Find("Said").GetComponent<TMP_Text>();
                sayingImage = GameObject.Find("Header");
                text = GameObject.Find("DialogueFrame");
                ResetScene();
            }
        }
    }
}