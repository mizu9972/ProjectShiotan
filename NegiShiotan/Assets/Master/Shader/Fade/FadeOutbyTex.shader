Shader "Unlit/FadeOutbyTex"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("トランジション画像",2D) = "white"{}
		_isActive("有効か",Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				//トランシジョンルール画像
				sampler2D _TransitionTex;
				float4 _TransitionTex_ST;

				uniform float _TimeCount;//経過時間
				uniform float _isActive;//有効かどうか

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				//トランシジョン画像の黒いピクセルから判定して黒色で画面を上塗りしていく
				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					//描画ピクセルの画像情報取り出し
					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 addcol = tex2D(_TransitionTex, i.uv);

					addcol = step(_TimeCount * 0.01, addcol * _isActive);//トランジション画像の色判定
					col = col * addcol;//画像上書き
					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
}
