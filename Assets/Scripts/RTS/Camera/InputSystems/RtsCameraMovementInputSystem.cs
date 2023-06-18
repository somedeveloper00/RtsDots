using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RTS {
    public partial struct RtsCameraMovementInputSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            
            float2 navigation = float2.zero;
            float heightAxis = 0;
            float yawAxis = 0;
            float pitchAxis = 0;
            
            // populate from keyboard
            navigation.x += Input.GetKey( KeyCode.D ) ? 1 : 0;
            navigation.x += Input.GetKey( KeyCode.A ) ? -1 : 0;
            navigation.y += Input.GetKey( KeyCode.W ) ? 1 : 0;
            navigation.y += Input.GetKey( KeyCode.S ) ? -1 : 0;
            heightAxis += Input.GetKey( KeyCode.E ) ? 1 : 0;
            heightAxis += Input.GetKey( KeyCode.Q ) ? -1 : 0;
            yawAxis += Input.GetKey( KeyCode.Mouse1 ) ? Input.GetAxis( "Mouse X" ) : 0;
            pitchAxis += Input.GetKey( KeyCode.Mouse1 ) ? -Input.GetAxis( "Mouse Y" ) : 0;

            // apply
            foreach (var camera in SystemAPI.Query<RefRW<RtsCameraMovementComponentData>>()) {
                camera.ValueRW.navigate = navigation;
                camera.ValueRW.changeHeight = heightAxis;
                camera.ValueRW.rotateYaw = yawAxis;
                camera.ValueRW.rotatePitch = pitchAxis;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}