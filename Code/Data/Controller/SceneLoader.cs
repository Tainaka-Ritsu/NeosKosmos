using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.Controller
{
    public class SceneLoader : MonoBehaviour 
    {
        public GameObject targetObject = null;
        public Animator animator = null;
        public ResoucesManager resoucesManager;
        public DialogueShowingManager dialogueShowingManager;
        public SceneShowingManager sceneShowingManager;
        private void GetAllArguments(AnimationType type)
        {
            targetObject = gameObject.transform.Find(type.ToString()).gameObject;
            targetObject.GetComponent<Animator>().enabled = true;
            animator = targetObject.GetComponent<Animator>();
        }
        private void ReloadManagers()
        {
            resoucesManager = GameObject.Find("VisualManager").GetComponent<ResoucesManager>();
            dialogueShowingManager = GameObject.Find("VisualManager").GetComponent<DialogueShowingManager>();
            sceneShowingManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        }
        public void StopAnimationPlay()
        {
            targetObject.GetComponent<Animator>().enabled = false;
            targetObject.GetComponent<CanvasGroup>().alpha = 0;
            targetObject.GetComponent<CanvasGroup>().interactable = false;
            targetObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        public void LoadNext(int alpha, AnimationType type, Color c, float speed = 1, bool loadNewScene = false, string targetImage = "Null")
        {
            GetAllArguments(type);
            if(targetImage != "Null")
            {
                LoadNewSprite(targetImage, false);
            }
            else if(c.a != 0)
            {
                targetObject.GetComponent<Image>().sprite = null;
                targetObject.GetComponent<Image>().color = c;
            }
            else
            {
                targetObject.GetComponent<Image>().sprite = null;
                targetObject.GetComponent<Image>().color = new(0, 0, 0, 255);
            }
            StartCoroutine(LoadNextScene(alpha, speed, type, loadNewScene));
        }
        private IEnumerator LoadNextScene(int alpha, float speed, AnimationType type, bool loadNewScene)
        {
            ReloadManagers();
            GameObject.Find("VisualManager").GetComponent<UserOperatingManager>().enabled = false;
            animator.speed = speed;
            animator.SetTrigger("Start");
            dialogueShowingManager.SetWhenTransmationStart();
            yield return new WaitForSeconds(1 / speed);
            if (loadNewScene)
            {
                sceneShowingManager.RemoveAllCharacters(true);
            }
            sceneShowingManager.RemoveAllCharacters(false);
            if (loadNewScene)
            {
                sceneShowingManager.TurnIntoScene(alpha);
            }
            yield return null;
            AbnormalStatus status = dialogueShowingManager.NextDialogue();
            if (status != AbnormalStatus.Regular && status != AbnormalStatus.Null)
            {
                resoucesManager.ShowErrorMessage(status);
            }
            yield return new WaitForSeconds(0.99f / speed);
            animator.Play($"{type}_Start");
            animator.speed = 0;
            targetObject.GetComponent<Animator>().enabled = false;
            StopAnimationPlay();
            ReloadManagers();
            GameObject.Find("VisualManager").GetComponent<UserOperatingManager>().enabled = true;
            yield break;
        }
        public void ChangeBackground(string target, AnimationType type, GameObject background, float speed = 1, int r = 0, int g = 0, int b = 0, int a = 0)
        {
            GetAllArguments(type);
            if(target != "Null")
            {
                LoadNewSprite(target);
            }
            else
            {
                ReloadManagers();
                targetObject.GetComponent<Image>().sprite = null;
                targetObject.GetComponent<Image>().color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
                dialogueShowingManager.backGroundColor = targetObject.GetComponent<Image>().color;
                dialogueShowingManager.backGroundImage = null;
            }
            StartCoroutine(StartChangeImage(type, speed, background));
        }
        private IEnumerator StartChangeImage(AnimationType type, float speed, GameObject background)
        {
            if(speed != 114514)
            {
                if(targetObject.GetComponent<Image>().sprite != null)
                {
                    targetObject.GetComponent<Image>().color = new(255, 255, 255, 255);
                }
                animator.speed = speed;
                animator.Play($"{type}_Start");
                dialogueShowingManager.SetWhenTransmationStart();
                yield return new WaitForSeconds(1f / speed);
                background.GetComponent<Image>().sprite = targetObject.GetComponent<Image>().sprite;
                if (targetObject.GetComponent<Image>().sprite == null)
                {
                    background.GetComponent<Image>().color = targetObject.GetComponent<Image>().color;
                    targetObject.GetComponent<Image>().color = new(255, 255, 255, 0);
                }
                else
                {
                    targetObject.GetComponent<Image>().color = new(255, 255, 255, 0);
                }
                animator.Play($"{type}_ChangeBackgroundEnd");
                yield return new WaitForSeconds(0.05f / speed);
                animator.Play($"{type}_Start");
                animator.speed = 0;
                StopAnimationPlay();
                yield break;
            }
            background.GetComponent<Image>().sprite = targetObject.GetComponent<Image>().sprite;
            dialogueShowingManager.backGroundImage = targetObject.GetComponent<Image>().sprite;
            if(targetObject.GetComponent<Image>().color.a != 0)
            {
                dialogueShowingManager.backGroundColor = targetObject.GetComponent<Image>().color;
                background.GetComponent<Image>().color = targetObject.GetComponent<Image>().color;
            }
            targetObject.GetComponent<Animator>().enabled = false;
        }
        private void LoadNewSprite(string s, bool a = true)
        {
            ReloadManagers();
            targetObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Picture/Background/{s}");
            if (a)
            {
                dialogueShowingManager.backGroundImage = targetObject.GetComponent<Image>().sprite;
            }
        }
    }
}