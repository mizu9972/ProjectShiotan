Shader "Custom/SeaPlane"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_MainTex("MainTex",2D) = "white" {}
	}
		SubShader
	{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Pass
		{
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types

			#pragma vertex vert
			#pragma fragment frag
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			#include "UnityCG.cginc"
			sampler2D _MainTex;

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

			float4 _MainTex_ST;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			v2f vert(appdata IN)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(IN.vertex);
				o.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				return o;
			}

			//フラグメントシェーダー
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv);//色
			//col = float4(0.0f, 1.0f, 1.0f, 1);
			col = fixed4(0, col.x, col.x, 1.0);

			return col;
			}

			ENDCG
		}	
	}
    FallBack "Diffuse"
}
