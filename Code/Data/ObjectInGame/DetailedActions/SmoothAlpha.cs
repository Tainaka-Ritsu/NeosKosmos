using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class SmoothAlpha : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        private Character targetCharacter = null;
        private int endAlpha;
        private int startAlpha;
        private float lastTime = -1f;
        public override IEnumerator Trig()
        {
            SetArgument();
            Color32 c = targetCharacter.parent.GetComponent<Image>().color;
            float perSecond = (endAlpha - startAlpha) / lastTime;
            float endTime = Time.time + lastTime;
            while(Time.time <= endTime)
            {
                targetCharacter.parent.GetComponent<Image>().color = new Color32(c.r, c.g, c.b, (byte)Mathf.Clamp(Time.deltaTime * perSecond + c.a, 0, 255));
            }
            yield return null;
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            SetArgument();
            Color32 c = targetCharacter.parent.GetComponent<Image>().color;
            targetCharacter.parent.GetComponent<Image>().color = new Color32(c.r, c.g, c.b, (byte)Mathf.Clamp(endAlpha, 0, 255));
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetCharacter = displayManager.GetCharacter(Convert.ToInt32(parent.GetNewArgument()));
            if (targetCharacter == null)
            {
                return AbnormalStatus.Operate_Target_Not_Exsist;
            }
            startAlpha = (int)targetCharacter.parent.GetComponent<Image>().color.a * 255;
            endAlpha = Convert.ToInt32(parent.GetNewArgument());
            lastTime = (float)Convert.ToDouble(parent.GetNewArgument());
            return AbnormalStatus.Running;
        }
    }
}