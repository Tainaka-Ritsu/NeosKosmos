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
    public class StartTransformation : Actions // 会清理场上当前所有的角色
    {
        public bool containImage;
        public bool isImage;
        public float transformSpeed;
        public string transformationId;
        public string transformationImage = "Null";
        public Color c32;
        public override IEnumerator Trig()
        {
            SetArgument();
            GameObject.Find("Transition").GetComponent<SceneLoader>().LoadNext(-1, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), !isImage ? c32 : new(0, 0, 0, 0), transformSpeed, targetImage: transformationImage);
            SetStatusFinish();
            yield break;
        }
        public override AbnormalStatus QuickEnd()
        {
            SetArgument();
            GameObject.Find("Transition").GetComponent<SceneLoader>().LoadNext(-1, (AnimationType)Enum.Parse(typeof(AnimationType), transformationId), !isImage ? c32 : new(0, 0, 0, 0), 3, targetImage : transformationImage);
            SetStatusFinish();
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            containImage = Convert.ToInt32(parent.GetNewArgument()) > 0;
            transformSpeed = (float)Convert.ToDouble(parent.GetNewArgument());
            transformationId = parent.GetNewArgument().Replace("\n", "").Replace("\r", "");
            if (containImage)
            {
                isImage = Convert.ToInt32(parent.GetNewArgument()) > 0;
                if (isImage)
                {
                    transformationImage = parent.GetNewArgument();
                }
                else
                {
                    int transformR = Convert.ToInt32(parent.GetNewArgument());
                    int transformG = Convert.ToInt32(parent.GetNewArgument());
                    int transformB = Convert.ToInt32(parent.GetNewArgument());
                    int transformA = Convert.ToInt32(parent.GetNewArgument());
                    c32 = new(transformR / 255f, transformG / 255f, transformB / 255f, transformA / 255f);
                }
            }
            return AbnormalStatus.Running;
        }
    }
}