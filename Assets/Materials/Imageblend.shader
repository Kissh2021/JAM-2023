Shader "Hidden/Imageblend"
{
    Properties
    {
        _FirstTex("CurrentSeason", 2D) = "white" {}
        _SecondTex("NextSeason", 2D) = "white" {}
        _Range("Range", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
                o.uv = v.uv;
                return o;
            }

            sampler2D _FirstTex;
            sampler2D _SecondTex;

            float _Range;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_FirstTex, i.uv) * (1.0f - _Range) + tex2D(_SecondTex, i.uv) * _Range;
                // just invert the colors
                return col;
            }
            ENDCG
        }
    }
}
