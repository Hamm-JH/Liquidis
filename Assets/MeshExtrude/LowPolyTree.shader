    Shader "Custom/LowPolyTree" {
        Properties {
            _NoiseTex ("Noise Texture", 2D) = "white" {}
            _Color ("Color", Color) = (1,1,1,1)
            _Amount ("Amount", Range(-1, 1)) = 0
            _Glossiness ("Smoothness", Range(0,1)) = 0.5
            _Metallic ("Metallic", Range(0,1)) = 0.0
        }
        SubShader {
            Tags { "RenderType"="Opaque" }
            LOD 200
     
            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows vertex:vert
     
            #pragma target 3.0
     
            struct Input {
                float2 uv_NoiseTex;
                float2 uv_ProceduralTex;
                float3 worldPos;
                float4 COLOR;
            };
     
            sampler2D _NoiseTex;
     
            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            float _Amount;
     
            void vert (inout appdata_full v) {
                if(v.color.r > 0.8) {
//                    float tex = tex2Dlod (_NoiseTex, float4(v.texcoord.xy, 0, 0)).r;
					float2 offset = v.vertex.xy;
                    float tex = tex2Dlod (_NoiseTex, float4(v.texcoord.xy+offset, 0, 0)).r;
                    v.vertex.y += tex.x * _Amount*10;
                }        
              }
     
            void su rf (Input IN, inout SurfaceOutputStandard o) {
                o.Albedo = _Color.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = _Color.a;
            }
            ENDCG
        }
        FallBack "Diffuse"
    }
     
