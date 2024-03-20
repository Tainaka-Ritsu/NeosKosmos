using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class Noise : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        public Character targetCharacter;
        public int xMax, yMax, xMin, yMin;
        public float lastTime;
        public override IEnumerator Trig()
        {
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                CharacterDisplay characterDisplay = GameObject.Find(targetCharacter.idInScene.ToString()).GetComponent<CharacterDisplay>();
                if (characterDisplay != null)
                {
                    characterDisplay.SetNoise(lastTime, xMax, yMax, xMin, yMin);
                    yield return new WaitForSeconds(lastTime);
                    if(lastTime != -1)
                    {
                        characterDisplay.StopNoise();
                    }
                }
            }
            yield break;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetCharacter = resourcesManager.NewCharacter(Convert.ToInt32(parent.GetNewArgument()));
            xMax = Convert.ToInt32(parent.GetNewArgument());
            yMax = Convert.ToInt32(parent.GetNewArgument());
            xMin = Convert.ToInt32(parent.GetNewArgument());
            yMin = Convert.ToInt32(parent.GetNewArgument());
            lastTime = Convert.ToInt32(parent.GetNewArgument());
            if (targetCharacter == null)
            {
                return AbnormalStatus.Init_Target_Not_Found;
            }
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus QuickEnd()
        {
            if (displayManager != null)
            {
                SetArgument();
                if (lastTime == -1)
                {
                    CharacterDisplay characterDisplay = GameObject.Find(targetCharacter.idInScene.ToString()).GetComponent<CharacterDisplay>();
                    characterDisplay.SetNoise(lastTime, xMax, yMax, xMin, yMin);
                }
            }
            return AbnormalStatus.Regular;
        }
    }
}