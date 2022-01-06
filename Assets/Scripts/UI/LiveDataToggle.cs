// When the Live Data toggle is switched on this enables the relevant objects (like IP input)
// and disables irrelevant ones (like the scrubber) as long as they're in the proper arrays.
using UnityEngine;

namespace UI
{
    public class LiveDataToggle : MonoBehaviour
    {
        [SerializeField] private GameObject[] liveObjects;
        [SerializeField] private GameObject[] sampleObjects;
    
        private void Start() 
            => Toggle(true);

        public void Toggle(bool active)
        {
            foreach(var liveObject in liveObjects)
                liveObject.SetActive(active);

            foreach(var sampleObject in sampleObjects)
                sampleObject.SetActive(!active);
        }
    }
}