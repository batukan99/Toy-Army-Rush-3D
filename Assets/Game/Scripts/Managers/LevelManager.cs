using System.Collections;
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

        private ADManager _adManager;
        private UIManager _uiManager;
        private int _endMoney;
        
        private void Start() {
            levelText.text = $"LEVEL {ActiveLevelIndex}";
            _adManager = ManagerProvider.GetManager<ADManager>();
            _uiManager = ManagerProvider.GetManager<UIManager>();
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
        private IEnumerator LoadSceneDelayed(float delay) 
        {
            yield return new WaitForSeconds(delay);
            LoadScene();
        }
        public void ClaimMoney()
        {
            Vector3 popUpTarget = _uiManager.endPanel.endMoneyText.transform.position;
            ManagerProvider.GetManager<MoneyPopUpHandler>()
                    .ShowMoneyPopUp(10, popUpTarget, () => {
                        AddMoney(_endMoney);
                        StartCoroutine(LoadSceneDelayed(0.4f));
                    });
        }
        public void ShowInterstitialAd()
        {
            _adManager.ShowInterstitialAd();
        }
        public void ShowRewardedAd()
        {
            _adManager.ShowRewardedAd();
        }
        private void OnRewardedAd(bool hasRewardEarned) 
        {
            if(hasRewardEarned)
                _endMoney *= 3;
            
            _uiManager.endPanel.SetEndMoneyText(_endMoney);
            ClaimMoney();
        }

        private void OnGameOverEvent(bool status)
        {
            if (status)
            {
                int collectedAmount = GameSettings.CollectableMoneyValue * Multiplier.lastAchievedMultiplier.collectedCountOnFinish;
                int incomeBonus = DataManager.Instance.IncomeUpgrade * GameSettings.IncomeUpgradeValue;
                _endMoney = (collectedAmount + incomeBonus) * Multiplier.lastAchievedMultiplier.Value;
                
                _uiManager.endPanel.SetEndMoneyText(_endMoney);

                LevelUp();
            }

            DataManager.Instance.SetInterstitialCounter(DataManager.Instance.InterstitialCounter + 1);
            if(DataManager.Instance.InterstitialCounter >= _adManager.InterstitialAdFrequency)
            {
                ShowInterstitialAd();
                DataManager.Instance.SetInterstitialCounter(0);
            }
        }


        private void OnEnable()
        {
            ManagerProvider.Register(this);
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
            EventBase.StartListening(EventType.AdRewarded, (UnityAction<bool>) OnRewardedAd);
        }
        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
            EventBase.StopListening(EventType.AdRewarded, (UnityAction<bool>) OnRewardedAd);
        }
        

    }
}
