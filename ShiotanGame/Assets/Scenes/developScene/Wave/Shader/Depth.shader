Shader "Unlit/Depth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
			#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv));
			depth = sign(abs(depth));
				return fixed4(depth,depth,depth,1);
			}
			ENDCG
		}
	}
}
