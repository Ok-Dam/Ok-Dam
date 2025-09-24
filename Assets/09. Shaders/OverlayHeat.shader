Shader "Custom/OverlayHeat"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _HeatTex ("Heat Overlay", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _HeatTex;
            float4 _HeatColor;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 heatTexCol = tex2D(_HeatTex, i.uv);
                fixed4 heatCol = heatTexCol * _HeatColor;

                // If heat texture alpha near zero, output base color only
                if (heatTexCol.a < 0.01)
                {
                    return baseCol;
                }
                else
                {
                    fixed4 result = lerp(baseCol, heatCol, heatCol.a);
                    return result;
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
