using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.Core.Events;
using Game.Managers;
using EventType = Game.Core.Enums.EventType;
using DG.Tweening;
using Unity.VisualScripting;

namespace Game.Core
{
    public class Multiplier : MonoBehaviour
    {
        [SerializeField, Min(2)] private int multiplier = 2;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private MeshRenderer meshRenderer;

        public static Multiplier lastAchievedMultiplier;
        public int collectedCountOnFinish = 0;
        public int Value => multiplier;
        
        private UIManager _uiManager;
        private void Awake()
        {
            _uiManager = ManagerProvider.GetManager<UIManager>();
            text.transform.SetParent(_uiManager.WorldSpaceCanvas.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<Collider>().enabled = false;
                if(lastAchievedMultiplier != null)
                    collectedCountOnFinish += lastAchievedMultiplier.collectedCountOnFinish + 1;
                else
                    collectedCountOnFinish = 1;
                lastAchievedMultiplier = this;
            }
        }

        private void OnGameOverEvent(bool status)
        {
            if (status && lastAchievedMultiplier == this)
            {
                Color color = meshRenderer.material.color;
                meshRenderer.material.DOColor(Color.white, 0.25f)
                    .SetEase(Ease.OutQuart)
                    .OnComplete(() => {
                        meshRenderer.material.DOColor(color, 2f).SetEase(Ease.OutSine).SetDelay(2f);
                    });
            }
        }

        private void OnValidate()
        {
            if (text != null)
            {
                text.text = "X" + multiplier;
            }
        }

        private void OnEnable()
        {
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
        }
    }
}
