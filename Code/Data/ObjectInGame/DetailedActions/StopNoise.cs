using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class StopNoise : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        public Character targetCharacter;
        public override IEnumerator Trig()
        {
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                CharacterDisplay characterDisplay = GameObject.Find(targetCharacter.idInScene.ToString()).GetComponent<CharacterDisplay>();
                characterDisplay.StopNoise();
            }
            yield break;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetCharacter = resourcesManager.NewCharacter(Convert.ToInt32(parent.GetNewArgument()));
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
                CharacterDisplay characterDisplay = GameObject.Find(targetCharacter.idInScene.ToString()).GetComponent<CharacterDisplay>();
                characterDisplay.StopNoise();
            }
            return AbnormalStatus.Regular;
        }
    }
}