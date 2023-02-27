using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson3
{
    public class TextField : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textObject;
        [SerializeField]
        private Scrollbar scrollbar;
        private List<string> messages = new List<string>();

        private void Start()
        {
            scrollbar.onValueChanged.AddListener((float value) => UpdateText());
        }

        public void ReceiveMessage(object message)
        {
            messages.Add(message.ToString());
            float value = (messages.Count - 1) * scrollbar.value;
            scrollbar.value = Mathf.Clamp(value, 0, 1);
            UpdateText();
        }

        private void UpdateText()
        {
            string text = "";
            int index = (int)(messages.Count * scrollbar.value);
            for (int i = index; i < messages.Count; i++)
            {
                text += messages[i] + "\n";
            }
            textObject.text = text;
        }
    }
}