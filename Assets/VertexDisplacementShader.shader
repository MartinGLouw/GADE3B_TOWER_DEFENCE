Shader "Custom/VertexDisplacementShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Float) = 1.0
        _Frequency ("Frequency", Float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Frequency;

            v2f vert (appdata_t v) {
                v2f o;
                float wave = sin(v.vertex.y * _Frequency) * _Amplitude;
                v.vertex.xyz += float3(0, wave, 0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
