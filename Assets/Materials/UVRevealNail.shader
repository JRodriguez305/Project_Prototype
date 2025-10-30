Shader "Custom/UVNailReveal"
{
    Properties
    {
        _MainTex ("Nail Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0.6, 0.6, 1, 1)
        _GlowIntensity ("Glow Intensity", Float) = 3
        _ConeAngle ("Spotlight Cone Angle", Float) = 0.5
        _MaxRange ("Max Range", Float) = 10
        _LightOn ("Light On", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float3 _LightPos;
            float3 _LightDir;
            float _ConeAngle;
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _MaxRange;
            float _LightOn;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);

                float3 fragDir = i.worldPos - _LightPos;
                float dist = length(fragDir);
                float3 normDir = normalize(fragDir);

                float angleDot = dot(normDir, normalize(_LightDir));
                float coneMask = smoothstep(cos(_ConeAngle), 1.0, angleDot);
                float rangeMask = saturate(1.0 - dist / _MaxRange);
                float visibility = coneMask * rangeMask * _LightOn;

                if (visibility <= 0.01)
                    discard;

                fixed4 col;
                col.rgb = texCol.rgb * _GlowColor.rgb * _GlowIntensity * visibility;
                col.a = texCol.a * visibility;

                return col;
            }
            ENDCG
        }
    }
}
