using Unity.Burst;
using Unity.Mathematics;



public static class mathUtils {
    [BurstCompile]
    public static float moveTowards(this float current, float target, float maxDelta) {
        var d = target - current;
        d = math.abs( d ) > maxDelta ? math.sign( d ) * maxDelta : d;
        return current + d;
    }
    [BurstCompile]
    public static float2 moveTowards(this float2 current, float2 target, float maxDelta) {
        var d = target - current;
        d = math.length( d ) > maxDelta ? math.normalize( d ) * maxDelta : d;
        return current + d;
    }
    [BurstCompile]
    public static float3 moveTowards(this float3 current, float3 target, float maxDelta) {
        var d = target - current;
        d = math.length( d ) > maxDelta ? math.normalize( d ) * maxDelta : d;
        return current + d;
    }
    [BurstCompile]
    public static float4 moveTowards(this float4 current, float4 target, float maxDelta) {
        var d = target - current;
        d = math.length( d ) > maxDelta ? math.normalize( d ) * maxDelta : d;
        return current + d;
    }
}