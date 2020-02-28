Shader "Unlit/TextureAdd"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
			float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
			{
				//画像を合成する
				float com;
				com = step(_UVPosition.x - _Size, i.uv.x);
				com = step(i.uv.x, _UVPosition.x + _Size) * com;
				com = step(_UVPosition.y - _Size, i.uv.y) * com;
				com = step(i.uv.y, _UVPosition.y + _Size)* com;

				fixed4 col1 = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_AddTex, (i.uv - (_UVPosition - _Size)) * 0.5 / _Size);

				col2.a = col2.a * com;
				fixed alpha = col2.a + (1 - col2.a) * col1.a;
				fixed4 col = fixed4((col2.rgb * col2.a + (col1.rgb * col1.a * (1 - col2.a))) / alpha, alpha);
				return col;
			}
            ENDCG
        }
    }
}
