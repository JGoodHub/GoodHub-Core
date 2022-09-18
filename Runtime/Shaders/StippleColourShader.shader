Shader "Ocias/Diffuse Colour (Stipple Transparency)"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _Transparency ("Transparency", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        
        LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        half4 _Color;
        half4 _EmissionColor;

        half _Transparency;

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color;
            o.Alpha = _Color[3];

            o.Emission = _Color * _EmissionColor;

            // Screen-door transparency: Discard pixel if below threshold.
            const float4x4 threshold_matrix =
            {
                1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
                13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
                4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
                16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
            };

            const float4x4 row_access =
            {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            };

            float2 pos = IN.screenPos.xy / IN.screenPos.w;
            pos *= _ScreenParams.xy; // pixel position
            clip(_Transparency - threshold_matrix[fmod(pos.x, 4)] * row_access[fmod(pos.y, 4)]);
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}