Shader "Hidden/BSBBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
           

  

            float4 blur(sampler2D tex, float2 uv, int iter, float intensity){

             float pi = 6.28318530718;
             float Directions = 90.0;
             float Quality = iter;
             float4 c =0;
             for(float d=0; d<pi; d+=pi/Directions){

                 for(float i=1.0f/Quality;i<=1.0;i+=1.0f/Quality){

                     c+=tex2D(tex,uv+float2(cos(d),sin(d))*intensity*i);

                     }

                 }
             
             return c/(Quality*Directions);
            }

float _Blur;
int _Iter;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = blur(_MainTex,i.uv,_Iter,_Blur/8.0f);
                return col;
            }
            ENDCG
        }
    }
}
