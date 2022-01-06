using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InputIP : MonoBehaviour
    {
        public static event Action<string> OnNewIPEntered;
        
        [SerializeField] private Image inputBackground;
        [SerializeField] private Color goodInputColor = new Color(0f, 1f, 0f, 0.5f);
        [SerializeField] private Color badInputColor  = new Color(1f, 0f, 0f, 0.5f);

        private void Start() 
            => LiveDataReader.OnConnectionEvent += UpdateStatus;

        private void UpdateStatus(bool success, string message) 
            => inputBackground.color = success ? goodInputColor : badInputColor;

        public void UpdateIP(string newIP) 
            => OnNewIPEntered?.Invoke(newIP);
    }
}
