// Color correction effect by Olli S.

using UnityEngine;


[ExecuteInEditMode]
public class ColorCorrectionEffect : MonoBehaviour
{
    public bool enableEffect;

    [Header("Hue Saturation Value")]
    public bool enableHSV;
    [Range(0f, 1.0f)]
    public float hsvHue = 1;
    [Range(0,8)]
    public float hsvSaturation = 1;
    [Range(0,2)]
    public float hsvValue = 1;

    [Header("Gamma")]
    public bool enableGamma;
    [Range(0f,10f)]
    public float gamma = 1;

    [Header("Gamma Input Levels")]
    [Range(0,255)]
    public int inBlack = 0;
    [Range(0,255)]
    public int inWhite = 255;

    [Header("Gamma Output Levels")]
    [Range(0,255)]
    public int outBlack = 0;
    [Range(0,255)]
    public int outWhite = 255;

    [Header("Brightness")]
    public bool enableBrightness;
    [Range(-1,1)]
    public float brightness = 0;

    [Header("Brightness, Channels")]
    public bool enableBrightnessChannels;
    [Range(-1,1)]
    public float brightnessR = 0;
    [Range(-1,1)]
    public float brightnessG = 0;
    [Range(-1,1)]
    public float brightnessB = 0;

    [Header("Contrast")]
    public bool enableContrast;
    [Range(0,5)]
    public float contrast = 1;

    [Header("Contrast, Channels")]
    public bool enableContrastChannels;
    [Range(0,5)]
    public float contrastR = 1;
    [Range(0,5)]
    public float contrastG = 1;
    [Range(0,5)]
    public float contrastB = 1;

    [Header("Toggle Inverse")]
    public bool enableInverse;

    [Header("Toggle Grayscale")]
    public bool enableGrayscale;

    [Header("Saturation")]
    public bool enableSaturation;
    [Range(0,5)]
    public float saturation = 1;


    Material material;


    void Awake()
    {
        material = new Material(Shader.Find("Hidden/ColorCorrection"));
    }


    void OnRenderImage (RenderTexture rtSource, RenderTexture rtDestination)
    {

        // TOGGLES ------------------------------------------------------------
        #region toggles

        if (!enableEffect)
        {
            Graphics.Blit (rtSource, rtDestination);
            return;
        }


        if (enableHSV)
        {
            material.EnableKeyword ("ENABLEHSV");
            material.SetFloat("_HSVhue", hsvHue);
            material.SetFloat("_HSVsaturation", hsvSaturation);
            material.SetFloat("_HSVvalue", hsvValue);
        }
        else if (!enableHSV)
        {
            material.DisableKeyword ("ENABLEHSV");
        }


        if (enableGamma)
        {
            material.EnableKeyword ("ENABLEGAMMA");
            material.SetFloat("_InGamma", gamma);
            material.SetInt("_InWhite", inWhite);
            material.SetInt("_InBlack", inBlack);
            material.SetInt("_OutWhite", outWhite);
            material.SetInt("_OutBlack", outBlack);
        }
        else if (!enableGamma)
        {
            material.DisableKeyword ("ENABLEGAMMA");
        }


        if (enableBrightness)
        {
            material.EnableKeyword ("ENABLEBRIGHTNESS");
            material.SetFloat("_Brightness", brightness);
        }
        else if (!enableBrightness)
        {
            material.DisableKeyword ("ENABLEBRIGHTNESS");
        }


        if (enableBrightnessChannels)
        {
            material.EnableKeyword("ENABLEBRIGHTNESSCHANNELS");
            material.SetFloat("_BrightnessR", brightnessR);
            material.SetFloat("_BrightnessG", brightnessG);
            material.SetFloat("_BrightnessB", brightnessB);
        }
        else if (!enableBrightnessChannels)
        {
            material.DisableKeyword("ENABLEBRIGHTNESSCHANNELS");
        }


        if (enableContrast)
        {
            material.EnableKeyword ("ENABLECONTRAST");
            material.SetFloat("_Contrast", contrast);
        }
        else if (!enableContrast)
        {
            material.DisableKeyword ("ENABLECONTRAST");
        }


        if (enableContrastChannels)
        {
            material.EnableKeyword ("ENABLECONTRASTCHANNELS");
            material.SetFloat("_ContrastR", contrastR);
            material.SetFloat("_ContrastG", contrastG);
            material.SetFloat("_ContrastB", contrastB);
        }
        else if (!enableContrastChannels)
        {
            material.DisableKeyword ("ENABLECONTRASTCHANNELS");
        }


        if (enableInverse)
        {
            material.EnableKeyword ("ENABLEINVERSE");
        }
        else if (!enableInverse)
        {
            material.DisableKeyword ("ENABLEINVERSE");
        }


        if (enableGrayscale)
        {
            material.EnableKeyword ("ENABLEGRAYSCALE");
        }
        else if (!enableGrayscale)
        {
            material.DisableKeyword ("ENABLEGRAYSCALE");
        }


        if (enableSaturation)
        {
            material.EnableKeyword ("ENABLESATURATION");
            material.SetFloat ("_Saturation", saturation);
        }
        else if (!enableSaturation)
        {
            material.DisableKeyword ("ENABLESATURATION");
        }

        #endregion


        // RENDER EFFECT ------------------------------------------------------
        Graphics.Blit (rtSource, rtDestination, material);

    }


    // RESETS -----------------------------------------------------------------
    #region resets

    public void ResetHSV()
    {
        hsvHue = 1;
        hsvSaturation = 1;
        hsvValue = 1;
    }


    public void ResetGamma()
    {
        gamma = 1;
        inBlack = 0;
        inWhite = 255;
        outBlack = 0;
        outWhite = 255;
    }


    public void ResetBrightness()
    {
        brightness = 0;
    }


    public void ResetBrightnessChannels()
    {
        brightnessR = 0;
        brightnessG = 0;
        brightnessB = 0;
    }


    public void ResetContrast()
    {
        contrast = 1;
    }


    public void ResetContrastChannels()
    {
        contrastR = 1;
        contrastG = 1;
        contrastB = 1;
    }


    public void ResetSaturation()
    {
        saturation = 1;
    }

    #endregion

}