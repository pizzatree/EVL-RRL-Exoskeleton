// Handles the setup and updating of line renderers for the exoskeletons
using System.Collections.Generic;
using MarkerDataTypes;
using UnityEngine;

namespace ExoskeletonInteraction
{
    public class ExoskeletonLineRenderer : MonoBehaviour
    {
        [SerializeField] private Material lrMaterial;
        [SerializeField] private float    lineThickness = 0.01f;

        private List<TransformConnection> connections;

        public Dictionary<string, TransformConnection>       ConnectionsByName { get; private set; }
        public Dictionary<TransformConnection, LineRenderer> LrsByConnection   { get; private set; }

        private void Update()
        {
            if(connections is null)
                return;

            foreach(var connection in connections)
            {
                var lr = LrsByConnection[connection];
                lr.SetPosition(0, connection.Head.position);
                lr.SetPosition(1, connection.Tail.position);
            }
        }

        public void SetTerminals(List<TransformConnection> connections)
        {
            this.connections = connections;

            if(LrsByConnection != null)
                foreach(var lineRenderer in LrsByConnection)
                    Destroy(lineRenderer.Value.gameObject);

            LrsByConnection   = new Dictionary<TransformConnection, LineRenderer>();
            ConnectionsByName = new Dictionary<string, TransformConnection>();

            foreach(var connection in connections)
            {
                var lr = new GameObject($"LineRenderer: {connection.Head.name} - {connection.Tail.name}",
                                        typeof(LineRenderer))
                    .GetComponent<LineRenderer>();
                lr.transform.SetParent(transform);

                lr.material   = lrMaterial;
                lr.widthCurve = AnimationCurve.Constant(0f, 1f, lineThickness);
                lr.SetPosition(0, connection.Head.position);
                lr.SetPosition(1, connection.Tail.position);

                ConnectionsByName.Add(lr.name, connection);
                LrsByConnection.Add(connection, lr);
            }
        }
    }
}