using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RTS.SpawnObject {
    public class SpawnOnStartAuthoringAuthoring : MonoBehaviour {
        public GameObject prefab;
        public Vector3 area;
        public int count;

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube( transform.position, area );
        }


        class SpawnOnStartAuthoringBaker : Baker<SpawnOnStartAuthoringAuthoring> {
            public override void Bake(SpawnOnStartAuthoringAuthoring authoring) {
                var entity = GetEntity( TransformUsageFlags.Dynamic );
                AddComponent( entity, new SpawnOnStartAuthoringComponentData {
                    prefab = GetEntity( authoring.prefab, TransformUsageFlags.Dynamic ),
                    count =  authoring.count,
                    area = authoring.area,
                    position = authoring.transform.position,
                    random = new Random( (uint)new System.Random().Next() )
                } );
            }
        }
    }


    public struct SpawnOnStartAuthoringComponentData : IComponentData, IEnableableComponent {
        public Entity prefab;
        public int count;
        public float3 area;
        public float3 position;
        public Random random;
    }
    
    
}