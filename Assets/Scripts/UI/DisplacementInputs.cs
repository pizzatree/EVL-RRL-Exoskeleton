// Forwards manual displacement inputs to anyone subscribed to the UnityEvent
// Called by UnityEvents on the input fields

using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class DisplacementInputs : MonoBehaviour
    {
        [SerializeField] private TMP_InputField      x, y, z;
        [SerializeField] private UnityEvent<Vector3> OnUpdatedInputfield;

        public void UpdatedInput()
        {
            float.TryParse(x.text, out var xVal);
            float.TryParse(y.text, out var yVal);
            float.TryParse(z.text, out var zVal);
        
            OnUpdatedInputfield?.Invoke(new Vector3(xVal, yVal, zVal));
        }
    }
}
