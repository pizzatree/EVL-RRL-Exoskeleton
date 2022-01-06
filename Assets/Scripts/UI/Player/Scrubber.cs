// Uses a slider and an input field to scrub the loaded motion
// Depends on the exoskeleton's ControlDataHolder for frame counts and DataPlayer for issuing frame commands

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Player
{
    public class Scrubber : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]                                      private UnityEvent        OnClicked;
        [FormerlySerializedAs("dataHolder")] [SerializeField] private ControlDataHolder controlDataHolder;
        [SerializeField]                                      private DataPlayer        dataPlayer;
        
        private Slider         slider;
        private TMP_InputField inputField;

        private void Awake()
        {
            slider     = GetComponent<Slider>();
            inputField = GetComponentInChildren<TMP_InputField>();

            slider.minValue = slider.maxValue = 1;
        }

        private void OnEnable()
        {
            controlDataHolder.OnSuccessfulLoad += HandleOnSuccessfulLoad;
            dataPlayer.OnUpdatedFrame   += UpdateTime;
        }

        private void OnDisable()
        {
            controlDataHolder.OnSuccessfulLoad -= HandleOnSuccessfulLoad;
            dataPlayer.OnUpdatedFrame   -= UpdateTime;
        }

        private void HandleOnSuccessfulLoad(int maxValue) 
            => slider.maxValue = maxValue;

        public void UpdateTime(int frame)
        {
            if(!slider) 
                return;
            
            slider.value = frame;
            UpdateInputField();
        }

        public void UpdateInputField()
            => inputField.text = slider.value.ToString("0");

        public void OnPointerDown(PointerEventData eventData)
            => OnClicked?.Invoke();
    }
}