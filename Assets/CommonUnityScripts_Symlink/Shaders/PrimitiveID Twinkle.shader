Shader "Custom/PrimitiveID Twinkle"
{
//https://forum.unity.com/threads/per-triangle-color-based-on-random-seed.498748/
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,1,1,1)
        _Rate ("Twinkle Rate", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
           
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
 
            // "borrowed" from a shader toy implementation
            fixed hash11(float p)
            {
                float3 p3 = frac((float3)p * 0.1031);
                p3 += dot(p3, p3.yzx + 19.19);
                return frac((p3.x + p3.y) * p3.z);
            }
 
            fixed4 _ColorA, _ColorB;
            float _Rate;
           
            fixed4 frag (uint primID : SV_PrimitiveID) : SV_Target
            {
                fixed rand = hash11((float)primID);
                // sine ( time + rand * tau ) because tau, or 2 pi, is one full period
                return lerp(_ColorA, _ColorB, sin(_Time.y * _Rate + rand * 6.2831) * 0.5 + 0.5);
            }
            ENDCG
        }
    }
}