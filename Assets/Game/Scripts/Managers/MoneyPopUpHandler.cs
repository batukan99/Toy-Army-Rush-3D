using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace Game.Managers 
{
    public class MoneyPopUpHandler : MonoBehaviour, IProvidable
    {
        [SerializeField] private RectTransform moneyUI;
        private PoolManager _poolManager;

        private void Awake()
        {
            ManagerProvider.Register(this);
            _poolManager = ManagerProvider.GetManager<PoolManager>();
        }

        public void ShowMoneyPopUp(int count, Vector3 origin, UnityAction onComplete = null)
        {
            float randomizePos = 35f;
            float duration = 0.3f;
            float delay = 0.1f;

            for (int i = 0; i < count; i++)
            {
                RectTransform money = _poolManager.GetMoneyPopUpObject(origin, transform.rotation, transform);
                money.position = origin;

                Vector2 randomOffset = new Vector2(Random.Range(-randomizePos, randomizePos), Random.Range(-randomizePos, randomizePos));

                Sequence seq = DOTween.Sequence();

                money.localScale = Vector3.one * 0.75f;
                seq.Append(money.DOAnchorPos(money.anchoredPosition + randomOffset, duration).SetEase(Ease.OutSine));
                seq.Append(money.DOScale(1f, 0.1f).SetEase(Ease.InOutSine));
                seq.Append(money.DOMove(moneyUI.position, duration).SetEase(Ease.OutSine).SetDelay(delay));
                seq.OnComplete(() => {
                    _poolManager.ReturnMoneyPopUpToPool(money.gameObject);
                });
                seq.Play();

                money.gameObject.SetActive(true);
                
                
            }
            if (onComplete != null)
            {
                DelayHandler.WaitAndInvoke(onComplete.Invoke, 2 * duration + 0.1f + delay);
            }
        }
    }
}

