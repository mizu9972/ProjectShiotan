Shader "Custom/VertexWave"
{
	Properties
	{
		_Frequency("Frequency ", Range(0, 3)) = 1
		_Amplitude("Amplitude", Range(0, 1)) = 0.5
		_Color("Color",Color) = (1,1,1,1)
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _Frequency;
			float _Amplitude;
			fixed4 _Color;

			//頂点シェーダ
			v2f vert(appdata v)
			{
				v2f o;

				// _Frequencyが大きいほど細かい波になる（ポリゴン数が足りてないとおかしくなるけど）
				float offsetY = sin(v.vertex.x * _Frequency) + sin(v.vertex.z * _Frequency);
				// 振幅の値を乗算する
				offsetY *= _Amplitude;
				v.vertex.y += offsetY;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			//フラグメントシェーダー
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Color;
				return col;
			}
			ENDCG
		}
	}
}
