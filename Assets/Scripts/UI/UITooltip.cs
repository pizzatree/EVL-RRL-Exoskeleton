// Allows mousing over elements in 3D space and getting tooltips by the mouse

using TMPro;
using UnityEngine;

namespace UI
{
    public class UITooltip : MonoBehaviour
    {
        private RectTransform self, canvas;
        private CanvasGroup   canvasGroup;
        private TMP_Text      text;

        private ITooltip currentTooltip;

        private bool open;

        private void Start()
        {
            self        = GetComponent<RectTransform>();
            canvas      = transform.parent.GetComponent<RectTransform>();
            canvasGroup = GetComponentInParent<CanvasGroup>();
            text        = GetComponentInChildren<TMP_Text>();

            canvasGroup.alpha = 0f;
            open              = false;
        }

        private void LateUpdate()
        {
            self.anchoredPosition = Input.mousePosition / canvas.localScale.x;

            var hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 50f,
                                      LayerMask.GetMask("Tooltip"));
        
            if(open && !hit)
                CloseTooltip();

            if(open || !hit)
                return;

            currentTooltip = hitInfo.collider.GetComponent<ITooltip>();
            var tip = currentTooltip.GetTip();
            OpenTooltip(tip);
        }
    
        private void OpenTooltip(string tip)
        {
            open              = true;
            text.text         = tip;
            canvasGroup.alpha = 1f;
        }

        private void CloseTooltip()
        {
            currentTooltip?.EndTip();
        
            open              = false;
            canvasGroup.alpha = 0f;
        }
    }
}
