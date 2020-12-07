Shader "Custom/UseGPUInstance"
{
	Properties
	{
		_MainTex("Albedo", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
# pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:vertInstancingSetup
			#define UNITY_PARTICLE_INSTANCE_DATA MyParticleInstanceData
			#define UNITY_PARTICLE_INSTANCE_DATA_NO_ANIM_FRAME
			struct MyParticleInstanceData
			{
				float3x4 transform;
				uint color;
				float speed;
			};
			#include "UnityCG.cginc"
			#include "UnityStandardParticleInstancing.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				o.color = v.color;
				o.texcoord = v.texcoord;
				vertInstancingColor(o.color);
				vertInstancingUVs(v.texcoord, o.texcoord);
# if defined(UNITY_PARTICLE_INSTANCING_ENABLED)
				UNITY_PARTICLE_INSTANCE_DATA data = unity_ParticleInstanceData[unity_InstanceID];
				o.color.rgb += data.speed;
# endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				half4 albedo = tex2D(_MainTex, i.texcoord);
				return i.color * albedo;
			}
			ENDCG
		}
	}
}
