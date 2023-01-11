using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

   public class UISignals : MonoSingleton<UISignals>
    {
        /*public UnityAction<UIPanels> onOpenPanel;
        public UnityAction<UIPanels> onClosePanel;
        public UnityAction<int> onUpdateStageData;
        public UnityAction<int> onSetLevelText;
        */
        public UnityAction onUpdateScore;
    }
