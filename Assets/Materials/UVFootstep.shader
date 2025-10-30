Shader "Custom/UVFootstepOutline"
{
    Properties
    {
        _FootprintTex ("Footprint Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0.5,0,1,1)
        _GlowIntensity ("Glow Intensity", Float) = 5
        _ConeAngle ("Spotlight Cone Angle", Float) = 0.5
        _LightOn ("Light On", Float) = 0     // ✅ new toggle sent from script
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend One One
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _FootprintTex;
            float3 _LightPos;
            float3 _LightDir;
            float _ConeAngle;
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _LightOn;   // ✅ added

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
                // Sample footprint / nail mask
                float alpha = tex2D(_FootprintTex, i.uv).r;

                // Optional edge softness for glow
                float edge = smoothstep(0.1, 0.3, alpha);

                // Calculate spotlight influence
                float3 fragDir = normalize(i.worldPos - _LightPos);
                float dotAngle = dot(fragDir, normalize(_LightDir));

                // Falloff from cone edge to center
                float intensity = smoothstep(cos(_ConeAngle), 1.0, dotAngle);

                // ✅ Multiply by _LightOn so it’s completely dark when UV is off
                intensity *= _LightOn;

                // Final color output
                fixed4 finalCol;
                finalCol.rgb = _GlowColor.rgb * _GlowIntensity * intensity * edge;
                finalCol.a = intensity * edge;

                return finalCol;
            }
            ENDCG
        }
    }
}
