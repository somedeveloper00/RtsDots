using Unity.Entities;
using UnityEngine;

namespace RTS {
    public class LifeAuthoring : MonoBehaviour {
        public float maxLife;
        public float currentLife;
        
        public class LifeAuthoringBaker : Baker<LifeAuthoring> {
            public override void Bake(LifeAuthoring authoring) {
                var entity = GetEntity( authoring, TransformUsageFlags.Dynamic );
                AddComponent( entity, new LifeComponentData { maxLife = authoring.maxLife, currentLife = authoring.currentLife } );
            }
        }
    }


    public struct LifeComponentData : IComponentData {
        public float maxLife;
        public float currentLife;
    }
}