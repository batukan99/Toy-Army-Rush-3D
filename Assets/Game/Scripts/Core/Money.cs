using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Managers;
using Game.Core.Constants;

namespace Game.Core 
{
    public class Money : MonoBehaviour
    {
        private int value = 20;
        private Collider _collider;
        private MoneyPopUpHandler _moneyPopUpHandler;
        private LevelManager _levelManager;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _moneyPopUpHandler = ManagerProvider.GetManager<MoneyPopUpHandler>();
            _levelManager = ManagerProvider.GetManager<LevelManager>();
            value = GameSettings.CollectableMoneyValue;
        }

        private void Collect()
        {
            _collider.enabled = false;
            
            transform.DOScale(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));

            Vector3 moneyPos = GameManager.Instance.mainCamera.WorldToScreenPoint(transform.position);
            _moneyPopUpHandler.ShowMoneyPopUp(1, moneyPos, () => _levelManager.AddMoney(value));
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }
    }
}
    
