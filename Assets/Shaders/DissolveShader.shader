Shader "Custom/DissolveShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveColor ("Dissolve Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _DissolveTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _DissolveAmount;
        fixed4 _DissolveColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float dissolve = tex2D(_DissolveTex, IN.uv_MainTex).r;
            if (dissolve < _DissolveAmount)
            {
                discard;
            }
            if (dissolve < _DissolveAmount + 0.05)
            {
                c.rgb = _DissolveColor.rgb;
            }
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
