// Marker Data stored in a Dictionary
// For longer recorded data, allows scrubbing without a delay

using System.Collections.Generic;
using UnityEngine;

namespace Markers
{
    public class MarkerDataDictionary : IMarkerData
    {
        private readonly Dictionary<int, FrameData> frames;

        private readonly string markerName;


        public MarkerDataDictionary(string markerName)
        {
            frames          = new Dictionary<int, FrameData>();
            this.markerName = markerName;
        }

        public string GetName() => markerName;

        public FrameData this[int frameNumber]
        {
            get
            {
                var success = frames.TryGetValue(frameNumber, out var data);
                if(success)
                    return data;

                Debug.LogError($"Frame data at {frameNumber} is nonexistent.");
                return null;
            }

            set => Add(frameNumber, value);
        }

        public bool TryGetPositionAtFrame(int frameNumber, out Vector3 position)
        {
            var success = frames.TryGetValue(frameNumber, out var data);
            position = data?.Position ?? default;

            return success;
        }

        public void Add(int frameNumber, FrameData frameData)
        {
            if(!ValidFrameAdd(frameNumber))
                Debug.LogWarning($"Frame inserted at {frameNumber} fails context checks.");

            frames.Add(frameNumber, frameData);
        }

        private bool ValidFrameAdd(int frameNumber)
        {
            switch(frameNumber)
            {
                case < 1:
                case > 2 when !frames.ContainsKey(frameNumber - 1):
                    return false;
                default:
                    return true;
            }
        }
    }
}