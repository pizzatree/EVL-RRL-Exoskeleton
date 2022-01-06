// Allows facilitator to input which connections they would like to see Arrow feedback for
// Doing every connection in such a way that one can see the arrows seemed too fatiguing on the eyes
// If this is not desired, all of the connections can be loaded within UserAngleMetrics.cs

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExoskeletonInteraction;
using MarkerDataTypes;
using UnityEngine;

namespace UI
{
    public class ArrowsEntry : MonoBehaviour
    {
        [SerializeField] private GameObject entryField;

        private UserAngleMetrics userAngleMetrics;

        private void Start() 
            => userAngleMetrics = FindObjectOfType<UserAngleMetrics>();

        public void ToggleEntryField()
            => entryField.SetActive(!entryField.activeSelf);

        public void ValidateInput(string input)
        {
            var lines = input.Split('\n'); // System.Environment.NewLine wasn't working
                                                        // might cause oddities if platform is changed
            var markerSets = new List<MarkerSet>();
        
            foreach(var line in lines)
            {
                var entries = line.Split(',');
                if(entries.Length < 2)
                    continue;

                var head = entries[0].ToUpper();
                var tail = entries[1].ToUpper();
            
                // Remove everything but numbers and letters.
                head = Regex.Replace(head, @"[^a-zA-Z0-9]", "");
                tail = Regex.Replace(tail, @"[^a-zA-Z0-9]", "");

                markerSets.Add(new MarkerSet(head, tail));
            }

            userAngleMetrics.SetMarkerSets(markerSets);
        }
    }
}