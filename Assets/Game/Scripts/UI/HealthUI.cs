using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetHealth(float health)
        {
            if (health > 0)
            {
                text.text = health.ToString();
            }
        }
    }
}
