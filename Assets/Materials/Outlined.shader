Shader "Custom/OutlineInvertedHull"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0.1, 0.8, 1, 1)
        _OutlineWidth ("Outline Width (world)", Float) = 0.02
        _Alpha ("Alpha", Range(0,1)) = 1
    }
    SubShader
    {
        // Draw just after regular geometry so it tucks under nicely
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
        LOD 100

        // We draw the backfaces, slightly expanded along normals.
        Cull Front
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _OutlineColor;
            float _OutlineWidth;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);

                // Sign fix for negative (mirrored) scales so width stays consistent
                float signW = unity_WorldTransformParams.w;

                // Expand along *object-space* normal by world-space width
                // Convert obj-space normal to world-space direction magnitude using object to world
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                worldPos += worldNormal * _OutlineWidth * signW;

                o.pos = UnityWorldToClipPos(float4(worldPos, 1));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(_OutlineColor.rgb, _Alpha);
            }
            ENDCG
        }
    }
    FallBack Off
}
