Shader "Custom/SolidColorSprite"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata_t { float4 vertex : POSITION; float2 texcoord : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; float2 texcoord : TEXCOORD0; };
            sampler2D _MainTex;
            float4 _Color;
            v2f vert (appdata_t v) { v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.texcoord = v.texcoord; return o; }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.texcoord);
                // 텍스처 알파만 사용, 색상은 _Color로 덮어쓰기
                return fixed4(_Color.rgb, tex.a * _Color.a);
            }
            ENDCG
        }
    }
}
