Shader "Custom/RoundedEdgeStandard"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Roundness ("Roundness", Range(0,0.5)) = 0.15 // 둥글림 정도
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        float _Glossiness;
        float _Metallic;
        float _Roundness;

        float3 _ObjMin;
        float3 _ObjMax;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // 텍스처 알베도와 노멀맵 불러오기
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // 1. 위치 정규화 (0~1 구간)
            float3 normedPos = (IN.worldPos - _ObjMin) / (_ObjMax - _ObjMin);

            // 2. 각면까지 거리 계산
            float3 dists = normedPos;
            float3 invs = 1 - normedPos;
            float edgeDist = min(min(min(dists.x, dists.y), dists.z), min(min(invs.x, invs.y), invs.z));

            // 3. 둥글림 강도 계산 (부드럽게 변화)
            float round_strength = saturate(edgeDist / _Roundness);
            round_strength = pow(round_strength, 2.0); // 강도 조절

            // 4. 오브젝트 중심점 계산
            float3 center = (_ObjMin + _ObjMax) * 0.5;

            // 5. 오브젝트 중심에서 픽셀 방향 법선 계산
            float3 dirFromCenter = normalize(IN.worldPos - center);

            // 6. 원래 법선과 보정법선 사이 보간으로 둥글림 효과
            o.Normal = lerp(dirFromCenter, o.Normal, round_strength);
        }
        ENDCG
    }
    FallBack "Standard"
}
