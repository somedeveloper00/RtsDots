using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace RTS.SpawnObject {
    public partial struct SpawnOnStartSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer( state.WorldUnmanaged );
            foreach (var (spawn, entity) in SystemAPI.Query<RefRW<SpawnOnStartAuthoringComponentData>>().WithEntityAccess()) {
                for (int i = 0; i < spawn.ValueRO.count; i++) {
                    var pos = spawn.ValueRO.position + spawn.ValueRW.random.NextFloat3( -spawn.ValueRO.area / 2f, spawn.ValueRO.area / 2f );
                    var newEntity =  ecb.Instantiate( spawn.ValueRO.prefab );
                    ecb.SetComponent( newEntity, LocalTransform.FromPosition( pos ) );
                    ecb.RemoveComponent( entity, ComponentType.ReadOnly<SpawnOnStartAuthoringComponentData>() );
                }
            }
        }

        [BurstCompile]  
        public void OnDestroy(ref SystemState state) { }
    }
}