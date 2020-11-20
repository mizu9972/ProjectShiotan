Shader "Custom/BasicMaterialNormalMap"
{
	Properties
	{
		_MainTex("Main Texture (RGB)", 2D) = "white" {}
		_Color("Diffuse Tint (RGB)", Color) = (1.0, 1.0, 1.0, 1.0)
		_Metalness("Metalness", Range(0.0, 1.0)) = 0.0
		_IndirectDiffRefl("Indirect Diffuse Reflection", Range(0.0, 1.0)) = 1.0
		_Roughness("Roughness", Range(0.0, 1.0)) = 0.0
		[Normal] _NormalMap("Normal map", 2D) = "bump" {}
		_Shininess("Shininess", Range(0.0, 1.0)) = 0.078125
	}
		SubShader
		{
			Pass
			{
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					half2 uv : TEXCOORD0;
					half3 normal : NORMAL;
					half4 tangent : TANGENT;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
					half3 lightDir : TEXCOORD1;
					half3 viewDir : TEXCOORD2;
					half3 normal : TEXCOORD3;
					half3 tangent : TEXCOORD4;
					half3 binormal : TEXCOORD5;
				};

				#define F0                            0.04f
				#define INDIRECT_DIFF_TEX_MIP        8.5f
				#define MAX_SHININESS                50.0f
				#define SHININESS_POW                0.2f
				#define DIRECT_SPEC_ATTEN_MIN        0.2f
				#define MAX_MIP                        8.0f

				sampler2D _MainTex;
				half4 _MainTex_ST;
				  half4 _Color;
				half4 _LightColor0;
				  half _Metalness;
				  half _IndirectDiffRefl;
				  half _Roughness;
				sampler2D _NormalMap;
				half _Shininess;

				v2f vert(appdata v)
				{
					v2f o = (v2f)0;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					// ワールド空間のライト方向と視点方向を求める
					o.lightDir = normalize(mul(unity_ObjectToWorld, ObjSpaceLightDir(v.vertex)));
					o.viewDir = normalize(mul(unity_ObjectToWorld, ObjSpaceViewDir(v.vertex)));

					// ワールド <-> 接空間変換行列を作成するため、ワールド空間のnormal, tangent, binormalを求めておく
					o.binormal = normalize(cross(v.normal, v.tangent) * v.tangent.w);
					o.normal = UnityObjectToWorldNormal(v.normal);
					o.tangent = mul(unity_ObjectToWorld, v.tangent.xyz);
					o.binormal = mul(unity_ObjectToWorld, o.binormal);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// 接空間 -> ワールド空間変換行列
					half3x3 tangentToWorld = transpose(half3x3(i.tangent.xyz, i.binormal, i.normal));

					half4 base = tex2D(_MainTex, i.uv) * _Color;
					half3 specColor = lerp(1.0, base.rgb, _Metalness);

					// ノーマルマップから法線情報を取得する
					half3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
					normal = mul(tangentToWorld, normal);

					half3 directDiff = base * (max(0.0, dot(normal, i.lightDir)) * _LightColor0.rgb);

					half3 halfDir = normalize(i.viewDir + i.lightDir);
					half shininess = lerp(MAX_SHININESS, 1.0, pow(_Roughness, SHININESS_POW));
					half directSpecAtten = lerp(DIRECT_SPEC_ATTEN_MIN, 1.0, shininess / MAX_SHININESS);
					half3 directSpec = pow(max(0.0, dot(normal, halfDir)), shininess) * _LightColor0.rgb * specColor * directSpecAtten;

					half fresnel = F0 + (1 - F0) * pow(1 - dot(i.viewDir, normal), 5);
					half indirectRefl = lerp(fresnel, 1, _Metalness);
					half3 indirectDiff = base * UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, normal, INDIRECT_DIFF_TEX_MIP) * _IndirectDiffRefl;

					half3 reflDir = reflect(-i.viewDir, normal);
					half3 indirectSpec = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflDir, _Roughness * MAX_MIP) * specColor;

					fixed4 col;
					col.rgb = lerp(directDiff, directSpec, _Metalness) + lerp(indirectDiff, indirectSpec, indirectRefl);
					return col;
				}
				ENDCG
			}
		}
}