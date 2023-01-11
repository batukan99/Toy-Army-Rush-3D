using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Game.Managers;
using Game.Core.Constants;
using Game.Core.Events;
using Game.Core.Enums;
using EventType = Game.Core.Enums.EventType;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UpgradeButtonBase : MonoBehaviour
    {
        [SerializeField] protected UpgradeButtonType upgradeButtonType;
        [SerializeField] protected TextMeshProUGUI levelText;
        [SerializeField] protected TextMeshProUGUI priceText;
        [SerializeField] private Image bottomImage;
        [SerializeField] private Color buyableColor = Color.green;
        [SerializeField] private Color unBuyableColor = Color.red;

        protected bool isLocked = false;

        private Vector3 initialScale;
        private CanvasGroup canvasGroup;
        private Tween fadeTween;
        private Tween scaleTween;
        private LevelManager _levelManager;

        protected static UnityEvent freeUpgradeCompletedEvent = new UnityEvent();

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            initialScale = transform.localScale;

            GetComponent<Button>().onClick.AddListener(OnCLick);
            _levelManager = ManagerProvider.GetManager<LevelManager>();
        }
        private void Start()
        {
            // Check if we can buy the upgrade once at the beginning.
            SetLockStatus(!CanBuyUpgrade(DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType), out _));
            UpdateTexts();
        }

        protected bool CanBuyUpgrade(int upgradeLevel, out int cost)
        {
            cost = GameSettings.GetUpgradeCost(upgradeLevel);
            return DataManager.Instance.Money >= cost;
        }

        protected void SetLevelText(string text)
        {
            levelText.text = text;
        }

        protected void SetPriceText(string text)
        {
            priceText.text = text;
        }

        protected void SetLockStatus(bool isLocked)
        {
            // Keep lock information
            this.isLocked = isLocked;

            // Kill fade tween to be able to change canvasGroup alpha immediately.
            fadeTween.Kill();

            // Set alpha value
            canvasGroup.alpha = isLocked ? 0.75f : 1f;

            // Set color for the bottom image according to lock status
            bottomImage.color = isLocked ? unBuyableColor: buyableColor;
        }

        private void ClickAnimation()
        {
            fadeTween.Kill();
            fadeTween = canvasGroup.DOFade(0.75f, 0.1f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() => {
                    canvasGroup.DOFade(1f, 0.1f).SetEase(Ease.OutQuart);
                });

            scaleTween.Kill();
            scaleTween = transform.DOScale(initialScale * 0.95f, 0.1f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() => {
                    transform.DOScale(initialScale, 0.1f).SetEase(Ease.OutQuart);
                });
        }

        public virtual void OnCLick()
        {
            // Check if next upgrade is still affordable and update the lock status.
            SetLockStatus(!CanBuyUpgrade(DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType), out _));

            if (!isLocked)
            {
                ClickAnimation();
            }

            Upgrade();
        }
        private void UpdateTexts()
        {
            SetLevelText(DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType).ToString());
            SetPriceText(Mathf.RoundToInt(GameSettings.GetUpgradeCost(
                DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType))).ToString());
        }
        private void UpgradeIncome()
        {
            int upgradeLevel = DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType);

            if (CanBuyUpgrade(upgradeLevel, out int cost))
            {
                DataManager.Instance.SetUpgradeButtonLevel(upgradeButtonType);
                _levelManager.DecreaseMoney(cost);

                NotifyUpgradeListeners(upgradeButtonType);
            }
        }
        private void NotifyUpgradeListeners(UpgradeButtonType upgradeButtonType)
        {
            switch(upgradeButtonType)
            {
                case UpgradeButtonType.DamageUpgrade:
                EventBase.NotifyListeners(EventType.DamageUpgraded);
                break;
                
                case UpgradeButtonType.IncomeUpgrade:
                EventBase.NotifyListeners(EventType.IncomeUpgraded);
                break;

                case UpgradeButtonType.HealthUpgrade:
                EventBase.NotifyListeners(EventType.HealthUpgraded);
                break;
            }
        }

        #region Virtual Methods

        protected virtual void OnMoneyUpdated()
        {
            SetLockStatus(!CanBuyUpgrade(DataManager.Instance.GetUpgradeButtonLevel(upgradeButtonType), out _));
            UpdateTexts();
        }

        protected virtual void Upgrade()
        {
            UpgradeIncome();
            UpdateTexts();
        }

        #endregion

        private void OnEnable()
        {
            EventBase.StartListening(EventType.MoneyAdded, OnMoneyUpdated);
            EventBase.StartListening(EventType.MoneyDecreased, OnMoneyUpdated);
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.MoneyAdded, OnMoneyUpdated);
            EventBase.StopListening(EventType.MoneyDecreased, OnMoneyUpdated);
        }
    }

}
