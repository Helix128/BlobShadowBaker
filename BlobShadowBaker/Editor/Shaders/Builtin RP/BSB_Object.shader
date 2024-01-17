Shader "Hidden/BSBObject"
{
    Properties
    {
    }
    SubShader
    {
       Tags {"Queue"="Geometry" "IgnoreProjector"="False" "RenderType"="Opaque"}
        ZWrite On   

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(0,0,0,1);
            }
            ENDCG
        }
    }
}
