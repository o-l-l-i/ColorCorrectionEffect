// Color correction effect by Olli S.

Shader "Hidden/ColorCorrection"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}

        _HSVhue ("Hue", float) = 0
        _HSVsaturation ("Saturation", float) = 1
        _HSVvalue ("Value", float) = 1

        _InGamma ("Gamma", float) = 1
        _InWhite ("In White", int) = 0
        _InBlack ("In Black", int) = 255
        _OutWhite ("Out White", int) = 0
        _OutBlack ("Out Black", int) = 255

        _Brightness ("Brightness", float) = 0
        _BrightnessR ("Brightness R", float) = 0
        _BrightnessG ("Brightness G", float) = 0
        _BrightnessB ("Brightness B", float) = 0

        _Contrast ("Contrast", float) = 1
        _ContrastR ("Contrast R", float) = 1
        _ContrastG ("Contrast G", float) = 1
        _ContrastB ("Contrast B", float) = 1

        _Inverse ("Invert Colors", float) = 0

        _Grayscale ("Grayscale", float) = 0

        _Saturation ("Saturation", float) = 0
    }


    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile __ ENABLEHSV
            #pragma multi_compile __ ENABLEGAMMA
            #pragma multi_compile __ ENABLEBRIGHTNESS
            #pragma multi_compile __ ENABLEBRIGHTNESSCHANNELS
            #pragma multi_compile __ ENABLECONTRAST
            #pragma multi_compile __ ENABLECONTRASTCHANNELS
            #pragma multi_compile __ ENABLEINVERSE
            #pragma multi_compile __ ENABLEGRAYSCALE
            #pragma multi_compile __ ENABLESATURATION
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            uniform float _HSVhue;
            uniform float _HSVsaturation;
            uniform float _HSVvalue;

            uniform float _InGamma;
            uniform fixed _InWhite;
            uniform fixed _InBlack;
            uniform fixed _OutWhite;
            uniform fixed _OutBlack;

            uniform float _Brightness;
            uniform float _BrightnessR;
            uniform float _BrightnessG;
            uniform float _BrightnessB;

            uniform float _Contrast;
            uniform float _ContrastR;
            uniform float _ContrastG;
            uniform float _ContrastB;

            uniform float _Inverse;
            uniform float _Grayscale;

            uniform float _Saturation;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };


            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                #ifdef UNITY_HALF_TEXEL_OFFSET
                    v.uv.y += _MainTex_TexelSize.y;
                #endif

                #if SHADER_API_D3D9
                if (_MainTex_TexelSize.y < 0)
                    v.uv.y = 1.0 - v.uv.y;
                #endif

                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.uv);

                return o;
            }


            #if ENABLEHSV
                // HSV color correction concepts borrowed from:
                // http://www.chilliant.com/rgb2hsv.html
                float3 RGBtoHSV(float3 RGB)
                {
                    float3 HSV = 0;
                    HSV.z = max(RGB.r, max(RGB.g, RGB.b));
                    float M = min(RGB.r, min(RGB.g, RGB.b));
                    float C = HSV.z - M;

                    if (C != 0)
                    {
                        HSV.y = C / HSV.z;
                        float3 Delta = (HSV.z - RGB) / C;
                        Delta.rgb -= Delta.brg;
                        Delta.rg += float2(2,4);
                        if (RGB.r >= HSV.z)
                            HSV.x = Delta.b;
                        else if (RGB.g >= HSV.z)
                            HSV.x = Delta.r;
                        else
                            HSV.x = Delta.g;
                        HSV.x = frac(HSV.x / 6);
                    }

                    return HSV;
                }


                float3 Hue(float H)
                {
                    float R = abs(H * 6 - 3) - 1;
                    float G = 2 - abs(H * 6 - 2);
                    float B = 2 - abs(H * 6 - 4);

                    return saturate(float3(R,G,B));
                }


                float3 HSVtoRGB(in float3 HSV)
                {
                    return ((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z;
                }


                float3 HSVadjust(float3 c, float h, float s, float v)
                {

                    fixed3 colhsv = RGBtoHSV(c.rgb);
                    colhsv.x += _HSVhue;
                    colhsv.y *= _HSVsaturation;
                    colhsv.z *= _HSVvalue;
                    colhsv.x = fmod(colhsv.x, 1.0);
                    c.rgb = HSVtoRGB(colhsv.xyz);

                    return c;
                }
            #endif


            #if ENABLEGAMMA
                float3 GammaSimple(float3 c, float GammaValue, fixed inLow, fixed inHigh, fixed outLow, fixed outHigh)
                {
                    float inR = max(c.r - inLow/255.0, 0.0) / ((inHigh/255.0) - (inLow/255.0));
                    c.r = min(inR, 1.0);
                    float inG = max(c.g - inLow/255.0, 0.0) / ((inHigh/255.0) - (inLow/255.0));
                    c.g = min(inG, 1.0);
                    float inB = max(c.b - inLow/255.0, 0.0) / ((inHigh/255.0) - (inLow/255.0));
                    c.b = min(inB, 1.0);

                    float3 gammaCorrected = pow(c, 1 / GammaValue);
                    c = gammaCorrected;

                    float outR = lerp(outLow/255.0, outHigh/255.0, c.r);
                    c.r = outR;
                    float outG = lerp(outLow/255.0, outHigh/255.0, c.g);
                    c.g = outG;
                    float outB = lerp(outLow/255.0, outHigh/255.0, c.b);
                    c.b = outB;

                    return c;
                }
            #endif


            #if ENABLEBRIGHTNESS
                float3 Brightness(float3 c, float brightness)
                {
                    c.rgb += brightness;

                    return c;
                }
            #endif


            #if ENABLEBRIGHTNESSCHANNELS
                float BrightnessChannel(float chan, float brightness)
                {
                    chan += brightness;

                    return chan;
                }
            #endif


            #if ENABLECONTRAST
                float3 Contrast(float3 c, float contrast)
                {
                    c.rgb -= 0.5;
                    c.rgb *= contrast;
                    c.rgb += 0.5;

                    return c;
                }
            #endif


            #if ENABLECONTRASTCHANNELS
                float ContrastChannel(float chan, float contrast)
                {
                    chan -= 0.5;
                    chan *= contrast;
                    chan += 0.5;

                    return chan;
                }
            #endif


            #if ENABLEINVERSE
                float3 Invert(float3 c)
                {
                    c.rgb = 1 - c.rgb;

                    return c;
                }
            #endif


            #if ENABLEGRAYSCALE
                float3 ConvertToGray(float3 c)
                {
                    c.rgb = c.r * 0.3 + c.g * 0.59 + c.b * 0.11;

                    return c;
                }
            #endif


            #if ENABLESATURATION
                float3 Saturation(float3 c, float saturation)
                {
                    float3 grayscale;
                    grayscale.rgb = c.r * 0.3 + c.g * 0.59 + c.b * 0.11;

                    c.rgb = lerp(grayscale.rgb, c.rgb, saturation);

                    return c;
                }
            #endif


            float4 frag(v2f i) : COLOR
            {
                float4 col = tex2D(_MainTex, i.uv);


                #if ENABLEHSV
                    col.rgb = HSVadjust(col.rgb, _HSVhue, _HSVsaturation, _HSVvalue);
                #endif


                #if ENABLEGAMMA
                    col.rgb = GammaSimple(col.rgb, _InGamma, _InBlack, _InWhite, _OutBlack, _OutWhite);
                #endif


                #if ENABLEBRIGHTNESS
                    col.rgb = Brightness(col.rgb, _Brightness);
                #endif


                #if ENABLEBRIGHTNESSCHANNELS
                    col.r = BrightnessChannel(col.r, _BrightnessR);
                    col.g = BrightnessChannel(col.g, _BrightnessG);
                    col.b = BrightnessChannel(col.b, _BrightnessB);
                #endif


                #if ENABLECONTRAST
                    col.rgb = Contrast(col.rgb, _Contrast);
                #endif


                #if ENABLECONTRASTCHANNELS
                    col.r = ContrastChannel(col.r, _ContrastR);
                    col.g = ContrastChannel(col.g, _ContrastG);
                    col.b = ContrastChannel(col.b, _ContrastB);
                #endif


                #if ENABLESATURATION
                    col.rgb = Saturation(col.rgb, _Saturation);
                #endif


                #if ENABLEINVERSE
                    col.rgb = Invert(col.rgb);
                #endif


                #if ENABLEGRAYSCALE
                    col.rgb = ConvertToGray(col.rgb);
                #endif


                return col;
            }

            ENDCG
        }
    }
}