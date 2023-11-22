using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UILogger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private float logDelay;
        public async Task LogMessage(string message)
        {
            string currentMessage = "";
            float timer = 0;
            int it = 0;
            while (it != message.Length)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = logDelay;
                    currentMessage += message[it];
                    it++;
                    logText.text = currentMessage;
                }
                await Task.Yield();
            }
        }
    }
}