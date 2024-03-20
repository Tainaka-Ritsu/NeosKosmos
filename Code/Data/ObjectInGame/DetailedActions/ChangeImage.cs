using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class ChangeImage : Actions
    {
        SceneShowingManager sceneShowingManager;
        private Character targetCharacter;
        private int targetImageIndex;
        public override IEnumerator Trig()
        {
            AbnormalStatus status = SetArgument();
            if(status != AbnormalStatus.Regular && status != AbnormalStatus.Null)
            {
                yield break;
            }
            status = targetCharacter.parent.GetComponent<CharacterDisplay>().SetImage(targetCharacter.GetCharacterImage(targetCharacter.nowImageId = targetImageIndex));
            SetStatusFinish();
            if(status == AbnormalStatus.Regular)
            {
                yield break;
            }
            yield return null;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            sceneShowingManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
            targetCharacter = sceneShowingManager.GetCharacter(Convert.ToInt32(parent.GetNewArgument()));
            if(targetCharacter == null)
            {
                return AbnormalStatus.Operate_Target_Not_Exsist;
            }
            targetImageIndex = Convert.ToInt32(parent.GetNewArgument());
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus QuickEnd()
        {
            AbnormalStatus status = SetArgument();
            if (status != AbnormalStatus.Regular && status != AbnormalStatus.Null)
            {
                return status;
            }
            status = targetCharacter.parent.GetComponent<CharacterDisplay>().SetImage(targetCharacter.GetCharacterImage(targetCharacter.nowImageId = targetImageIndex));
            SetStatusFinish();
            return status;
        }
    }
}