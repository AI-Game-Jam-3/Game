Shader "Unlit/LightingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5//设置的控制透明部分的系数
        _Color("MainColor", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="AlphaTest"  "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.x *= _Color.x;
                col.y *= _Color.y;
                col.z *= _Color.z;
                col.a *= _Color.a;
                // apply fog
                //half4 col = half4(col.x, col.y, col.z, _Cutoff);
                return col;
            }
            ENDCG
        }
    }
}
