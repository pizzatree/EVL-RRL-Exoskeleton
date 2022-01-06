// Interprets the sticks / marker connections contained in .VSK files
// Loads once per app run

using System.Collections.Generic;
using System.Xml.Linq;
using UI;
using UnityEngine;

namespace DataParser
{
    public class ConnectionsParser : MonoBehaviour
    {
        private static List<(string, string)> _connections;

        public static List<(string, string)> GetConnects()
        {
            if(_connections != null)
                return _connections;
        
            // awful but works
            var paths = FindObjectOfType<FileOpener>().OpenVSKPanel();
            if(paths.Length.Equals(0))
                return null;
            var path = paths[0];
        
            var xmlDoc = XDocument.Load(path);
            var sticks = xmlDoc.Descendants("Sticks").Elements();

            _connections = new List<(string, string)>();

            foreach(var stick in sticks)
            {
                var head = stick.Attribute("MARKER1").Value;
                var tail = stick.Attribute("MARKER2").Value;
            
                _connections.Add((head, tail));
            }

            return _connections;
        }
    }
}
