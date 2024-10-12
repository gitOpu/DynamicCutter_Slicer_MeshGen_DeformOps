Shader "Unlit/GlowShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent"
            "Queue" ="Transparent" 
        }
        LOD 100

        Pass
        {
            
          Cull Off //Cull Front Cull Back
           ZWrite Off //Default: On, Stop rendering behind this object
           Blend One One // (shader output * 1.0) + (frame buffer * 1.0).
           ZTest LEqual //Default LEqual, Always, GEqual Only render a pixel if its depth is less than or equal to the current depth in the ZBuffer
           
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
            float4 _MainColor;
            float4 _MainTex_ST;
            
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return  col * _MainColor;
            }
            ENDCG
        }
    }
}
