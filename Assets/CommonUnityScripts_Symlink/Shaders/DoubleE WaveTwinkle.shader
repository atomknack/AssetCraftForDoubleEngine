Shader "D/WaveTwinkle"
{
//Pavel, based on: https://forum.unity.com/threads/per-triangle-color-based-on-random-seed.498748/
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,1,1,1)
        _Rate ("Twinkle Rate", Float) = 1
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
 
            fixed hashU01(uint seed)
            {
                float pre =seed*11+seed*7+seed*3;
                pre +=pre*9.7011;
                return frac(pre*0.02371 + pre*0.0171);
            }
 
            fixed4 _ColorA, _ColorB;
            float _Rate;
           
            fixed4 frag (uint primID : SV_PrimitiveID) : SV_Target
            {
                fixed rand = hashU01(primID);
                return lerp(_ColorA, _ColorB, sin(_Time.y * _Rate + rand * 5.2171) * 0.5 + 0.5);
            }
            ENDCG
        }
    }
}