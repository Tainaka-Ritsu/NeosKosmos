using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class CreateCharacter : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        public Character targetCharacter;
        private int x;
        private int y;
        public override IEnumerator Trig()
        {
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                if (targetCharacter.id == 0)
                {
                    displayManager.AddBackground();
                    yield break;
                }
                displayManager.AddCharacter(targetCharacter, x, y);
                SetStatusFinish();
                yield break;
            }
            yield break;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetCharacter = resourcesManager.NewCharacter(Convert.ToInt32(parent.GetNewArgument()));
            if(targetCharacter == null)
            {
                return AbnormalStatus.Init_Target_Not_Found;
            }
            x = Convert.ToInt32(parent.GetNewArgument());
            y = Convert.ToInt32(parent.GetNewArgument());
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus QuickEnd()
        {
            if (displayManager != null && SetArgument() == AbnormalStatus.Regular)
            {
                if (targetCharacter.id == 0)
                {
                    displayManager.AddBackground();
                    return AbnormalStatus.Regular;
                }
                displayManager.AddCharacter(targetCharacter, x, y);
                SetStatusFinish();
            }
            return AbnormalStatus.Regular;
        }
    }
}