Shader "Unlit/FadebyTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TransitionTex("トランジション画像",2D) = "white"{}
		_MinColor("色のしきい地",Float) = 0
		_FadeSpeed("フェード速度",Float) = 1
		_isActive("有効か",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

			sampler2D _TransitionTex;
			float4 _TransitionTex_ST;

			sampler2D _InitStateTex;
			float4 _InitStateTex_ST;

			uniform float4 _StartTime;//開始時間
			uniform float _isActive;//有効かどうか

			uniform float _minColor;//しきい値
			uniform float _FadeSpeed;//フェード速度

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				_StartTime = tex2D(_InitStateTex, v.uv);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 addcol = tex2D(_TransitionTex, i.uv);

				addcol = step(addcol,((_Time.x - _StartTime.x) * _FadeSpeed * _isActive));
				col = col * addcol;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
