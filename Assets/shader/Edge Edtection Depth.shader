Shader "TA/Edge Detection 2"
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
			CGINCLUDE

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _OutlineColor;
			half _SampleDis;
			half _SensitiveNormal;
			half _SensitiveDepth;
			half _EpsNormal;
			half _EpsDepth;

			fixed4 _MainTex_TexelSize;
			sampler2D _CameraDepthNormalsTexture;//获取相机里的深度法线图

			struct v2f
			{
				half4 pos:SV_POSITION;
				half2 uv[5]:TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f res;

				//得到顶点的视图坐标
				res.pos = UnityObjectToClipPos(v.vertex);

				half2 centerUv = v.texcoord;
				res.uv[0] = centerUv;
				//开抗锯齿的时候，direct3d绘制是从上往下的
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					centerUv.y = 1 - centerUv.y;
				#endif

				
				//以这个顶点为中心，取一个矩阵
				//放在VS算比较快
				res.uv[1] = centerUv + half2(1,1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[2] = centerUv + half2(-1,-1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[3] = centerUv + half2(-1,1)*_SampleDis*_MainTex_TexelSize.xy;
				res.uv[4] = centerUv + half2(1,-1)*_SampleDis*_MainTex_TexelSize.xy;

				return res;
			}

			//判断法线深度是否一样
			half isSame(half4 a,half4 b)
			{
				half2 aNom = a.xy;
				half2 bNom = b.xy;
				half2 subNom = abs(aNom - bNom)*_SensitiveNormal;
				half sameNom = step(subNom.x + subNom.y,_EpsNormal);

				half aDep = DecodeFloatRG(a.zw);
				half bDep = DecodeFloatRG(b.zw);
				half subDep = abs(aDep - bDep)*_SensitiveDepth;
				half sameDep = step(subDep,_EpsDepth);
				return sameNom * sameDep;
			}

			fixed4 frag(v2f p) :SV_Target
			{
				half4 sample1 = tex2D(_CameraDepthNormalsTexture,p.uv[1]);
				half4 sample2 = tex2D(_CameraDepthNormalsTexture,p.uv[2]);
				half4 sample3 = tex2D(_CameraDepthNormalsTexture,p.uv[3]);
				half4 sample4 = tex2D(_CameraDepthNormalsTexture,p.uv[4]);

				half same = isSame(sample1,sample2)*isSame(sample3,sample4);
				fixed4 res;
				res = lerp(_OutlineColor,tex2D(_MainTex,p.uv[0]),same);//法线深度有一个不同就描边
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