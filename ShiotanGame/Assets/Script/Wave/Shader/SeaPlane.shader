Shader "Custom/SeaPlane"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Alpha("透明度",Range(0.0,1.0)) = 1.0
		_MergeTex("MergeTex",2D) = "white" {}
		_FloorTex("FloorTex",2D) = "black" {}
		_TideTex("TideTex",2D) = "black" {}
		_ScrollTime("ScrollTime", Float) = 1.0
	}
		SubShader
	{
			Tags { "RenderType" = "transparent" "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
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
			sampler2D _MergeTex;
			sampler2D _FloorTex;
			sampler2D _TideTex;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 :TEXCOORD2;
				float2 Scrolluv :TEXCOORD3;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 :TEXCOORD2;
				float2 Scrolluv :TEXCOORD3;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_ST;
			float4 _MergeTex_ST;
			float4 _FloorTex_ST;
			float4 _TideTex_ST;
			float _TideLitRate;
			float _ScrollTime;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float _Alpha;

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
				o.uv2 = TRANSFORM_TEX(IN.uv2, _MergeTex);
				o.uv3 = TRANSFORM_TEX(IN.uv3, _FloorTex);
				o.Scrolluv = TRANSFORM_TEX(IN.Scrolluv, _TideTex);
				return o;
			}

			//フラグメントシェーダー
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv);//波の色
				col.x -= 0.5f;

				fixed4 MainCol = tex2D(_MergeTex, i.uv2);//テクスチャ
				fixed4 FloorMaskCol = 1.0f - tex2D(_FloorTex, i.uv3);

				fixed4 TideTexCol = tex2D(_TideTex, i.uv2 + _Time * _ScrollTime) * _TideLitRate;//潮テクスチャ uvスクロールさせる
				//col.x = floor(col.x * 10) / 10.0f;//小数点第二以下を切り捨て
				col.x *= -1.0f;//波を白色で表現
				col = fixed4(MainCol.x + TideTexCol.x + col.x, MainCol.y + TideTexCol.y + col.x, MainCol.z + TideTexCol.z + col.x, MainCol.w + TideTexCol.w + col.x);
				col.w = col.w * FloorMaskCol.x * FloorMaskCol.y * FloorMaskCol.z * _Alpha;

				
				return col;
			}

			ENDCG
		}	
	}
    FallBack "Diffuse"
}
