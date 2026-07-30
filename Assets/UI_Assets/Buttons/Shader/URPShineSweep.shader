Shader "UI/URPShineSweep"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _Width ("Shine Width", Range(0.01,0.5)) = 0.2
        _Speed ("Sweep Duration", Float) = 1.0
        _Delay ("Delay Between Sweeps", Float) = 0.5
        _Angle ("Angle", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _ShineColor;
            float _Width;
            float _Speed;
            float _Delay;
            float _Angle;
            float _UnscaledTime;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * IN.color;

                float diag = uv.x + uv.y * _Angle;
                float maxDiag = 1.0 + _Angle;

                // Tiempo total del ciclo
                float cycle = _Speed + _Delay;

                // Tiempo dentro del ciclo
                float phase = fmod(_UnscaledTime, cycle);

                float shine = 0;

                // Solo dibujar el brillo durante la fase de barrido
                if (phase < _Speed)
                {
                    float t = lerp(-_Width, maxDiag + _Width, phase / _Speed);

                    shine =
                        smoothstep(t - _Width, t, diag) -
                        smoothstep(t, t + _Width, diag);
                }

                col.rgb += _ShineColor.rgb * shine;

                return col;
            }

            ENDHLSL
        }
    }
}