using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.UI;
using System.Collections.Generic;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class SceneShowingManager : MonoBehaviour
    {
        public ResoucesManager resoucesManager;
        public DisplayManager displayManager;
        public Scenes nowScene = Scenes.empty;
        public Dialogue nowDialogue = null;
        public List<Character> characters = new();
        public List<bool> active = new();
        public int used_id = -1;
        public AbnormalStatus ResetUsingId()
        {
            used_id = -1;
            return AbnormalStatus.Regular;
        }
        public AbnormalStatus Next()
        {
            if (nowScene == null || nowScene.Equals(Scenes.empty))
            {
                return AbnormalStatus.Scene_Not_Found;
            }
            nowDialogue = nowScene.NextDialogue();
            if(nowDialogue == null || nowDialogue == Dialogue.empty)
            {
                return AbnormalStatus.Next_Dialogue_Not_Found;
            }
            return AbnormalStatus.Regular;
        }
        public AbnormalStatus AddCharacter(Character target, float x, float y)
        {
            target.position.x = x;
            target.position.y = y;
            used_id++;
            target.idInScene = used_id;
            characters.Add(new(target));
            active.Add(true);
            displayManager.Refresh();
            return AbnormalStatus.Regular;
        }
        public AbnormalStatus AddBackground()
        {
            Character newOne = new(resoucesManager.allCharacters[0]);
            newOne.isBG = true;
            return AddCharacter(newOne, 0, 0);
        }
        public void CompareCharacterToGameobject()
        {
            if (characters.Count == 0)
            {
                return;
            }
            GameObject chara;
            for (int i = 0; i < characters.Count; i++)
            {
                if (!active[i])
                {
                    continue;
                }
                if(GameObject.Find(i.ToString()) == null)
                {
                    displayManager.AddCharactersToScreen(characters[i], characters[i].isBG);
                    chara = GameObject.Find(i.ToString());
                    if (chara == null)
                    {
                        continue;
                    }
                }
                chara = GameObject.Find(i.ToString());
                chara.GetComponent<CharacterDisplay>().character.parent = chara;
                chara.GetComponent<CharacterDisplay>().TransformTo(chara.GetComponent<CharacterDisplay>().character.position);
                chara.GetComponent<CharacterDisplay>().Zoom(chara.GetComponent<CharacterDisplay>().character.zoomCoefficient);
            }
        }
        public AbnormalStatus RemoveAllCharacters(bool destroyBG)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if(!destroyBG && characters[i].isBG)
                {
                    continue;
                }
                Character target = characters[i];
                characters.Remove(target);
                displayManager.RemoveCharacter(target);
            }
            return AbnormalStatus.Regular;
        }
        public AbnormalStatus RemoveCharacter(Character target)
        {
            for(int i = 0; i < characters.Count; i++)
            {
                if (target.Equals(characters[i]))
                {
                    active[i] = false;
                    AbnormalStatus status = displayManager.RemoveCharacter(target);
                    if(status == AbnormalStatus.Regular)
                    {
                        return status;
                    }
                    return status;
                }
            }
            return AbnormalStatus.Destroy_Target_Not_Found;
        }
        public Character GetCharacter(int idInScene)
        {
            foreach(Character character in characters)
            {
                if(character.idInScene == idInScene)
                {
                    if(character.parent == null)
                    {
                        character.parent = GameObject.Find(character.idInScene.ToString());
                    }
                    return character;
                }
            }
            return null;
        }
        public void OnNewSceneStart()
        {
            Debug.Log(characters.Count);
            if(characters.Count == 0)
            {
                Character chara = new(resoucesManager.NewCharacter(0));
                chara.isBG = true;
                displayManager.AddCharactersToScreen(chara, true);
                chara.parent = GameObject.Find("0");
                AddCharacter(chara, 0, 0);
            }
            else
            {
                used_id = characters.Count - 1;
            }
        }
        public void TurnIntoScene(int alpha)
        {
            resoucesManager.GetNewScene(alpha, nowDialogue);
            if (nowScene == Scenes.empty)
            {
                resoucesManager.ShowErrorMessage(AbnormalStatus.Scene_Not_Found);
            }
        }
        public void Init()
        {
            TurnIntoScene(0);
            GetComponent<DialogueShowingManager>().NextDialogue();
        }
    }
}