Shader "Custom/BasicUnlit"
{
	Properties
	{
		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)
		_ColorSide ("Non-Forward Color", Color) = (0.0,0.0,0.0,1.0)
		_Overlay ("Overlay", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			sampler2D _Overlay;
			float4 _Overlay_ST;

			float4 _Color;
			float4 _ColorSide;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Overlay);
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = _Color;
				float4 tex = tex2D(_Overlay, i.uv);

				col = lerp(col, tex, tex.a);

				float largestNormal = abs(max(i.normal.x, i.normal.y));

				float4 trueColorSide = lerp(_Color, _ColorSide, _ColorSide.a);

				col = lerp(col, trueColorSide, largestNormal);

				return col;
			}
			ENDCG
		}
	}
}
