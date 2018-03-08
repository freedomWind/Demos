// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/NewImageEffectShader"
{
	Properties{
		_Color ("Color Tint",Color) = (1.0,1.0,1.0,1.0)
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct a2v{
				float4 vertex :POSITION;
				float3 normal :NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			struct v2f{
				float4 pos :SV_POSITION;
				fixed3 color:COLOR0;
			};

			v2f vert(a2v v) :v2f{
				v2f O;
				O.pos = UnityObjectToClipPos(v.vertex);
				O.color = v.normal * 0.5 +fixed3(0.5,0.5,0.5);
				return O;
			}
			fixed4 frag(v2f i):SV_Target{
				fixed3 c = i.color;
				c *= _Color.rgb;
				return fixed4(c,1.0);
			}
/*
			float4 vert(float4 v:POSITION):SV_POSITION{
				return UnityObjectToClipPos(v);
			}

			fixed4 frag():SV_Target{
				return fixed4(1.0,1.0,1.0,1.0);
			}
			*/
			ENDCG
		}
	}
}
