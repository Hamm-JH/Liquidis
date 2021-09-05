    Shader "Custom/LowPolyTree-anim" {
        Properties {
            _NoiseTex ("Noise Texture", 2D) = "white" {}
            _Color ("Color", Color) = (1,1,1,1)
            _Amount ("Amount", Range(-10, 10)) = 1
            _Speed ("Speed", Range(-10, 10)) = 1
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
            float _Speed;
     

//        UNITY_INSTANCING_CBUFFER_START(Props)
//        UNITY_INSTANCING_CBUFFER_END
		
            void vert (inout appdata_full v) {
                if(v.color.r > 0.8) 
				{
                    float4 noise = tex2Dlod (_NoiseTex, float4(v.vertex.x+v.texcoord.x+_Time.x*_Speed,v.vertex.z+v.texcoord.y, 0, 0));
                    v.vertex.y += noise.r * _Amount*10;
                }        
              }
     
            void surf (Input IN, inout SurfaceOutputStandard o) {
                o.Albedo = _Color.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = _Color.a;
            }
            ENDCG
        }
        FallBack "Diffuse"
    }
     
