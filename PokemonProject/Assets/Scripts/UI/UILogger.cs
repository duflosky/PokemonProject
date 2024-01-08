using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UILogger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private float logDelay;
        public bool isLogging { get; private set; }
        public async Task LogMessage(string message)
        {
            isLogging = true;
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
            isLogging = false;
        }
    }
}