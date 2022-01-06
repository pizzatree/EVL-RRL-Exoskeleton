// Allows user to update the displacement manually

using UnityEngine;

namespace ExoskeletonInteraction
{
    public class ExoskeletonDisplacement : MonoBehaviour
    {
        [SerializeField] private Vector3 displacement;

        private Vector3 startingPosition;

        private void Start() 
            => startingPosition = transform.localPosition;

        public void SetDisplacement(Vector3 newDisplacement)
        {
            displacement = newDisplacement;
            UpdateDisplacement();
        }

        private void UpdateDisplacement()
            => transform.localPosition = startingPosition + displacement;
    }
}
