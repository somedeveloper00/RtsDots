using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTS.Unit {
    [BurstCompile]
    public partial struct UnitSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) => state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {

            var enemyQuery = SystemAPI.QueryBuilder()
                .WithAll<LocalTransform>()
                .WithAll<UnitComponentData>()
                .WithAll<EnemyTag>()
                .Build();

            var enemyLocalTransforms = enemyQuery.ToComponentDataArray<LocalTransform>( Allocator.TempJob );
            var enemyUnits = enemyQuery.ToComponentDataArray<UnitComponentData>( Allocator.TempJob );

            

            var allyQuery = SystemAPI.QueryBuilder()
                .WithAll<LocalTransform>()
                .WithAll<UnitComponentData>()
                .WithAll<AllyTag>()
                .Build();

            var allyLocalTransforms = allyQuery.ToComponentDataArray<LocalTransform>( Allocator.TempJob );
            var allyUnits = allyQuery.ToComponentDataArray<UnitComponentData>( Allocator.TempJob );

            
            state.Dependency = new AllyUnitMovementJob {
                deltaTime = SystemAPI.Time.DeltaTime,
                enemyTransforms = enemyLocalTransforms,
                enemyUnits = enemyUnits,
                enemyCount = enemyLocalTransforms.Length
            }.ScheduleParallel( state.Dependency );
            
            state.Dependency = new EnemyUnitMovementJob {
                deltaTime = SystemAPI.Time.DeltaTime,
                enemyTransforms = allyLocalTransforms,
                enemyUnits = allyUnits,
                enemyCount = allyLocalTransforms.Length
            }.ScheduleParallel( state.Dependency );
            
            // state.CompleteDependency();
            state.CompleteDependency();
            enemyLocalTransforms.Dispose();
            enemyUnits.Dispose();
            allyLocalTransforms.Dispose();
            allyUnits.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
    
    [BurstCompile]
    [WithAll(typeof(AllyTag))] 
    public partial struct AllyUnitMovementJob : IJobEntity {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public NativeArray<LocalTransform> enemyTransforms;
        [ReadOnly] public NativeArray<UnitComponentData> enemyUnits;
        [ReadOnly] public int enemyCount;

        public void Execute(ref LocalTransform transform, in UnitComponentData unit) {
            LocalTransform closestEnemy = default;
            var closestDistance = float.MaxValue;
            bool found = false;

            for (int i = 0; i < enemyCount; i++) {
                var d = math.distancesq( enemyTransforms[i].Position, transform.Position );
                if (d < closestDistance) {
                    closestDistance = d;
                    closestEnemy = enemyTransforms[i];
                }

                found = true;
            }
            if (found == false) return;

            if (closestDistance > unit.range) {
                transform.Position += math.normalize( closestEnemy.Position - transform.Position ) * unit.moveSpeed * deltaTime;
            }
        }
    }

    [BurstCompile]
    [WithAll(typeof(EnemyTag))] 
    public partial struct EnemyUnitMovementJob : IJobEntity {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public NativeArray<LocalTransform> enemyTransforms;
        [ReadOnly] public NativeArray<UnitComponentData> enemyUnits;
        [ReadOnly] public int enemyCount;

        public void Execute(ref LocalTransform transform, in UnitComponentData unit) {
            LocalTransform closestEnemy = default;
            var closestDistance = float.MaxValue;
            bool found = false;

            for (int i = 0; i < enemyCount; i++) {
                var d = math.distancesq( enemyTransforms[i].Position, transform.Position );
                if (d < closestDistance) {
                    closestDistance = d;
                    closestEnemy = enemyTransforms[i];
                }

                found = true;
            }
            if (found == false) return;

            if (closestDistance > unit.range) {
                transform.Position += math.normalize( closestEnemy.Position - transform.Position ) * unit.moveSpeed * deltaTime;
            }
        }
    }
}