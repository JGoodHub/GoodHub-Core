Shader "Unlit/RingShader"
{
    Properties
    {
        _Color("Color", color) = (1, 1, 1, 1)
        _InnerRadius("Inner Radius", Range(0, 0.5)) = 1
        _Thickness("Thickness", Range(0, 0.5)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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

            float4 _Color;
            float  _InnerRadius;
            float  _Thickness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float centreDistance = length(float2(0.5, 0.5) - i.uv);

                if(centreDistance <= _InnerRadius || centreDistance >= _InnerRadius + _Thickness)
                {
                    return fixed4(0, 0, 0, 0);
                }

                return _Color;
            }
            ENDCG
        }
    }
}