using UnityEngine;


    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static bool _quiting = false;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null && !_quiting) //if instance null and not closing the game, create a new instance
                    {
                        GameObject newGo = new GameObject();
                        _instance = newGo.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        private void OnDestroy() 
        {
            _quiting = true;
        }
    }
