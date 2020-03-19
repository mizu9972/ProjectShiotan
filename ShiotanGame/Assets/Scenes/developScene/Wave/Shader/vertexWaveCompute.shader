﻿Shader "Unlit/WaveCompute"
{
    Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PhaseVelocity("PhaseVelocity",Range(0,0.5)) = 0.1
		_Attenuation("Attenuation",Range(0.9,1.0)) = 1.0
		_DeltaUV("Delta UV",Float) = 1
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
			float4 _MainTex_TexelSize;
			float _PhaseVelocity;
			float _Attenuation;
			float _DeltaUV;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float4 frag (v2f i) : SV_Target
            {
			float2 coord = i.uv;
            float4 col = tex2D(_MainTex, coord);
			float3 duv = float3(_MainTex_TexelSize.x, _MainTex_TexelSize.y, 0) * _DeltaUV;
			
			//波動方程式
			float dh = tex2D(_MainTex, coord + duv.xz).r
				+ tex2D(_MainTex, coord - duv.xz).r
				+ tex2D(_MainTex, coord + duv.zy).r
				+ tex2D(_MainTex, coord - duv.zy).r
				- 4 * col.r;
			dh = (2 * (col.r * 2 - col.g + dh * _PhaseVelocity) - 1) * _Attenuation;
			
			dh = (dh + 1) * 0.5;//-1~1の間に補正
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				return float4(dh, col.r, 0, 0);
            }
            ENDCG
        }
    }
}