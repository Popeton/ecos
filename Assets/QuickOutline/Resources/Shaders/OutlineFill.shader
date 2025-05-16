Shader "Custom/OutlineFillSoft" 
{
    Properties 
    {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 10)) = 2
        _Softness("Softness", Range(0, 1)) = 0.5
        _NoiseScale("Noise Intensity", Range(0, 0.2)) = 0.1
        _NoiseSpeed("Noise Speed", Range(0, 2)) = 0.5
    }

    SubShader 
    {
        Tags {
            "Queue" = "Transparent+110"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
        }

        Pass 
        {
            Name "Fill"
            Cull Off
            ZTest [_ZTest]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            Stencil {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f 
            {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform fixed4 _OutlineColor;
            uniform float _OutlineWidth;
            uniform float _Softness;
            uniform float _NoiseScale;
            uniform float _NoiseSpeed;

            v2f vert(appdata input) 
            {
                v2f output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Generar ruido orgánico
                float noise = sin(_Time.y * _NoiseSpeed + input.vertex.x * 100) * _NoiseScale;
                
                float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal;
                float3 viewPosition = UnityObjectToViewPos(input.vertex);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

                // Aplicar efecto de ruido al ancho
                float adjustedWidth = _OutlineWidth * (1.0 + noise);
                output.position = UnityViewToClipPos(viewPosition + viewNormal * -viewPosition.z * adjustedWidth / 800.0);

                // Calcular dirección de visión y normal mundial
                float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
                output.worldNormal = UnityObjectToWorldNormal(normal);
                output.viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                output.color = _OutlineColor;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target 
            {
                // Efecto Fresnel para difuminado progresivo
                float fresnel = 1.0 - saturate(dot(input.viewDir, input.worldNormal));
                float alpha = smoothstep(0.0, _Softness, fresnel);
                
                fixed4 color = input.color;
                color.a *= alpha; // Aplicar transparencia dinámica
                return color;
            }
            ENDCG
        }
    }
}