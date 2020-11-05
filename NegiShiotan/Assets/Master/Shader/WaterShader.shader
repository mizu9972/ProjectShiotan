Shader "Custom/WaterShader"{
	Properties
	{
		_DistortionTex("Distortion Texture(RG)", 2D) = "grey" {}
		_Color("WaterColor", Color) = (0,0,0,0)
		_DistortionPower("Distortion Power", Range(0, 1)) = 0
		_ScrollSpeed("Scroll Speed", Range(0, 0.5)) = 0.01
		_Frequency("Frequency ", Range(0, 3)) = 1
		_Amplitude("Amplitude", Range(0, 5)) = 0.5
		_WaveSpeed("WaveSpeed",Range(0, 100)) = 10
	}

		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

			Cull Back
			ZWrite On
			ZTest LEqual
			ColorMask RGB

			GrabPass { "_GrabPassTexture" }

			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata {
					half4 vertex  : POSITION;
					half4 texcoord  : TEXCOORD0;
					half4 normal : NORMAL;
				};

				struct v2f {
					half4 vertex  : SV_POSITION;
					half2 uv  : TEXCOORD0;
					half4 grabPos : TEXCOORD1;
				};

				sampler2D _DistortionTex;
				half4 _DistortionTex_ST;
				sampler2D _GrabPassTexture;
				half4 _Color;
				half _DistortionPower;
				half _ScrollSpeed;
				float _Frequency;
				float _Amplitude;
				float _WaveSpeed;


				v2f vert(appdata v)
				{
					v2f o = (v2f)0;

					// 頂点を動かしている
					float2 factors = _Time.x * _WaveSpeed + v.vertex.xz * _Frequency;
					float2 offsetYFactors = (sin(factors) * _Amplitude);

					v.vertex.y += offsetYFactors.x + offsetYFactors.y;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _DistortionTex);
					o.grabPos = ComputeGrabScreenPos(o.vertex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// w除算
					half2 uv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);

					// Distortionの値に応じてサンプリングするUVをずらす
					half2 distortion = UnpackNormal(tex2D(_DistortionTex, i.uv + _Time * _ScrollSpeed)).rg;
					distortion *= _DistortionPower;
					uv += distortion;
					half4 refraction = tex2D(_GrabPassTexture, uv);
					refraction *= _Color;
					return refraction;
				}
				ENDCG
			}
		}
}