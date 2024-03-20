using KinglandStudio.NeosKosmos.Data.BasicValue;
using UnityEngine;
using KinglandStudio.NeosKosmos.Controller;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
using UnityEngine.XR;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class UserOperatingManager : MonoBehaviour
    {
        public ResoucesManager resoucesManager;
        public SceneShowingManager sceneShowingManager;
        public DisplayManager displayManager;
        public DialogueShowingManager dialogueShowingManager;
        public SceneLoader transitionLoader;
        public bool isAuto;
        private float lastSkip;
        private float lastNext;
        public void NextDialogue()
        {
            AbnormalStatus status = dialogueShowingManager.NextDialogue();
            if(status != AbnormalStatus.Regular)
            {
                resoucesManager.ShowErrorMessage(status);
            }
        }
        public AbnormalStatus NextDialogue(bool a = true)
        {
            return dialogueShowingManager.NextDialogue();
        }
        public AbnormalStatus SkipDialogue()
        {
            dialogueShowingManager.JumpNextDialogue();
            return AbnormalStatus.Regular;
        }
        public void ToSetting()
        {
            dialogueShowingManager.StopAllCoroutinesByMyself();
            SceneManager.LoadScene("Setting");
            GetComponent<UserOperatingManager>().enabled = false;
        }
        public void Update()
        {
            [DllImport("user32.dll")]
            static extern IntPtr GetActiveWindow();
            [DllImport("user32.dll")]
            static extern IntPtr SendMessage(int hWnd, int msg, IntPtr wParam, IntPtr lParam);
            AbnormalStatus DialogueOperatingCheck()
            {
                if ((Input.GetKey(KeyMapping.skipDialogue1) || Input.GetKey(KeyMapping.skipDialogue2)) && Time.time >= lastSkip)
                {
                    lastSkip = Time.time + BasicValueInGame.skipIntervalTime;
                    isAuto = false;
                    return SkipDialogue();
                }
                if (Input.GetKey(KeyMapping.nextDialogue) && Time.time >= lastNext)
                {
                    lastNext = BasicValueInGame.enterKeepDownIntervalTime + Time.time;
                    return NextDialogue(false);
                }
                return AbnormalStatus.Null;
            }
            AbnormalStatus status;
            if (Input.GetKeyDown(KeyMapping.bossComing))
            {
                SendMessage((int)GetActiveWindow(), 0x0112, new(0xF020), new(0)); //Very nice method, this makes my windows rolling.
                //ShowWindow(GetActiveWindow(), 7); this couldn't work
            }
            status = DialogueOperatingCheck();
            if(status != AbnormalStatus.Regular && status != AbnormalStatus.Null)
            {
                resoucesManager.ShowErrorMessage(status);
            }
        }
        private void Start()
        {
            transitionLoader = GameObject.Find("Transition").GetComponent<SceneLoader>();
            DontDestroyOnLoad(gameObject);
        }
    }
}