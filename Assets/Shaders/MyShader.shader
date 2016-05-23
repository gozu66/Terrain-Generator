Shader "Custom/My Shader"
{
	properties
	{
		_Tint ("Tint", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass 
		{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			
			float4 MyVertexProgram(float4 position : POSITION, out float3 localPosition : TEXCOORD0) : SV_POSITION
			{
				localPosition = position.xyz;
				
				return mul(UNITY_MATRIX_MVP, position);
			}

			float4 MyFragmentProgram(float3 localPosition : TEXCOORD0) : SV_TARGET
			{
				return float4(localPosition + 0.5, 1) * _Tint;
			}
			ENDCG
		}
	}
}