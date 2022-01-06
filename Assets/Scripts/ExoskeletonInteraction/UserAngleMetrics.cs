// Colors the line renderers based on severity of off angle postions
// Sensitivity is controlled by strictness (exposed in UI)
// Creates and controls the corrective action arrows as well

using System.Collections.Generic;
using MarkerDataTypes;
using UnityEngine;

namespace ExoskeletonInteraction
{
    public class UserAngleMetrics : MonoBehaviour
    {
        [SerializeField] private ExoskeletonLineRenderer control, user;
        [SerializeField] private float                   defaultStrictness = 6f;

        [Header("Line Renderer")] 
        [SerializeField] private Color goodColor = Color.green;
        [SerializeField] private Color astrayColor = Color.red;

        [Header("Arrows")] 
        [SerializeField] private GameObject arrowPrefab;
        [Tooltip("Modified at runtime by the arrow button\n" +
                 "Enter pairs of connected markers for arrow feedback.")]
        [SerializeField] private List<MarkerSet> arrowMarkers;
        
        [Tooltip("t values under this number will draw an arrow")]
        [SerializeField, Range(0f, 1f)] private float tThreshold = 0.51f;

        private List<Transform> arrowTransforms;
        
        private float           strictness;

        private void Start()
        {
            strictness      = defaultStrictness;
            arrowTransforms = new List<Transform>();
        }

        public void AdjustStrictness(string input)
            => strictness = float.TryParse(input, out var value) ? value : defaultStrictness;

        public void SetMarkerSets(List<MarkerSet> newSets)
            => arrowMarkers = newSets;

        private void Update()
        {
            if(control.ConnectionsByName == null || user.ConnectionsByName == null)
                return;

            var numMarkers = arrowMarkers.Count;
            if(arrowTransforms.Count != numMarkers)
                CreateNewArrows(numMarkers);
        
            UpdateVisualMetrics();
        }
        
        private void CreateNewArrows(int numMarkers)
        {
            foreach(var arrowTransform in arrowTransforms)
                Destroy(arrowTransform.gameObject);
        
            arrowTransforms.Clear();

            for(int i = 0; i < numMarkers; ++i)
            {
                var arrow = Instantiate(arrowPrefab).transform;
                arrow.SetParent(transform, true);
                arrowTransforms.Add(arrow);
            }    
        }

        private void UpdateVisualMetrics()
        {
            foreach(var controlPair in control.ConnectionsByName)
            {
                // Resources
                var controlName       = controlPair.Key;
                var controlConnection = controlPair.Value;

                var userConnection = user.ConnectionsByName[controlName];
                var userLR         = user.LrsByConnection[userConnection];

                // Maths
                var controlDispl = controlConnection.Tail.localPosition - controlConnection.Head.localPosition;
                var userDispl    = userConnection.Tail.localPosition    - userConnection.Head.localPosition;

                var dot   = Vector3.Dot(controlDispl.normalized, userDispl.normalized);
                var t     = Mathf.Pow(dot, strictness);
                var color = Color.Lerp(astrayColor, goodColor, t);

                // Update line renderer color
                userLR.startColor = userLR.endColor = color;

                // Possibly update the arrow for this region
                UpdateArrow(controlName, t, controlDispl, userDispl, userConnection);
            }
        }
        
        private void UpdateArrow(string connectionName, float t, Vector3 controlDisp, Vector3 userDisp, TransformConnection user)
        {
            for(var i = 0; i < arrowMarkers.Count; i++)
            {
                var set = arrowMarkers[i];
                if(!connectionName.Contains(set.Head) || !connectionName.Contains(set.Tail))
                    continue;

                var arrow = arrowTransforms[i];
            
                var dif = controlDisp - userDisp;
                if(dif.sqrMagnitude == 0f)
                    return;

                arrow.position = (user.Head.position + user.Tail.position) / 2;

                if(t > tThreshold)
                {
                    arrow.localScale = Vector3.Lerp(arrow.localScale, Vector3.one * 0.001f, t + .1f);
                    return;
                }

                arrow.localScale = Vector3.Lerp(arrow.localScale, Vector3.one * (1f - t), .9f);
            
                // Look towards object, but also try to keep the z-axis visible to user
                arrow.localRotation = Quaternion.LookRotation(dif) *
                                      Quaternion.Euler(0f, 0f, transform.parent.rotation.eulerAngles.y);
            }
        }
    }
}