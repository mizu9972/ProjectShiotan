Shader "Unlit/UnderWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="transparent" "Queue" = "background"}
        LOD 100

		GrabPass{"_GrabTex"}
		
        Pass
        {
		//ZWrite OFF
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
				half4 GrabPos:TEXCOORD1;
				half4 ScrPos:TEXCOORD2;
            };

            sampler2D _MainTex;
			sampler2D _CameraDepthTexture;//デプスマップ
			sampler2D _GrabTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

				o.GrabPos = ComputeGrabScreenPos(o.vertex);
				o.ScrPos = ComputeScreenPos(o.vertex);//デプスマップ取得
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

			float4 Depth = i.ScrPos;
			//Depth.xy = i.GrabPos.xy / i.GrabPos.w;

			float4 GrabColor = tex2D(_GrabTex, i.uv);
			float SeaDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(Depth)));
			float MyDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(i.ScrPos.z);
			float SubDepth = 1 - sign(saturate(SeaDepth - MyDepth));
			// apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				//if (SubDepth == 1) {
				//	discard;
				//	//return float4(1, 0, 0, 1);
				//}
				//return (col * SubDepth) + (GrabColor * (1 - SubDepth));
				return col;
            }
            ENDCG
        }
    }
}
