Shader "Unlit/PlaneDepthSet"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Fade" "Queue" = "Background-1"}
        LOD 100

		//デプスマップ書き込みのみ
        Pass
        {
		ZWrite ON
		ColorMask 0
        }
    }
}
