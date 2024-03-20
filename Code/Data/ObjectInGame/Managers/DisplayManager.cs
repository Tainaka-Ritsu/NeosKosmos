using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class DisplayManager : MonoBehaviour
    {
        public ResoucesManager resoucesManager;
        public GameObject characterPrefab;
        public GameObject backgroundPrefab;
        public GameObject buttonPrefab;
        public GameObject buttonBackground;
        public SceneShowingManager showing;
        public Sprite showingBackGround;
        [SerializeField]
        private List<Character> differenceSet;
        public List<Character> showingCharacters;
        public void Refresh()
        {
            differenceSet = showing.characters.Except(showingCharacters, new CharacterComparer()).ToList();
            AbnormalStatus status = Rendering(differenceSet);
            if (status != AbnormalStatus.Regular && status != AbnormalStatus.Null)
            {
                resoucesManager.ShowErrorMessage(status);
            }
            ResetShowingCharacter();
        }
        private AbnormalStatus Rendering(List<Character> characters)
        {
            foreach(var chara in characters)
            {
                if(chara.idInScene == 0)
                {
                    continue;
                }
                AbnormalStatus returnValue = AddCharactersToScreen(chara, chara.isBG);
                if (returnValue != AbnormalStatus.Regular && returnValue != AbnormalStatus.Null)
                {
                    return returnValue;
                }
            }
            return AbnormalStatus.Regular;
        }
        public AbnormalStatus AddCharactersToScreen(Character a, bool isBG)
        {
            GameObject newChara = GameObject.Instantiate<GameObject>(isBG ? backgroundPrefab : characterPrefab, GameObject.Find("Canvas").transform, true);
            newChara.transform.localPosition = new(a.position.x, a.position.y, a.layer);
            newChara.transform.localScale = Vector3.one;
            newChara.transform.localScale *= a.zoomCoefficient;
            newChara.name = a.idInScene.ToString();
            a.parent = newChara;
            newChara.GetComponent<CharacterDisplay>().character = a;
            return newChara.GetComponent<CharacterDisplay>().SetImage(a.GetCharacterImage(a.nowImageId));
        }
        public AbnormalStatus RemoveCharacter(Character chara)
        {
            for (int i = 0; i < showingCharacters.Count; i++)
            {
                if (chara.Equals(showingCharacters[i]))
                {
                    GameObject target = GameObject.Find(chara.idInScene.ToString());
                    if (target != null)
                    {
                        Destroy(target);
                        return AbnormalStatus.Regular;
                    }
                    break;
                }
            }
            return AbnormalStatus.Destroy_Target_Not_Found;
        }
        public AbnormalStatus SetButtons(List<Dialogue> dialogues)
        {
            int i = 0;
            buttonBackground = GameObject.Find("ButtonBackground");
            foreach (Dialogue dialogue in dialogues)
            {
                GameObject button = Instantiate(buttonPrefab, buttonBackground.transform);
                button.GetComponent<IngameButton>().dialogue = dialogue;
                button.GetComponent<IngameButton>().Set();
                button.name = "Button" + i.ToString();
                i++;
            }
            return AbnormalStatus.Regular;
        }
        public void RemoveButtons()
        {
            for (int i = 0; i < buttonBackground.transform.childCount; i++)
            {
                Destroy(buttonBackground.transform.GetChild(i).gameObject);
            }
        }
        private void ResetShowingCharacter()
        {
            showingCharacters.Clear();
            foreach (var item in showing.characters)
            {
                showingCharacters.Add(new(item));
            }
        }
    }
}