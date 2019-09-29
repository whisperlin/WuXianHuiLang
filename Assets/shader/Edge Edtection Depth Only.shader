Shader "TA/Edge Detection Depth Only"
{
	Properties
	{
		_MainTex("MainTex",2D) = "white" {}
		_OutlineColor("OutlineColor",Color) = (0,0,0,1)
		_SampleDis("SampleDis",Float) = 1
		_SensitiveNormal("SensitiveNormal",Float) = 1
		_SensitiveDepth("SensitiveDepth",Float) = 1
		_EpsNormal("EpsNormal",Float) = 0.1
		_EpsDepth("EpsDepth",Float) = 0.1
	}

		SubShader
		{
			ZTest Off
			CGINCLUDE
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 _OutlineColor;
			half _SampleDis;
			half _SensitiveNormal;
			half _SensitiveDepth;
			half _EpsNormal;
			half _EpsDepth;
			half4 _MainTex_TexelSize;
			sampler2D _CameraDepthNormalsTexture;
			struct v2f
			{
				half4 pos:SV_POSITION;
				half2 uv[5]:TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f res;

				res.pos = UnityObjectToClipPos(v.vertex);

				half2 centerUv = v.texcoord;
				res.uv[0] = centerUv;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					centerUv.y = 1 - centerUv.y;
				#endif

				res.uv[1] = centerUv + half2(1,1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[2] = centerUv + half2(-1,-1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[3] = centerUv + half2(-1,1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[4] = centerUv + half2(1,-1)*_SampleDis*_MainTex_TexelSize.xy;

				return res;
			}
			half isSame(half4 a,half4 b)
			{
			 
				half2 subNom = abs(a.xy - b.xy)*_SensitiveNormal;
				float z = subNom.x + subNom.y;
				return  step(z, _EpsNormal);
				//half sameNom = step(subNom.x + subNom.y,_EpsNormal);
				//return sameNom  ;
				//return 1;
			}

			fixed4 frag(v2f p) :SV_Target
			{
				half4 sample0 = tex2D(_CameraDepthNormalsTexture,p.uv[0]);
				half4 sample1 = tex2D(_CameraDepthNormalsTexture,p.uv[1]);
				half4 sample2 = tex2D(_CameraDepthNormalsTexture,p.uv[2]);
				half4 sample3 = tex2D(_CameraDepthNormalsTexture,p.uv[3]);
				half4 sample4 = tex2D(_CameraDepthNormalsTexture,p.uv[4]);
				float4 col0 = tex2D(_MainTex, p.uv[0]);
				half same = isSame(sample1,sample2)*isSame(sample3,sample4);
				fixed4 res;
				res = lerp(_OutlineColor, col0,same);
				return res;
			}

			ENDCG

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
			}

		}

			FallBack Off

}