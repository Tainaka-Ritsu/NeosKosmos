using KinglandStudio.NeosKosmos.Controller;
using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class ChangeBackground : Actions
    {
        GameObject targetBackground;
        GameObject backgroundMask;
        public float transformSpeed;
        public string loadBackgroundId;
        public string transformationId;
        public int backgroundId;
        public int transformR;
        public int transformG;
        public int transformB;
        public int transformA;
        public override IEnumerator Trig()
        {
            SetArgument();
            if (loadBackgroundId != "Null")
            {
                backgroundMask.GetComponent<SceneLoader>().ChangeBackground(loadBackgroundId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), targetBackground, transformSpeed);
            }
            else
            {
                backgroundMask.GetComponent<SceneLoader>().ChangeBackground(loadBackgroundId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), targetBackground, transformSpeed, transformR, transformG, transformB, transformA);
            }
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            if(loadBackgroundId == null)
            {
                SetArgument();
            }
            backgroundMask.GetComponent<SceneLoader>().StopAnimationPlay();
            if (loadBackgroundId != "Null")
            {
                backgroundMask.GetComponent<SceneLoader>().ChangeBackground(loadBackgroundId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), targetBackground, 114514);
            }
            else
            {
                backgroundMask.GetComponent<SceneLoader>().ChangeBackground(loadBackgroundId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), targetBackground, 114514, transformR, transformG, transformB, transformA);
            }
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            backgroundMask = GameObject.Find("BackGroundTransition");
            backgroundId = Convert.ToInt32(parent.GetNewArgument());
            targetBackground = GameObject.Find(backgroundId.ToString());
            transformSpeed = (float)Convert.ToDouble(parent.GetNewArgument());
            loadBackgroundId = parent.GetNewArgument().Replace("\n", "").Replace("\r", "");
            transformationId = parent.GetNewArgument().Replace("\n", "").Replace("\r", "");
            if(loadBackgroundId == "Null")
            {
                transformR = Convert.ToInt32(parent.GetNewArgument());
                transformG = Convert.ToInt32(parent.GetNewArgument());
                transformB = Convert.ToInt32(parent.GetNewArgument());
                transformA = Convert.ToInt32(parent.GetNewArgument());
            }
            return AbnormalStatus.Running;
        }
    }
}