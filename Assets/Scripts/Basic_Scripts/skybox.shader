Shader "Custom/skybox"
{
    Properties
    {
        _MainTex("Texture", Cube) = "white" {}
        _SkyboxDay("Skybox Day", Cube) = "white" {}
        _SunsetTex("Sunset Texture", Cube) = "white" {}
        _SkyboxNight("Skybox Night", Cube) = "white" {}
        _TransitionDuration("Transition Duration", Range(0.0, 1.0)) = 1.0
        _SunTex("Sun Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Background" "RenderType" = "Background"}
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform samplerCUBE _MainTex;
            uniform samplerCUBE _SkyboxDay;
            uniform samplerCUBE _SunsetTex;
            uniform samplerCUBE _SkyboxNight;
            uniform float _TransitionDuration;
            uniform sampler2D _SunTex;

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float3 dayColor = texCUBE(_SkyboxDay, i.worldPos).rgb;
                float3 SunsetColor = texCUBE(_SunsetTex, i.worldPos).rgb;
                float3 nightColor = texCUBE(_SkyboxNight, i.worldPos).rgb * 2.5f;

                float3 darkBlue = float3(0.0, 0.0, 0.2);
                float3 nigthColor2 = lerp(darkBlue, nightColor, 0.93);

                float3 color = lerp(dayColor, SunsetColor, 1-_TransitionDuration);
                color = lerp(color, nigthColor2, 1-_TransitionDuration);

                return fixed4(color, 1.0);
            }
            ENDCG
        }
        }
            FallBack "Skybox/Cubemap"
}
