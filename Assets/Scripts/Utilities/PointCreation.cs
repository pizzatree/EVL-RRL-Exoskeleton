using UI;
using UnityEngine;

namespace Utilities
{
    public static class PointCreation
    {
        public static GameObject CreatePoint(Transform parent, float pointSize, Material pointMaterial, string mName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = mName;
            go.transform.SetParent(parent, false);
            go.transform.localScale                    *= pointSize;
            go.GetComponent<Renderer>().material       =  pointMaterial;
            go.GetComponent<Renderer>().material.color =  GenerateColor(go.name);
            go.GetComponent<SphereCollider>().radius   *= 2f;
            go.AddComponent<MarkerTooltip>();

            return go;
        }
    
        private static Color GenerateColor(string objName)
        {
            var hash      = Mathf.Abs(objName.GetHashCode());
            var colorCode = $"#{hash}".Substring(0, 7);
            ColorUtility.TryParseHtmlString(colorCode, out var color);
            return color;
        }
    }
}