using KinglandStudio.NeosKosmos.Controller;
using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class ChangeScene : Actions
    {
        public int targetSceneId;
        public float transformSpeed;
        public string transformationId;
        public override IEnumerator Trig()
        {
            SetArgument();
            GameObject.Find("Transition").GetComponent<SceneLoader>().LoadNext(targetSceneId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), new(0, 0, 0, 0), transformSpeed, true);
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            SetArgument();
            GameObject.Find("Transition").GetComponent<SceneLoader>().LoadNext(targetSceneId, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), new(0, 0, 0, 0), transformSpeed, true);
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetSceneId = Convert.ToInt32(parent.GetNewArgument());
            transformSpeed = (float)Convert.ToDouble(parent.GetNewArgument());
            transformationId = parent.GetNewArgument();
            return AbnormalStatus.Running;
        }
    }
}