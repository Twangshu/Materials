Shader "Custom/GrayScale" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _LuminosityAmount ("GrayScale Amount", Range(0.0, 1)) = 1.0
    }
    SubShader {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            fixed _LuminosityAmount;

            uniform sampler2D _OutLine;
            fixed _NumPixelH;
            fixed _NumPixelV;
            
            fixed4 frag(v2f_img i) : COLOR
            {   
                fixed4 outlineTex = tex2D(_OutLine, i.uv);
                fixed4 renderTex = tex2D(_MainTex, i.uv);

                if((outlineTex.r+outlineTex.b+outlineTex.g+outlineTex.a)<0.1f)
                {
                    float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
                    fixed4 finalColor = lerp(renderTex, luminosity, _LuminosityAmount);
                    return finalColor;
                }
                else
                {
                    return renderTex;
                }
            }
    
            ENDCG
            }
    } 
    FallBack "Diffuse"
}