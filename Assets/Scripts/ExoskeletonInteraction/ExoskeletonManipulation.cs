// Allows user to click and drag the Exoskeletons in order to rotate them

using UnityEngine;

namespace ExoskeletonInteraction
{
    public class ExoskeletonManipulation : MonoBehaviour
    {
        [SerializeField] private float rotSpeed         = 10f,
                                       translationSpeed = 3f;

        private Vector3 lastMousePos;

        private void OnMouseDown()
            => lastMousePos = Input.mousePosition;

        private void OnMouseDrag()
        {
            var mousePos = Input.mousePosition;

            if(Input.GetMouseButton(1))
                HandleMovement(mousePos);
            else
                HandleRotation(mousePos);

            lastMousePos = mousePos;
        }

        private void HandleRotation(Vector3 mousePos)
        {
            var displacement = mousePos.x - lastMousePos.x;
            transform.rotation *= Quaternion.Euler(0f, displacement * rotSpeed * Time.deltaTime, 0f);
        }

        private void HandleMovement(Vector3 mousePos)
        {
            var displacement = mousePos - lastMousePos;
            transform.position += new Vector3(displacement.x * translationSpeed * Time.deltaTime, 0f,
                                displacement.y * translationSpeed * Time.deltaTime);
        }
    }
}