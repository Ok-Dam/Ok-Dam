Shader "Custom/OverlayHeat"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}   // 기본 바닥 텍스처
        _HeatTex ("Heat Overlay", 2D) = "white" {} // 오버레이 (히트맵) 텍스처
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert         // 정점 셰이더 함수 이름
            #pragma fragment frag      // 픽셀 셰이더 함수 이름

            #include "UnityCG.cginc"   // 유니티 내장 셰이더 유틸리티 포함

            sampler2D _MainTex;        // 기본 텍스처 샘플러
            sampler2D _HeatTex;        // 히트맵(오버레이) 텍스처 샘플러
            float4 _HeatColor;         // 오버레이 색상 (Material에서 설정 가능, 여기선 빨강 반투명)
            float4 _MainTex_ST;        // 기본 텍스처 UV 스케일/오프셋 (자동 전달됨)

            // 정점 셰이더 입력 데이터 구조체
            struct appdata
            {
                float4 vertex : POSITION;    // 정점 위치 (모델 좌표계)
                float2 uv : TEXCOORD0;       // 텍스처 좌표
            };

            // 정점 셰이더 출력 데이터 구조체 (픽셀 셰이더 입력 역할)
            struct v2f
            {
                float2 uv : TEXCOORD0;       // 텍스처 좌표 전달
                float4 vertex : SV_POSITION; // 스크린 공간 위치
            };

            // 정점 셰이더: 모델 좌표를 클립 좌표로 변환, UV 변환 적용
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);              // 모델 좌표->클립 좌표 변환
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);                   // 텍스처 UV에 스케일/오프셋 반영
                return o;
            }

            // 픽셀 셰이더: 실제 픽셀별 색 결정
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);                // 기본 텍스처 색 샘플링
                fixed4 heatTexCol = tex2D(_HeatTex, i.uv);             // 히트맵 텍스처 색 샘플링
                fixed4 heatCol = heatTexCol * _HeatColor;              // 색상 보정 (여기선 오버레이 색과 곱함)

                // 히트맵 부분 투명하면 기본 텍스처 색 그대로 출력
                if (heatTexCol.a < 0.01)
                {
                    return baseCol;
                }
                else
                {
                    // 히트맵 투명도(알파)로 두 색을 혼합해서 출력
                    fixed4 result = lerp(baseCol, heatCol, heatCol.a);
                    return result;
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"   // 쉐이더가 지원 안 되면 디폴트 Diffuse 사용
}
