// Color correction effect by Olli S.

using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ColorCorrectionEffect))]
public class ColorCorrectionEffectEditor : Editor
{

    private ColorCorrectionEffect cc;
    private GUIStyle ButtonStyle;
    private GUIStyle LabelStyle;


    public override void OnInspectorGUI()
    {
        var cc = target as ColorCorrectionEffect;

        serializedObject.Update();

        ButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 100,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(10, 10, 5, 5),
            fontSize = 10,
            fontStyle = FontStyle.Bold,
        };

        ButtonStyle.normal.textColor = Color.black;


        LabelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
        };

        LabelStyle.alignment = TextAnchor.MiddleCenter;


        // HEADER -------------------------------------------------------------
        EditorGUILayout.Space();
        LabelStyle.fontSize = 18;
        EditorGUILayout.LabelField("Color Correction Effect", LabelStyle);
        EditorGUILayout.Space();
        LabelStyle.fontSize = 10;
        EditorGUILayout.LabelField("by Olli S.", LabelStyle);
        EditorGUILayout.Space();


        // HSV ----------------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableHSV = serializedObject.FindProperty("enableHSV");
            EditorGUILayout.PropertyField(enableHSV, new GUIContent("Enable"));

            if (enableHSV.boolValue)
            {
                var hsvHue = serializedObject.FindProperty("hsvHue");
                EditorGUILayout.PropertyField(hsvHue);

                var hsvSaturation = serializedObject.FindProperty("hsvSaturation");
                EditorGUILayout.PropertyField(hsvSaturation);

                var hsvValue = serializedObject.FindProperty("hsvValue");
                EditorGUILayout.PropertyField(hsvValue);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle)) { cc.ResetHSV(); }
                GUI.color = Color.white;
            }
        }


        // GAMMA --------------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableGamma = serializedObject.FindProperty("enableGamma");
            EditorGUILayout.PropertyField(enableGamma, new GUIContent("Enable"));

            if (enableGamma.boolValue)
            {
                var gamma = serializedObject.FindProperty("gamma");
                EditorGUILayout.PropertyField(gamma);

                var inBlack = serializedObject.FindProperty("inBlack");
                EditorGUILayout.PropertyField(inBlack);

                var inWhite = serializedObject.FindProperty("inWhite");
                EditorGUILayout.PropertyField(inWhite);

                var outBlack = serializedObject.FindProperty("outBlack");
                EditorGUILayout.PropertyField(outBlack);

                var outWhite = serializedObject.FindProperty("outWhite");
                EditorGUILayout.PropertyField(outWhite);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetGamma();
                }
                GUI.color = Color.white;
            }
        }


        // BRIGHTNESS ---------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableBrightness = serializedObject.FindProperty("enableBrightness");
            EditorGUILayout.PropertyField(enableBrightness, new GUIContent("Enable"));

            if (enableBrightness.boolValue)
            {
                var brightness = serializedObject.FindProperty("brightness");
                EditorGUILayout.PropertyField(brightness);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetBrightness();
                }
                GUI.color = Color.white;
            }
        }


        // BRIGHTNESS CHANNELS ------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableBrightnessChannels = serializedObject.FindProperty("enableBrightnessChannels");
            EditorGUILayout.PropertyField(enableBrightnessChannels, new GUIContent("Enable"));

            if (enableBrightnessChannels.boolValue)
            {
                var brightnessR = serializedObject.FindProperty("brightnessR");
                EditorGUILayout.PropertyField(brightnessR);

                var brightnessG = serializedObject.FindProperty("brightnessG");
                EditorGUILayout.PropertyField(brightnessG);

                var brightnessB = serializedObject.FindProperty("brightnessB");
                EditorGUILayout.PropertyField(brightnessB);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetBrightnessChannels();
                }
                GUI.color = Color.white;
            }
        }


        // CONTRAST  ----------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableContrast = serializedObject.FindProperty("enableContrast");
            EditorGUILayout.PropertyField(enableContrast, new GUIContent("Enable"));

            if (enableContrast.boolValue)
            {
                var contrast = serializedObject.FindProperty("contrast");
                EditorGUILayout.PropertyField(contrast);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetContrast();
                }
                GUI.color = Color.white;
            }
        }


        // CONTRAST CHANNELS --------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableContrastChannels = serializedObject.FindProperty("enableContrastChannels");
            EditorGUILayout.PropertyField(enableContrastChannels, new GUIContent("Enable"));

            if (enableContrastChannels.boolValue)
            {
                var contrastR = serializedObject.FindProperty("contrastR");
                EditorGUILayout.PropertyField(contrastR);
                var contrastG = serializedObject.FindProperty("contrastG");
                EditorGUILayout.PropertyField(contrastG);
                var contrastB = serializedObject.FindProperty("contrastB");
                EditorGUILayout.PropertyField(contrastB);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetContrastChannels();
                }
                GUI.color = Color.white;
            }
        }


        // INVERSE  -----------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableInverse = serializedObject.FindProperty("enableInverse");
            EditorGUILayout.PropertyField(enableInverse, new GUIContent("Enable"));
        }


        // GRAYSCALE  ---------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableGrayscale = serializedObject.FindProperty("enableGrayscale");
            EditorGUILayout.PropertyField(enableGrayscale, new GUIContent("Enable"));
        }


        // SATURATION  --------------------------------------------------------
        using (var vert = new EditorGUILayout.VerticalScope("box"))
        {
            var enableSaturation = serializedObject.FindProperty("enableSaturation");
            EditorGUILayout.PropertyField(enableSaturation, new GUIContent("Enable"));

            if (enableSaturation.boolValue)
            {
                var saturation = serializedObject.FindProperty("saturation");
                EditorGUILayout.PropertyField(saturation);

                GUI.color = Color.red;
                if (GUILayout.Button("Reset", ButtonStyle))
                {
                    cc.ResetSaturation();
                }
                GUI.color = Color.white;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}