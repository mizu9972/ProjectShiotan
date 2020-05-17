Shader "Unlit/ReflectionSeaPlane"
{
	Properties
	{
		//インスペクターに表示する数値
		_DistortionTex("DistortionTex",2D) = "grey"{}
		_DistortionPower("DistortionPower",Float) = 1
		_DistortionRate("DistortionColorRate",Range(0,5)) = 1.0

		_Color("Color", Color) = (0,0,0,1)
		_ColorbyWaterDepth("水深による暗くなる度合い",Float) = 0.003
		_MinColorbyWaterDepth("暗くなる度合いの最低値",Float) = 180
		_MinDepthColor("減色の最低値",Float) = 0.0
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Alpha("透明度",Range(0.0,1.0)) = 1.0
		_MergeTex("MergeTex",2D) = "black" {}
		_FloorTex("FloorTex",2D) = "black" {}
	}
		SubShader
		{
			//RenderTypeは現状自由
			//Queueは BackGroundより後 Geometryより前
			Tags { "RenderType" = "fade" "Queue" = "Geometry-1" }
			LOD 100

			GrabPass{ "_GrabTex" }//背景テクスチャ使用宣言

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
					half4 scrPos :TEXCOORD5;
				};

				//テクスチャ
				sampler2D _MainTex;//波情報
				sampler2D _GrabTex;//描画済みの背景
				sampler2D _MergeTex;//合成テクスチャ
				sampler2D _FloorTex;//ステージ範囲情報
				sampler2D _TideTex;//潮
				sampler2D _DistortionTex;//屈折マスク
				sampler2D _CameraDepthTexture;//デプスマップ

				//テクスチャそれぞれのTiling/offset情報
				float4 _MainTex_ST;
				float4 _MergeTex_ST;
				float4 _FloorTex_ST;
				float4 _TideTex_ST;
				half4 _DistortionTex_ST;
				float4 _GrabTex_ST;
				float4 _CameraDepthTexture_ST;

				//色
				fixed4 _Color;
				float _Alpha;

				//深度による色の変化の度合い
				float _ColorbyWaterDepth;
				float _MinColorbyWaterDepth;
				float _MinDepthColor;

				float4 _CameraDepthTexture_TexelSize;//デプスマップのテクセルのサイズ
				float2 _GrabTex_Texel;//背景テクスチャのテクセル情報

				half _DistortionPower;//歪みの強さ
				float _DistortionRate;//歪みの比率

				//ドットの情報落ちを補正する関数
				//2020/05/12 あまり意味なかった
				float2 AlignWithGrabTexel(float2 uv) {
					return (floor(uv * _CameraDepthTexture_TexelSize.zw) + 0.5) * abs(_CameraDepthTexture_TexelSize.xy);
				}

				//頂点シェーダー
				v2f vert(appdata v)
				{
					//各情報取り出し＆フラグメントシェーダーへ流す
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.uv2 = TRANSFORM_TEX(v.uv2, _MergeTex);
					o.uv3 = TRANSFORM_TEX(v.uv3, _FloorTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					o.grabPos = ComputeGrabScreenPos(o.vertex);//grabテクスチャ取得
					o.scrPos = ComputeScreenPos(o.vertex);//デプスマップ取得
					return o;
				}

				//フラグメントシェーダー
				fixed4 frag(v2f i) : SV_Target
				{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);//波の高さ
				col.x -= 0.5f;//-0.5~0.5に補正

				fixed4 MainCol = tex2D(_MergeTex, i.uv2);//テクスチャ
				fixed4 FloorMaskCol = 1.0f - tex2D(_FloorTex, i.uv3);//ステージ地面マスク

				col.x *= -1.0f;//波を白色で表現

				//屈折処理==========================================================
				half2 Distortion = col.x * _DistortionPower;//波の高さを設定

				//屈折後の描画色を取得
				float4 DepthUV = i.grabPos;
				DepthUV.xy = (i.grabPos.xy) + Distortion;

				float surfDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(i.scrPos.z);//自身の深度値を取得
				float refFix = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(DepthUV))));//描画するピクセルの深度値を取得
				float subDepth = refFix - surfDepth;//深度差計算
				float depthDiff = saturate(subDepth);//深度差を0~1の範囲で設定(マイナスの値は0になる)

				//Distortion = Distortion * max(0, subDepth);
				Distortion = Distortion * depthDiff;//深度差に応じて屈折を戻す
				//=================================================================

				//描画
				float2 GrabUv = i.grabPos.xy;
				float GrabDistortion = col.x * _DistortionPower;
				GrabUv = AlignWithGrabTexel((GrabUv + GrabDistortion * depthDiff) / i.grabPos.w);
				fixed4 outCol = tex2D(_GrabTex, GrabUv);

				float ColorRate = ((max(col.x, 0) * 2.0f) * _DistortionRate);//波の高さの値
				outCol += _Color * ColorRate;//色加算
				outCol += tex2D(_MergeTex, i.uv) * ColorRate;//画像加算
				outCol -= abs(subDepth / i.scrPos.w - 1.0f) * _ColorbyWaterDepth / 10.0f + _MinColorbyWaterDepth;//深度差に応じて暗くする
				outCol.w = FloorMaskCol.x * FloorMaskCol.y * FloorMaskCol.z * _Alpha;//エリア外描画しないように
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, outCol);
				return outCol;
				}

			ENDCG
		}
	}
}