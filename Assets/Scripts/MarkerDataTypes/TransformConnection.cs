using UnityEngine;

namespace MarkerDataTypes
{
    public class TransformConnection
    {
        public readonly Transform Head;
        public readonly Transform Tail;

        public TransformConnection(Transform head, Transform tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}