Shader "Custom/UVGlow_NeonVisible"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (0.5,0,1,1)
        _GlowIntensity ("Glow Intensity", Float) = 5
        _ConeAngle ("Spotlight Cone Angle", Float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float3 _LightPos;
            float3 _LightDir;
            float _ConeAngle;
            fixed4 _BaseColor;
            fixed4 _GlowColor;
            float _GlowIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Always visible base color
                fixed4 col = _BaseColor;

                // Glow calculation
                float3 fragDir = normalize(i.worldPos - _LightPos);
                float dotAngle = dot(fragDir, normalize(_LightDir));
                float intensity = smoothstep(cos(_ConeAngle), 1.0, dotAngle);

                // Add neon glow only where light hits
                col.rgb += _GlowColor.rgb * _GlowIntensity * intensity;

                return col;
            }
            ENDCG
        }
    }
}
