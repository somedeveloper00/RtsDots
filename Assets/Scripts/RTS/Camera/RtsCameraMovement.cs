using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTS {
    public class RtsCameraMovement : MonoBehaviour {
        public float yawRotationSpeed = 5;
        public float pitchRotationSpeed = 5;
        public float navigationSpeed = 5;
        public float changeHeightSpeed = 5;
        public float yawRotationCoolDown = 1;
        public float pitchRotationCoolDown = 1;
        public float navigationCoolDown = 1;
        public float changeHeightCoolDown = 1;

        public Entity _entity;
        EntityManager _entityManager;

        void OnEnable() {
            // convert to entity
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _entity = _entityManager.CreateEntity();
            _entityManager.SetName( _entity, "RtsCameraMovement" );
            _entityManager.AddComponentData( _entity, new RtsCameraMovementComponentData {
                yawRotationSpeed = yawRotationSpeed,
                pitchRotationSpeed = pitchRotationSpeed,
                navigationSpeed = navigationSpeed,
                changeHeightSpeed = changeHeightSpeed,
                yawRotationCoolDown = yawRotationCoolDown,
                pitchRotationCoolDown = pitchRotationCoolDown,
                navigationCoolDown = navigationCoolDown,
                changeHeightCoolDown = changeHeightCoolDown,
            } );
            _entityManager.AddComponentData( _entity, LocalTransform.FromPositionRotation( transform.position, transform.rotation ) );
        }

        void LateUpdate() {
            UpdateTransform();
        }

        void UpdateTransform() {
            // check if valid
            if ( _entity == Entity.Null) return;
            if (!_entityManager.Exists( _entity )) {
                Debug.LogWarning( $"Entity is not found." );
                enabled = false;
                return;
            }
            
            var localTransform = _entityManager.GetComponentData<LocalTransform>( _entity );
            transform.position = localTransform.Position;
            transform.rotation = localTransform.Rotation;
        }
    }


    public struct RtsCameraMovementComponentData : IComponentData {
        public float yawRotationSpeed;
        public float yawRotationCoolDown;
        public float pitchRotationSpeed;
        public float pitchRotationCoolDown;
        public float navigationSpeed;
        public float navigationCoolDown;
        public float changeHeightSpeed;
        public float changeHeightCoolDown;
        
        public float rotateYaw;
        public float rotatePitch;
        public float2 navigate;
        public float changeHeight;
    }
}