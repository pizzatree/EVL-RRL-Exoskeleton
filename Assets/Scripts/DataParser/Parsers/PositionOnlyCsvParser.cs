using System.Collections.Generic;
using System.IO;
using Markers;
using UnityEngine;

namespace DataParser.Parsers
{
    public class PositionOnlyCsvParser : IParser
    {
        public ParserInfo ParseFileAt(string loc)
        {
            var titles  = new List<string>();
            var markers = new Dictionary<string, IMarkerData>();

            var frameCount = 0;

            var reader = new StreamReader(loc);

            // Get through junk & read marker titles
            while(!reader.EndOfStream)
            {
                reader.ReadLine();
                reader.ReadLine();

                var line = reader.ReadLine();

                PopulateTitles(titles, line);

                reader.ReadLine();
                break;
            }

            // Read marker positions
            while(!reader.EndOfStream)
            {
                var line     = reader.ReadLine();
                var numSplit = line.Split(',');

                int frame = -1;
                if(int.TryParse(numSplit[0], out var data))
                    frame = data;

                if(frame < 1)
                    continue;

                // Probably will remain unused, but titled just in case
                int subFrame = -1;
                if(int.TryParse(numSplit[1], out var data2))
                    subFrame = data2;

                frameCount = Mathf.Max(frame, frameCount);
            
                ParsePositionalData(titles, numSplit, markers, frame);
            }

            return new ParserInfo()
            {
                Titles     = titles,
                Markers    = markers,
                FrameCount = frameCount
            };
        }

        private static void ParsePositionalData(List<string> titles, string[] numSplit, Dictionary<string, IMarkerData> markers, int frame)
        {
            for(int i = 0; i < titles.Count; i++)
            {
                var validX = float.TryParse(numSplit[2 + (i * 3)], out var x);
                var validY = float.TryParse(numSplit[3 + (i * 3)], out var y);
                var validZ = float.TryParse(numSplit[4 + (i * 3)], out var z);

                if(!validX || !validY || !validZ)
                    continue;

                var markerTitle = titles[i];
                if(!markers.ContainsKey(markerTitle))
                    markers.Add(markerTitle, new MarkerDataDictionary(markerTitle));

                // x, z, y in order to match the flipped coordinate systems between VICON and Unity
                var frameData = new FrameData() { Position = new Vector3(x, z, y) };
                markers[markerTitle].Add(frame, frameData);
            }
        }

        private static void PopulateTitles(List<string> titles, string titlesLine)
        {
            var titleSplit = titlesLine.Split(',');

            foreach(var s in titleSplit)
            {
                if(s.Length < 2)
                    continue;

                titles.Add(s.Split(':')[1]);
            }
        }
    }
}