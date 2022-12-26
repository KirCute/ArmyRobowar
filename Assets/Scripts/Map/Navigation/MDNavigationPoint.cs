using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Navigation {
    [ExecuteInEditMode]
    public class MDNavigationPoint : MonoBehaviour {
        private const int TERRAIN_LAYER_MASK = 1 << 3;
        [SerializeField] private float acceptDistance = 10.0f;
        [SerializeField] private List<MDNavigationPoint> neighbors = new();
        private readonly List<MDNavigationPoint> closes = new();
        private Vector3 lastPosition;
        public float G { get; private set;}
        public float H { get; private set;}
        
        public MDNavigationPoint parent { get; private set; }
        public float F => G + H;

        public void SetG(float g)
        {
            G = g;
        }

        public void SetH(float h)
        {
            H = h;
        }

        public void SetParent(MDNavigationPoint connection)
        {
            parent = connection;
        }

        private void Update() {
            if (Application.isPlaying) return;
            //if (transform.position != lastPosition) {
                lastPosition = transform.position;
                closes.Clear();
                foreach (var point in transform.parent.gameObject.GetComponentsInChildren<MDNavigationPoint>()) {
                    if (point == this) continue;
                    if ((point.transform.position - transform.position).magnitude > acceptDistance) continue;
                    closes.Add(point);
                }
            //}

            neighbors.Clear();
            foreach (var point in closes.Where(
                         point => !Physics.Linecast(point.transform.position, transform.position, TERRAIN_LAYER_MASK)
                     )) {
                neighbors.Add(point);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.DrawSphere(transform.position, 0.25f);
            foreach (var neighbor in neighbors) {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }

        public IReadOnlyList<MDNavigationPoint> GetNeighbors() {
            
            return neighbors;
        }
    }
}