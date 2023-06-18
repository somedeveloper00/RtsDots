using Unity.Entities;
using UnityEngine;

namespace RTS.Tower {
    public class TowerAuthoring : MonoBehaviour {
        public float range;

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere( transform.position, range );
        }

        public class TowerAuthoringBaker : Baker<TowerAuthoring> {
            public override void Bake(TowerAuthoring authoring) {
                var entity = GetEntity( TransformUsageFlags.Dynamic );
                AddComponent( entity, new TowerComponentData { range = authoring.range } );
            }
        }
    }


    public struct TowerComponentData : IComponentData {
        public float range;
    }
}