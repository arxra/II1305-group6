// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Peacerich/BendWorld"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Curvature ("Curvature", Float) = 0.001
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow

		#pragma target 3.0

        sampler2D _MainTex;
		uniform float _Curvature;

		half _Glossiness;
        half _Metallic;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void vert(inout appdata_full v)
		{
			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4(0.0f, (worldSpace.z * worldSpace.z) * -_Curvature, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}

		UNITY_INSTANCING_BUFFER_START(Props)

		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
