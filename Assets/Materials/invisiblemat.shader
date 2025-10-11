Shader "Custom/UVSpotGlow_Neon"
{
    Properties
    {
        _GlowColor ("Glow Color", Color) = (0.5, 0, 1, 1)
        _ConeAngle ("Spotlight Cone Angle", Range(0.1, 1)) = 0.5
        _Intensity ("Glow Intensity", Range(0, 20)) = 10
        _Range ("Glow Range", Float) = 10
        _MinDistance ("Min Bright Distance", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend One One
        Cull Off
        ZWrite Off

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
            float _Intensity;
            float _Range;
            float _MinDistance;
            fixed4 _GlowColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 fragToLight = i.worldPos - _LightPos;
                float dist = length(fragToLight);

                // Prevent extreme brightness near zero distance
                dist = max(dist, _MinDistance);

                float3 fragDir = normalize(fragToLight);
                float angleDot = dot(fragDir, normalize(_LightDir));
                float angleFalloff = smoothstep(cos(_ConeAngle), 1.0, angleDot);
                float distanceFalloff = saturate(1.0 - (dist / _Range));

                // Optional soft rolloff instead of linear
                distanceFalloff = pow(distanceFalloff, 2.0);

                float intensity = angleFalloff * distanceFalloff;
                float3 color = _GlowColor.rgb * _Intensity * intensity;

                return fixed4(color, intensity);
            }
            ENDCG
        }
    }
}
