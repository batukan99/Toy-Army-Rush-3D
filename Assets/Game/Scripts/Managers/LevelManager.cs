using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using Game.Core.Constants;
using Game.Core;
using TMPro;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour, IProvidable
    {
        [SerializeField] private int levelCount;
        [SerializeField] private int playLevel;
        [SerializeField] private TextMeshProUGUI levelText;

        public int ActiveLevelIndex { get; private set; }

        private void Start() {
            levelText.text = $"LEVEL {ActiveLevelIndex}";
        }
        
        public void AddMoney(int amount)
        {
            if (amount > 0)
            {
                DataManager.Instance.SetMoney(DataManager.Instance.Money + amount);
                EventBase.NotifyListeners(EventType.MoneyAdded);
            }
        }
        public void DecreaseMoney(int amount)
        {
            if (amount > 0)
            {
                DataManager.Instance.SetMoney(DataManager.Instance.Money - amount);
                EventBase.NotifyListeners(EventType.MoneyDecreased);
            }
        }
        
        public void CreateLevel()
        {
            ActiveLevelIndex = playLevel == -1 ? DataManager.Instance.Level : playLevel;

            Instantiate(Resources.Load<GameObject>("Levels/Level-" + ActiveLevelIndex));
        }

        private void LevelUp()
        {
            int level = DataManager.Instance.Level + 1;

            if (level > levelCount)
            {
                level = 1;
            }

            DataManager.Instance.SetLevel(level);
        }
        public void LoadScene()
        {
            ManagerProvider.ResetProvider();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnGameOverEvent(bool status)
        {
            if (status)
            {
                int collectedAmount = GameSettings.CollectableMoneyValue * Multiplier.lastAchievedMultiplier.collectedCountOnFinish;
                int incomeBonus = DataManager.Instance.IncomeUpgrade * GameSettings.IncomeUpgradeValue;
                int endMoney = (collectedAmount + incomeBonus) * Multiplier.lastAchievedMultiplier.Value;
                print(endMoney);

                UIManager uiManager = ManagerProvider.GetManager<UIManager>();
                uiManager.endPanel.SetEndMoneyText(endMoney);

                Vector3 popUpTarget = uiManager.endPanel.endMoneyText.transform.position;
                ManagerProvider.GetManager<MoneyPopUpHandler>()
                    .ShowMoneyPopUp(10, popUpTarget, () => AddMoney(endMoney));

                LevelUp();
            }
        }


        private void OnEnable()
        {
            ManagerProvider.Register(this);
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
        }
        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
        }
        

    }
}
