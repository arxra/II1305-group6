Shader "SumoDashParticle"
{
	Properties
	{
		_MainTex("Albedo", 2D) = "white" {}
		_Normal("Normap Map", 2D) = "bump" {}
		_Color("Color", Color) = (1,1,1,1)
		_Curvature ("Curvature", Float) = 0.001
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;
		uniform float _Curvature;

		struct Input
		{
			float2 uv_MainTex;
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color;

			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4(0.0f, (worldSpace.z * worldSpace.z) * -_Curvature, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}

		UNITY_INSTANCING_BUFFER_START(Props)

		UNITY_INSTANCING_BUFFER_END(Props)

		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb*IN.color;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			o.Alpha = c.a*IN.color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}