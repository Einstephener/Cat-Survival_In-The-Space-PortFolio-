Shader "Skybox/NightDayCubemap"
{
    Properties
    {
        _Cubemap1("Cubemap1", CUBE) = "white" {}
        _Cubemap2("Cubemap2", CUBE) = "white" {}
        _Blend("Blend", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            samplerCUBE _Cubemap1;
            samplerCUBE _Cubemap2;
            float _Blend;
 
            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                // 샘플링할 방향을 worldPos로부터 정규화하여 사용
                float3 dir = normalize(i.worldPos);

                // Cubemap 샘플링
                fixed4 cubemap1 = texCUBE(_Cubemap1, dir);
                fixed4 cubemap2 = texCUBE(_Cubemap2, dir);

                // 두 Cubemap을 섞음
                return lerp(cubemap1, cubemap2, _Blend);
            }
            ENDCG
        }
    }
}
