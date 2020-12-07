Shader "Custom/ToonShader" {
	// プロパティ
	Properties{
		// ベースとなる色
		_Color("Color", Color) = (1, 1, 1, 1)
		// メインテクスチャ
		_MainTex("Albedo(RGB)", 2D) = "white" {}
	// rampテクスチャ
	_RampTex("Ramp", 2D) = "white" {}
	}

		// Shaderの中身を記述
		SubShader{
		// 一般的なShaderを使用
		Tags {"RenderType" = "Opaque"}
		// しきい値
		LOD 200

		// cg言語記述
		CGPROGRAM
		// メソッド名がLightingToonRampのカスタムライティング宣言
		#pragma surface surf ToonRamp
		// Shader Model
		#pragma target 3.0

		// メインテクスチャ
		sampler2D _MainTex;
	// rampテクスチャ
	sampler2D _RampTex;

	// input構造体
	struct Input {
		// uv座標
		float2 uv_MainTex;
	};

	// ベースとなる色
	fixed4 _Color;

	// カスタムライティング
	fixed4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) {
		// 内積を取得
		half diff = dot(s.Normal, lightDir);
		// rampテクスチャのuv値を取得
		fixed3 ramp = tex2D(_RampTex, fixed2(diff, diff)).rgb;
		// rampテクスチャのuv値から色を取得
		fixed4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
		c.a = s.Alpha;
		return c;
	}

	// surf関数
	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	// Shaderの記述終了
	ENDCG
	}
		// SubShaderが失敗した時に呼ばれる
		Fallback "Diffuse"
}