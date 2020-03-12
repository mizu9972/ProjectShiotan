Shader "Custom/VertexWave"
{
	Properties
	{
		_Frequency("Frequency ", Range(0, 3)) = 1
		_Amplitude("Amplitude", Range(0, 1)) = 0.5
		_Speed("WaveSpeed",float) = 1
		_Color("Color",Color) = (1,1,1,1)
		_Texture("Texture",2D) = "white" {}
		
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _Frequency;
			float _Amplitude;
			float _Speed;
			sampler2D _Texture;
			fixed4 _Color;
			
			//頂点シェーダ
			v2f vert(appdata v)
			{
				v2f o;

				//時間経過を反映
				float time = _Time * _Speed;
				float2 factors = time + v.vertex.xz * _Frequency;
				//波の調整 _Frequencyの値で波の高さが設定される
				float offsetY = sin(factors.x) * _Amplitude + sin(factors.y) * _Amplitude;
				//振幅の値を乗算する
				v.vertex.y += offsetY;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//法線を補正
				float2 normalOffsets = -cos(factors) * _Amplitude;
				v.normal.xyz = normalize(half3(normalOffsets.x, 1, normalOffsets.y));
				return o;
			}

			//フラグメントシェーダー
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Color;//色
				return col;
			}
			ENDCG
		}
	}
}
