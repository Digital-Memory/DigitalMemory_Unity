Shader "Custom/zOverride"
{
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}

		Pass{
			ZWrite Off
		}
	}
}
