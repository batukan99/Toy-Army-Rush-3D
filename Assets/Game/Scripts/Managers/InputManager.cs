
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Managers 
{
    public class InputManager : MonoBehaviour, IProvidable, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float maxDistance = 100f;

        public Vector2 Input {get; private set;}
        [HideInInspector] public PointerEventData eventData;

        private Vector2 _startPos;
        private Vector2 _delta;
        void Awake()
        {
            ManagerProvider.Register(this);
        }

        void Update()
        {
            if(eventData != null) 
            {
                _delta = eventData.position - _startPos;
                _delta.x = Mathf.Clamp(_delta.x, -maxDistance, maxDistance);
                _delta.y = Mathf.Clamp(_delta.y, -maxDistance, maxDistance);
                Input = _delta / maxDistance;
                _startPos = eventData.position;

            }
            
        }
        public void OnPointerDown (PointerEventData _eventData)
        {
            eventData = _eventData;
            _startPos = eventData.position;
        }
        public void OnPointerUp (PointerEventData _eventData)
        {
            eventData = null;
            _delta = Vector2.zero;
            _startPos = Vector2.zero;
            Input = Vector2.zero;
        }
    }
}

