using KinglandStudio.NeosKosmos.Data.BasicDataType;
using KinglandStudio.NeosKosmos.Data.BasicValue;
using KinglandStudio.NeosKosmos.Data.Manager;
using KinglandStudio.NeosKosmos.UI;
using System;
using System.Collections;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.DetailedAction
{
    public class JumpDialogue : Actions
    {
        SceneShowingManager displayManager = GameObject.Find("VisualManager").GetComponent<SceneShowingManager>();
        private int targetId;
        public override IEnumerator Trig()
        {
            SetArgument();
            displayManager.nowDialogue = displayManager.nowScene.JumpDialogue(targetId, parent);
            displayManager.nowScene.playDialogueId = displayManager.nowDialogue.precursor.inSceneId;
            dialogueShowingManager.NextDialogue(false);
            yield break;
        }
        public override AbnormalStatus SetArgument()
        {
            FindOurManager();
            targetId = Convert.ToInt32(parent.GetNewArgument());
            return AbnormalStatus.Regular;
        }
        public override AbnormalStatus QuickEnd()
        {
            SetArgument();
            displayManager.nowDialogue = displayManager.nowScene.JumpDialogue(targetId, parent.precursor).precursor;
            dialogueShowingManager.NextDialogue(false);
            return AbnormalStatus.Regular;
        }
    }
}