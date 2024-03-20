using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class SmoothRotate : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        private Position2 targetPosition = new(0, 0);
        private Position2 start;
        private Character targetCharacter = null;
        private float lastTime = -1f;
        public override IEnumerator Trig()
        {
            SetArgument();
            if (lastTime == 0)
            {
                QuickEnd();
                yield break;
            }
            start = new(targetCharacter.position.x, targetCharacter.position.y);
            Debug.Log(targetCharacter.position.x + " " + targetCharacter.position.y);
            Position2 velocity = targetPosition / lastTime;
            targetCharacter.parent.GetComponent<CharacterDisplay>().velocity = velocity;
            yield return new WaitForSeconds(lastTime);
            targetCharacter.parent.GetComponent<CharacterDisplay>().StopMovingByVelocity();
            targetCharacter.parent.GetComponent<CharacterDisplay>().TransformTo(targetPosition + start);
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            if (targetPosition == null || targetCharacter == null || lastTime == -1)
            {
                SetArgument();
            }
            start = new(targetCharacter.position.x, targetCharacter.position.y);
            targetCharacter.parent.GetComponent<CharacterDisplay>().StopMovingByVelocity();
            targetCharacter.parent.GetComponent<CharacterDisplay>().TransformTo(targetPosition + start);
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetPosition.x = (float)Convert.ToDouble(parent.GetNewArgument());
            targetPosition.y = (float)Convert.ToDouble(parent.GetNewArgument());
            string tar = parent.GetNewArgument();
            if (tar != "BG")
            {
                targetCharacter = displayManager.GetCharacter(Convert.ToInt32(tar));
            }
            else if (tar == "BG")
            {
                targetCharacter = GameObject.Find("BackGround").GetComponent<CharacterDisplay>().character;
            }
            if (targetCharacter == null)
            {
                return AbnormalStatus.Operate_Target_Not_Exsist;
            }
            lastTime = (float)Convert.ToDouble(parent.GetNewArgument());
            return AbnormalStatus.Running;
        }
    }
}