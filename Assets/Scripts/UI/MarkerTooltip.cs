using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MarkerTooltip : MonoBehaviour, ITooltip
    {
        private Renderer renderer;
        private Color    color;
        private Color    highlightColor = Color.white;

        private void Start()
        {
            renderer         = GetComponent<Renderer>();
            gameObject.layer = LayerMask.NameToLayer("Tooltip");
        }

        public string GetTip()
        {
            color                   = renderer.material.color;
            renderer.material.color = highlightColor;
        
            return name;
        }

        public void EndTip() => renderer.material.color = color;
    }
}