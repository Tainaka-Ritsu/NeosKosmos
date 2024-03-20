using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class DestroyCharacter : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        public Character targetCharacter;
        public override IEnumerator Trig()
        {
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                displayManager.RemoveCharacter(targetCharacter);
                SetStatusFinish();
                yield break;
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
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                displayManager.RemoveCharacter(targetCharacter);
                SetStatusFinish();
            }
            return AbnormalStatus.Regular;
        }
    }
}