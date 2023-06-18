using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace RTS {
    public class RtsNameAuthoring : MonoBehaviour {
        public new string name;
        public class RtsNameAuthoringBaker : Baker<RtsNameAuthoring> {
            public override void Bake(RtsNameAuthoring authoring) {
                var entity = GetEntity( TransformUsageFlags.Dynamic );
                AddComponent( entity, new RtsNameComponentData { name = authoring.name } );
            }
        }
    }


    public struct RtsNameComponentData : IComponentData {
        public FixedString32Bytes name;
    }
}