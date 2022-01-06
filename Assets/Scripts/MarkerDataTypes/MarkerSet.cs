using System;

namespace MarkerDataTypes
{
    [Serializable]
    public class MarkerSet
    {
        public string Head;
        public string Tail;

        public MarkerSet(string head, string tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}