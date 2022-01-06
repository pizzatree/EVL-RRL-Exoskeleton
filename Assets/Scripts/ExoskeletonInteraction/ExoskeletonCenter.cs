// Attempts to center (every frame) an exoskeleton based on the average X, Z plane position of a group of pre specified
// markers. Continually searches if the markers exist, would be better to use an event

using System.Collections.Generic;
using MarkerDataTypes;
using UnityEngine;

namespace ExoskeletonInteraction
{
    public class ExoskeletonCenter : MonoBehaviour
    {
        [SerializeField] private MarkerGroup markerGroup;

        private void Update()
        {
            var transforms = new List<Transform>();

            foreach(var markerName in markerGroup.MarkerNames)
            {
                var t = transform.Find(markerName);

                if(!t)
                    return;

                transforms.Add(t);
            }

            var avgPos = Vector3.zero;
            foreach(var t in transforms)
                avgPos += t.localPosition;

            avgPos /= transforms.Count;

            transform.localPosition = new Vector3(-avgPos.x, 0f, -avgPos.z);
        }
    }
}