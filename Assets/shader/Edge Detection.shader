Shader "TA/Edge Detection" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_EdgeOnly("Edge Only", Float) = 1.0
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
		_BackgroundColor("Background Color", Color) = (1, 1, 1, 1)
	}
		SubShader{
			Pass {
			//设置渲染状态
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert  
			#pragma fragment fragSobel

			sampler2D _MainTex;

		uniform half4 _MainTex_TexelSize;
		fixed _EdgeOnly;
		fixed4 _EdgeColor;
		fixed4 _BackgroundColor;

		struct v2f {
			float4 pos : SV_POSITION;
			half2 uv[9] : TEXCOORD0;
		};

		v2f vert(appdata_img v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			half2 uv = v.texcoord;
			//通过_MainTex_TexelSize.xy值，计算需要计算的矩阵的UV值
			o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
			o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
			o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
			o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
			o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
			o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
			o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
			o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
			o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);

			return o;
		}
		//计算亮度值
		fixed luminance(fixed4 color) {
			return  0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
		}

		//通过Sobe算子算出这个像素点与周围的梯度值
		half Sobel(v2f i) {
			//Sobe算子矩阵，分为X与Y两个方向
			const half Gx[9] = {-1,  0,  1,
									-2,  0,  2,
									-1,  0,  1};
			const half Gy[9] = {-1, -2, -1,
									0,  0,  0,
									1,  2,  1};

			half texColor;
			half edgeX = 0;
			half edgeY = 0;
			//累加周围采样点的梯度值
			for (int it = 0; it < 9; it++) {
				texColor = luminance(tex2D(_MainTex, i.uv[it]));
				edgeX += texColor * Gx[it];
				edgeY += texColor * Gy[it];
			}
			//用绝对值代替开根号
			half edge = 1 - abs(edgeX) - abs(edgeY);

			return edge;
		}

		fixed4 fragSobel(v2f i) : SV_Target {
			half edge = Sobel(i);
		//根据梯度值，将原颜色与背景颜色、描边颜色进行插值运算，得到最终的颜色值
		fixed4 withEdgeColor = lerp(_EdgeColor, tex2D(_MainTex, i.uv[4]), edge);
		fixed4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
		return lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly);
	}

	ENDCG
}
		}
			FallBack Off
}