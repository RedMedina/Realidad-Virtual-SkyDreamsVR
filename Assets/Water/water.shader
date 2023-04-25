Shader "Custom/water"
{
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _NormalMap1("Normal Map 1", 2D) = "bump" {}
        _NormalMap2("Normal Map 2", 2D) = "bump" {}
        _WaveSpeed("Wave Speed", Range(0, 10)) = 1
        _WaveScale("Wave Scale", Range(0, 1)) = 0.1
        _Shadows("Shadows", Range(0, 1)) = 1
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard

            sampler2D _MainTex;
            sampler2D _NormalMap1;
            sampler2D _NormalMap2;
            float _WaveSpeed;
            float _WaveScale;
            float _Shadows;

            struct Input {
                float2 uv_MainTex;
                float2 uv_NormalMap1;
                float2 uv_NormalMap2;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                // Calculate the normal by combining the two normal maps and adding some waves
                float bumpPower = 200.0f;
                float2 uv1 = IN.uv_NormalMap1 * _WaveScale + _Time.y * _WaveSpeed;
                float2 uv2 = IN.uv_NormalMap2 * _WaveScale + _Time.y * _WaveSpeed;
                //float2 uv1 = IN.uv_NormalMap1;
                //uv1.x += _WaveScale * sin(_WaveSpeed * IN.uv_NormalMap1.y * (_Time.y) / 5);
                //float2 uv2 = IN.uv_NormalMap2;
                //uv2.y += _WaveScale * sin(_WaveSpeed * IN.uv_NormalMap1.x * (_Time.y) / 5);
                //float3 normal = UnpackNormal(1-tex2D(_NormalMap1, uv1)) + UnpackNormal(tex2D(_NormalMap2, uv2));
                float3 normal = lerp(UnpackNormal(1-tex2D(_NormalMap1, uv1)) * 2.0 - 1.0, UnpackNormal(tex2D(_NormalMap2, uv2)) * 2.0 - 1.0, 0.5f);
                normal += float3(sin(uv1.x * 10) * 0.01, sin(uv2.y * 10) * 0.01, 0);

                float3 lightDir = normalize(float3(0.5f, 1.0f, 0.3f));
                float ambient = 0.2f;
                float diffuse = max(dot(normalize(normal * bumpPower), lightDir), 0.0);
                float3 specular = float3(0.35f, 0.35f, 0.35f);

                o.Normal = float3(ambient + diffuse, ambient + diffuse, ambient + diffuse) + specular;

                // Set the diffuse color
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
                o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

                // Add shadows by inverting one of the normal maps and multiplying it by the shadow value
                float3 shadowNormal = UnpackNormal(1-tex2D(_NormalMap2, uv2));
                if (_Shadows > 0) {
                    shadowNormal = 1 - shadowNormal;
                }
                float shadow = pow(dot(normalize(shadowNormal), float3(0, 0, 1)), 5);
                o.Emission = shadow * _Shadows;
            }
            ENDCG
        }

            FallBack "Diffuse"
}

