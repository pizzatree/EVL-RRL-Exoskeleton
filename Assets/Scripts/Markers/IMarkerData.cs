// Interface for storage of marker data, allows array indexers
using UnityEngine;

namespace Markers
{
    public interface IMarkerData
    {
        bool   TryGetPositionAtFrame(int frameNumber, out Vector3 position);
        void   Add(int                   frameNumber, FrameData   frameData);
        string GetName();
        FrameData this[int             frameNumber] { get; set; }
    }
}