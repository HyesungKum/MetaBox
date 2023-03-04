Shader "Custom/SpriteStencilShader" {
    Properties {
        _Color("Color", Color) = (1,1,1,1)
        _StencilRef("Stencil Reference", Range(0, 255)) = 1
        _StencilComp("Stencil Comparison", Range(0, 8)) = 7
        _StencilPass("Stencil Pass", Range(0, 3)) = 0
        _MainTex("Albedo", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Pass {
            Stencil {
                Ref [_StencilRef]
                Comp [_StencilComp]
                Pass [_StencilPass]
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                clip(tex2D(_MainTex, i.uv).a - 0.5);
                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}