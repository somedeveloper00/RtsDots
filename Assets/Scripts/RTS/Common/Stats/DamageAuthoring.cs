using Unity.Entities;
using UnityEngine;

namespace RTS {
    public class DamageAuthoring : MonoBehaviour {
        public float damage;
        public float damageRate;
        
        public class DamageAuthoringBaker : Baker<DamageAuthoring> {
            public override void Bake(DamageAuthoring authoring) {
                var entity = GetEntity( TransformUsageFlags.Dynamic );
                AddComponent( entity,
                    new DamageComponentData { damage = authoring.damage, damageRate = authoring.damageRate } );
            }
        }
    }


    public struct DamageComponentData : IComponentData {
        public float damage;
        public float damageRate;
    }
}