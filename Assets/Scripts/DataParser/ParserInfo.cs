// For retrieving data from an IParser
using System.Collections.Generic;
using Markers;

namespace DataParser
{
    public class ParserInfo
    {
        public List<string>                Titles;
        public Dictionary<string, IMarkerData> Markers;
        public int                         FrameCount;
    }
}