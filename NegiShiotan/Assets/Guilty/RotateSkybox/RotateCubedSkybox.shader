// Copyright (c) 2020 Guilty
// License : MIT License
// GitHub : https://github.com/GuiltyWorks
// Twitter : https://twitter.com/GuiltyWorks
// Gmail : guilty0546@gmail.com

Shader "Guilty/RotateCubedSkybox" {
    Properties {
        _XRotationSpeed ("X-Axis Rotation Speed", Range(-100, 100)) = 5.0
        _XDefaultDegree ("X-Axis Default Degree", Range(0, 360)) = 0.0
        _YRotationSpeed ("Y-Axis Rotation Speed", Range(-100, 100)) = 0.0
        _YDefaultDegree ("Y-Axis Default Degree", Range(0, 360)) = 0.0
        _ZRotationSpeed ("Z-Axis Rotation Speed", Range(-100, 100)) = 0.0
        _ZDefaultDegree ("Z-Axis Default Degree", Range(0, 360)) = 0.0
        _Tint ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        [NoScaleOffset] _Tex ("Cubemap   (HDR)", Cube) = "grey" {}
    }

    SubShader {
        Tags {
            "Queue" = "Background"
            "RenderType" = "Background"
            "PreviewType" = "Skybox"
        }
        Cull Off ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            float _XRotationSpeed, _XDefaultDegree, _YRotationSpeed, _YDefaultDegree, _ZRotationSpeed, _ZDefaultDegree;
            samplerCUBE _Tex;
            half4 _Tex_HDR;
            half4 _Tint;
            half _Exposure;

            struct appdata_t {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata_t v) {
                float3 unitY = float3(0.0, 1.0, 0.0);
                float3 unitZ = float3(0.0, 0.0, 1.0);

                float sina, cosa;
                sincos(((_Time.y * (2.0 * (step(0.0, _XRotationSpeed) - 0.5)) * (1.0 - cos(_XRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _XDefaultDegree) * UNITY_PI / 180.0, sina, cosa);
                float2x2 rotation = float2x2(cosa, -sina, sina, cosa);

                v.vertex.zy = mul(rotation, v.vertex.zy);
                unitY.zy = mul(rotation, unitY.zy);
                unitZ.zy = mul(rotation, unitZ.zy);

                float3x3 rodrigues = float3x3(cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0) + pow(unitY.x, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)),
                                              unitY.x * unitY.y * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) - unitY.z * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              unitY.x * unitY.z * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) + unitY.y * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              unitY.y * unitY.x * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) + unitY.z * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0) + pow(unitY.y, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)),
                                              unitY.y * unitY.z * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) - unitY.x * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              unitY.z * unitY.x * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) - unitY.y * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              unitY.z * unitY.y * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)) + unitY.x * sin(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0),
                                              cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0) + pow(unitY.z, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _YRotationSpeed) - 0.5)) * (1.0 - cos(_YRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _YDefaultDegree) * UNITY_PI / 180.0)));

                v.vertex.xyz = mul(rodrigues, v.vertex.xyz);
                unitZ = mul(rodrigues, unitZ);

                rodrigues = float3x3(cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0) + pow(unitZ.x, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)),
                                     unitZ.x * unitZ.y * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) - unitZ.z * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     unitZ.x * unitZ.z * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) + unitZ.y * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     unitZ.y * unitZ.x * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) + unitZ.z * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0) + pow(unitZ.y, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)),
                                     unitZ.y * unitZ.z * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) - unitZ.x * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     unitZ.z * unitZ.x * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) - unitZ.y * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     unitZ.z * unitZ.y * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)) + unitZ.x * sin(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0),
                                     cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0) + pow(unitZ.z, 2.0) * (1.0 - cos(((_Time.y * (2.0 * (step(0.0, _ZRotationSpeed) - 0.5)) * (1.0 - cos(_ZRotationSpeed / 100.0 * UNITY_PI / 2.0)) * 360.0) + _ZDefaultDegree) * UNITY_PI / 180.0)));

                v.vertex.xyz = mul(rodrigues, v.vertex.xyz);

                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                half4 tex = texCUBE(_Tex, i.texcoord);
                half3 c = DecodeHDR(tex, _Tex_HDR);
                c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
                c *= _Exposure;
                return half4(c, 1);
            }
  
            ENDCG
        }
    }

    Fallback Off
}
