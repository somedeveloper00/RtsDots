using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RTS.Unit {
    public class UnitAuthoring : MonoBehaviour {
        public bool isAlly;
        public float moveSpeed;
        public float range;
        public State state;


        public enum State {
            Idle,
            Moving,
            Attacking
        }


        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere( transform.position, range );
        }


        public class UnitAuthoringBaker : Baker<UnitAuthoring> {
            public override void Bake(UnitAuthoring authoring) {
                var entity = GetEntity( authoring, TransformUsageFlags.Dynamic );
                AddComponent( entity, new UnitComponentData {
                    moveSpeed = authoring.moveSpeed, 
                    state = authoring.state,
                    range = authoring.range
                } );
                if (authoring.isAlly) {
                    AddComponent( entity, new AllyTag() );
                }
                else {
                    AddComponent( entity, new EnemyTag() );
                }
            }
        }
    }


    public struct UnitComponentData : IComponentData {
        public float moveSpeed;
        public float range;
        public UnitAuthoring.State state;
    }
}