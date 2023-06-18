using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RTS {
    public partial struct RtsCameraMovementSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        // [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (camera, transform) in SystemAPI.Query<RefRW<RtsCameraMovementComponentData>, RefRW<LocalTransform>>()) {
                
                var deltaTime = SystemAPI.Time.DeltaTime;

                // apply yaw
                var yaw = camera.ValueRO.yawRotationSpeed * deltaTime * camera.ValueRO.rotateYaw;
                var yawRotation = quaternion.RotateY( yaw );
                transform.ValueRW.Rotation = math.mul( yawRotation, transform.ValueRW.Rotation );
                
                // apply pitch
                var pitch = camera.ValueRO.pitchRotationSpeed * deltaTime * camera.ValueRO.rotatePitch;
                var pitchRotation = quaternion.RotateX( pitch );
                transform.ValueRW.Rotation = math.mul( transform.ValueRW.Rotation, pitchRotation );
                
                // apply navigation along x-z
                var move = camera.ValueRO.navigate * deltaTime * camera.ValueRO.navigationSpeed;
                transform.ValueRW.Position.xz += (transform.ValueRW.Forward() * move.y + transform.ValueRW.Right() * move.x).xz;
                
                // apply navigation along y
                var yMove = camera.ValueRO.changeHeight * deltaTime * camera.ValueRO.changeHeightSpeed;
                transform.ValueRW.Position.y += yMove;
                
                // cooldown
                camera.ValueRW.rotateYaw = mathUtils.moveTowards( camera.ValueRO.rotateYaw, 0, deltaTime * camera.ValueRO.yawRotationCoolDown );
                camera.ValueRW.rotatePitch = mathUtils.moveTowards( camera.ValueRO.rotatePitch, 0, deltaTime * camera.ValueRO.pitchRotationCoolDown );
                camera.ValueRW.navigate = mathUtils.moveTowards( camera.ValueRO.navigate, float2.zero, camera.ValueRO.navigationCoolDown );
                camera.ValueRW.changeHeight = mathUtils.moveTowards( camera.ValueRO.changeHeight, 0, deltaTime * camera.ValueRO.changeHeightCoolDown );
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}