//テクスチャ合成用シェーダ
//参考 http://karanokan.info/2018/10/08/post-1155/

Shader "Unlit/TextureAdd"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_AddTex("BlushTexture",2D) = "white"{}
		_UVPosition("UV Position", VECTOR) = (0.5, 0.5, 0, 0)
		_SizeX("PaintSizeX",Float) = 0.1
		_SizeY("PaintSizeY",Float) = 0.1
		_RamdomFlag("RamdomFlag",Float) = 0.0
		_RandomRange("RandomRange",Range(0.0,1.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
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
			sampler2D _AddTex;
            float4 _MainTex_ST;
			float4 _AddTex_ST;
			float4 _UVPosition;
			float _SizeX;
			float _SizeY;
			float _RamdomFlag;//波の発生位置を若干ランダムにするか(true = 1,false = 0)
			float _RandomRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			//activeFlagが１ならランダムな値
			//			  0なら必ず１を返す
			float rand(float2 co,float activeFlag) {
				float retValue = frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);//-1.0 ~ 1.0
				retValue += 1.0;//0.0 ~ 2.0
				retValue = 1.0 - retValue * activeFlag;//-1.0 ~ 1.0 (ramdomFlag反映)
				retValue = (retValue + 1.0) * 0.5 * _RandomRange;//0.0 ~ RandomRange
				retValue -= _RandomRange / 2.0;//-RandomRange/2 ~ RandomRange/2
				return retValue;
			}

            fixed4 frag (v2f i) : SV_Target
			{
				//画像を合成する

				//画像のテクセルが合成先の画像の範囲内にあるか判定
				float com;
				
				com = step(_UVPosition.x - _SizeX, i.uv.x);
				com = step(i.uv.x, _UVPosition.x + _SizeX) * com;
				com = step(_UVPosition.y - _SizeY, i.uv.y) * com;
				com = step(i.uv.y, _UVPosition.y + _SizeY)* com;

				float2 size = { _SizeX ,_SizeY };
				fixed4 col1 = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_AddTex, ((i.uv - (_UVPosition.xy - size.xy))/* * rand(_Time * i.uv, _RamdomFlag)*/ * 0.5 / size.xy ));

				col2.a = col2.a * com;

				fixed alpha = col2.a + (1 - col2.a) * col1.a;
				fixed4 col = fixed4((col2.rgb * col2.a + (col1.rgb * col1.a * (1 - col2.a))) / alpha , alpha);
				return col;
			}

            ENDCG
        }
    }
}
