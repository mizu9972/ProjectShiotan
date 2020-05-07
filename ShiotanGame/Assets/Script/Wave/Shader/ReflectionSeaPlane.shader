Shader "Unlit/ReflectionSeaPlane"
{
    Properties
    {
		_DistortionTex("DistortionTex",2D) = "grey"{}
		_DistortionPower("DistortionPower",Range(0,1)) = 1

		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Alpha("透明度",Range(0.0,1.0)) = 1.0
		_MergeTex("MergeTex",2D) = "white" {}
		_FloorTex("FloorTex",2D) = "black" {}
		_TideTex("TideTex",2D) = "black" {}
		_ScrollTime("ScrollTime", Float) = 1.0
		_LightingRate("輝度",Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="transparent" "Queue" = "Geometry-1" }
        LOD 100

		GrabPass{ "_GrabTex" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

				float2 uv2 : TEXCOORD1;
				float2 uv3 :TEXCOORD2;
				float2 Scrolluv :TEXCOORD3;

				half4 grabPos : TEXCOORD4;
            };

			sampler2D _MainTex;
			sampler2D _GrabTex;
			sampler2D _MergeTex;
			sampler2D _FloorTex;
			sampler2D _TideTex;

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
			float _LightingRate;

			sampler2D _DistortionTex;
			half4 _DistortionTex_ST;
			half _DistortionPower;

            v2f vert (appdata v)
            {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _MergeTex);
				o.uv3 = TRANSFORM_TEX(v.uv3, _FloorTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);//grabテクスチャ取得
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
            fixed4 col = tex2D(_MainTex, i.uv);//波の色
			col.x -= 0.5f;

			fixed4 MainCol = tex2D(_MergeTex, i.uv2);//テクスチャ
			fixed4 FloorMaskCol = 1.0f - tex2D(_FloorTex, i.uv3);

			fixed4 TideTexCol = tex2D(_TideTex, i.uv2 + _Time * _ScrollTime) * _TideLitRate;//潮テクスチャ uvスクロールさせる

			col.x *= -1.0f;//波を白色で表現

			//col = fixed4(MainCol.x + TideTexCol.x + col.x, MainCol.y + TideTexCol.y + col.x, MainCol.z + TideTexCol.z + col.x, MainCol.w + TideTexCol.w + col.x);
			col.w = col.w * FloorMaskCol.x * FloorMaskCol.y * FloorMaskCol.z * _Alpha;

			//屈折処理-----------
			half2 GrabUv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);

			half2 Distortion = tex2D(_DistortionTex, i.uv + _Time.x * 0.1f + col.x).rg - 0.5;
			Distortion *= _DistortionPower;

			GrabUv = GrabUv + Distortion;
			fixed4 outCol = tex2D(_GrabTex, GrabUv);
			//------------------

			outCol.w = FloorMaskCol.x * FloorMaskCol.y * FloorMaskCol.z * _Alpha;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				return outCol;
            }
            ENDCG
        }
    }
}
