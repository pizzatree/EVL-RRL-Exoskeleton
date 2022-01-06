using UnityEngine;

namespace MarkerDataTypes
{
    [CreateAssetMenu(fileName = "MarkerGroup", menuName = "New Marker Group")]
    public class MarkerGroup : ScriptableObject
    {
        public string[] MarkerNames = new[] { "LHEE", "RHEE", "LTOE", "RTOE" };
    }
}