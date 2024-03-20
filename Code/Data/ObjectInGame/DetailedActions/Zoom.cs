using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class Zoom : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        private float zoom = -1f;
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
            float app = (zoom - targetCharacter.zoomCoefficient) / lastTime;
            float last = Time.time + lastTime;
            while(Time.time <= last)
            {
                targetCharacter.parent.GetComponent<CharacterDisplay>().Zoom(targetCharacter.zoomCoefficient + (app * Time.deltaTime));
                yield return null;
            }
            targetCharacter.parent.GetComponent<CharacterDisplay>().Zoom(zoom);
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            if (zoom == -1 || targetCharacter == null || lastTime == -1)
            {
                SetArgument();
            }
            targetCharacter.parent.GetComponent<CharacterDisplay>().Zoom(zoom);
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            zoom = (float)Convert.ToDouble(parent.GetNewArgument());
            string tar = parent.GetNewArgument();
            targetCharacter = displayManager.GetCharacter(Convert.ToInt32(tar));
            if (targetCharacter == null)
            {
                return AbnormalStatus.Operate_Target_Not_Exsist;
            }
            lastTime = (float)Convert.ToDouble(parent.GetNewArgument());
            return AbnormalStatus.Running;
        }
    }
}