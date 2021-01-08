// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ColorCorrectionCurvesSimple" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_RgbTex ("_RgbTex (RGB)", 2D) = "" {}
		_isShowGreen("isShowGreen",float) = 0
		_isShowYellow("isShowYellow",float) = 0
	}
	
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	sampler2D _RgbTex;
	float _isShowGreen;
	float _isShowYellow;
	float _isShowBlue;
	float _isShowRed;
	float _isShowAll;
	fixed _Saturation;
	
	
	fixed3 RGB2HSV(fixed3 c){
    fixed4 K = fixed4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	fixed4 p = lerp(fixed4(c.bg, K.wz), fixed4(c.gb, K.xy), step(c.b, c.g));
	fixed4 q = lerp(fixed4(p.xyw, c.r), fixed4(c.r, p.yzx), step(p.x, c.r));

	fixed d = q.x - min(q.w, q.y);
	fixed e = 1.0e-10;
	return fixed3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
	}
	
	//显示绿色青色
	bool ShowGreen(fixed3 c)
	{	
		if(_isShowGreen == 0)
		return false;
		//#--------------------------hsv范围调值
		float greenHBorderLow = 35 * 0.00556;
		float greenHBorderHigh = 99 * 0.00556;
		float commonSBorderLow = 43 * 0.0039;
		float commonSBorderHigh = 1;
		float commonVBorderLow = 46 * 0.0039;
		float commonVBorderHigh = 1;
		//#--------------------------hsv范围调值
		fixed3 hsv = RGB2HSV(c.rgb);
		if(hsv.r>greenHBorderLow && hsv.r <greenHBorderHigh && hsv.g > commonSBorderLow && hsv.g < commonSBorderHigh && hsv.b > commonVBorderLow && hsv.b < commonVBorderHigh)
		{
			return true;
		}
		return false;
	}

	//显示橙黄
	bool ShowYello(fixed3 c)
	{	
		if(_isShowYellow==0)
		return false;
		//#--------------------------hsv范围调值
		float commonHBorderLow = 11 * 0.00556;
		float commonHBorderHigh = 34 * 0.00556;
		float commonSBorderLow = 43 * 0.0039;
		float commonSBorderHigh = 1;
		float commonVBorderLow = 46 * 0.0039;
		float commonVBorderHigh = 1;
		//#--------------------------hsv范围调值
		fixed3 hsv = RGB2HSV(c.rgb);
		if(hsv.r>commonHBorderLow && hsv.r <commonHBorderHigh && hsv.g > commonSBorderLow && hsv.g < commonSBorderHigh && hsv.b > commonVBorderLow && hsv.b < commonVBorderHigh)
		{
			return true;
		}
		return false;
	}

	
	//显示蓝紫
	bool ShowBlue(fixed3 c)
	{	
		if(_isShowBlue==0)
		return false;
		//#--------------------------hsv范围调值
		float commonHBorderLow = 100 * 0.00556;
		float commonHBorderHigh = 155 * 0.00556;
		float commonSBorderLow = 43 * 0.0039;
		float commonSBorderHigh = 1;
		float commonVBorderLow = 46 * 0.0039;
		float commonVBorderHigh = 1;
		//#--------------------------hsv范围调值
		fixed3 hsv = RGB2HSV(c.rgb);
		if(hsv.r>commonHBorderLow && hsv.r <commonHBorderHigh && hsv.g > commonSBorderLow && hsv.g < commonSBorderHigh && hsv.b > commonVBorderLow && hsv.b < commonVBorderHigh)
		{
			return true;
		}
		return false;
	}

	//显示红
	bool ShowRed(fixed3 c)
	{	
		if(_isShowRed==0)
		return false;
		//#--------------------------hsv范围调值
		float commonHBorderLow = 0 * 0.00556;
		float commonHBorderHigh = 10 * 0.00556;
		float redHBorderLow = 156 * 0.00556;
		float redHBorderHigh = 180 * 0.00556;
		float commonSBorderLow = 43 * 0.0039;
		float commonSBorderHigh = 1;
		float commonVBorderLow = 46 * 0.0039;
		float commonVBorderHigh = 1;
		//#--------------------------hsv范围调值
		fixed3 hsv = RGB2HSV(c.rgb);
		if( ((hsv.r>commonHBorderLow && hsv.r <commonHBorderHigh) || (hsv.r> redHBorderLow && hsv.r < redHBorderHigh)) && hsv.g > commonSBorderLow && hsv.g < commonSBorderHigh && hsv.b > commonVBorderLow && hsv.b < commonVBorderHigh)
		{
			return true;
		}
		return false;
	}

	fixed3 ShowCollectColor(fixed3 color){
		if(_isShowAll == 1 || ShowGreen(color) ||ShowYello(color) ||ShowBlue(color) || ShowRed(color))
		return color;
		fixed lum = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;// 等效Luminance(color.rgb);
		color.rgb = lerp(fixed3(lum,lum,lum), color.rgb, _Saturation);
		return color;
	}

	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	fixed4 frag(v2f i) : SV_Target 
	{
		fixed4 color = tex2D(_MainTex, i.uv); 
		
		fixed3 red = tex2D(_RgbTex, half2(color.r, 0.5/4.0)).rgb * fixed3(1,0,0);
		fixed3 green = tex2D(_RgbTex, half2(color.g, 1.5/4.0)).rgb * fixed3(0,1,0);
		fixed3 blue = tex2D(_RgbTex, half2(color.b, 2.5/4.0)).rgb * fixed3(0,0,1);
		
		color = fixed4(red+green+blue, color.a);

		fixed lum = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;// 等效Luminance(color.rgb);
		color.rgb = ShowCollectColor(color.rgb);
		return color;		
	}

	

	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
}

Fallback off
	
} // shader
