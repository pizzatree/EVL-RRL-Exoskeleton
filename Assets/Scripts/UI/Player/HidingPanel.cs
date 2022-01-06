// Fades panel in and out using CanvasGroup alpha when the mouse is or isn't over the UI elements
// Supports a callback when reaching either full alpha or empty alpha

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Player
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HidingPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Range(0.2f, 1f), Tooltip("Higher is faster.")]
        private float transitionSpeed = 0.5f;

        private CanvasGroup canvasGroup;

        private void Start() 
            => canvasGroup = GetComponent<CanvasGroup>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(Transition(1f, null));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(Transition(0f, null));
        }

        private IEnumerator Transition(float newValue, Action callBack)
        {
            var t = 0f;
            while(t < 1f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, newValue, t);
                
                t += transitionSpeed * Time.deltaTime;
                yield return null;
            }

            callBack?.Invoke();
        }
    }
}