
using UnityEngine;
//using Game.UI.Panels;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using Game.UI.Panels;
using UnityEngine.Events;

namespace Game.Managers
{
    public class UIManager : MonoBehaviour, IProvidable
    {
        public Canvas WorldSpaceCanvas;
        public MainPanel mainPanel;
        public GamePanel gamePanel;
        public EndPanel endPanel;
        public CommonPanel commonPanel;


        void Start()
        {
            mainPanel.SetActiveImmediately(true);
            gamePanel.SetActiveImmediately(false);
            //endPanel.SetActiveImmediately(false);
            commonPanel.SetActiveImmediately(true);
        }

        private void OnGameStartedEvent()
        {
            mainPanel.SetActiveSmooth(false);
            gamePanel.SetActiveSmooth(true);
            //ManagerProvider.GetManager<GameManager>().EndGame(true);
        }
        private void OnGameOverEvent(bool won)
        {
            gamePanel.SetActiveSmooth(false);
            endPanel.SetActiveSmooth(true);

            if (won)
            {
                endPanel.EnableSuccessScreen();
            }
            else
            {
                commonPanel.SetActiveSmooth(false);
                endPanel.EnableFailScreen();
            }
        }
        private void OnEnable()
        {
            ManagerProvider.Register(this);
            EventBase.StartListening(EventType.GameStarted, OnGameStartedEvent);
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameStarted, OnGameStartedEvent);
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
        }
    }
}
