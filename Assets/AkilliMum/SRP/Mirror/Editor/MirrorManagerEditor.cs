//#define DEBUG_RENDER

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AkilliMum.SRP.Mirror
{
    [CustomEditor(typeof(MirrorManager))]
    [CanEditMultipleObjects]
    public class MirrorManagerEditor : Editor
    {
        bool MenuCommon = true;
        bool MenuCamera = true;

#if DEBUG_RENDER
        SerializedProperty DebugImage;
        SerializedProperty DebugImageDepth;
#endif
        SerializedProperty IsEnabled;
        SerializedProperty Name;
        SerializedProperty IsMirrorInMirror;
        SerializedProperty MirrorInMirrorId;
        SerializedProperty TurnOffOcclusion;

        SerializedProperty Platform;

        SerializedProperty WorkingType;
        SerializedProperty TransparencyClearColor;
        SerializedProperty UpVector;
        SerializedProperty CameraLODLevel;
        //SerializedProperty TextureLODLevel;

        SerializedProperty HDR;
        SerializedProperty MSAALevel;
        SerializedProperty FilterMode;
        SerializedProperty DisablePixelLights;
        SerializedProperty Shadow;
        SerializedProperty Cull;
        SerializedProperty CullDistance;
        SerializedProperty RenderTextureSize;
        SerializedProperty ManualSize;
        SerializedProperty ClipPlaneOffset;
        SerializedProperty ReflectLayers;

        SerializedProperty ReflectiveObjects;

        SerializedProperty EnableDepth;

        SerializedProperty CustomShaders;

        string space = "          ";
        GUIStyle headerStyle;


        void OnEnable()
        {
            headerStyle = new GUIStyle { fontStyle = FontStyle.Bold };
#if DEBUG_RENDER
            DebugImage = serializedObject.FindProperty("DebugImage");
            DebugImageDepth = serializedObject.FindProperty("DebugImageDepth");
#endif
            IsEnabled = serializedObject.FindProperty("IsEnabled");
            Name = serializedObject.FindProperty("Name");
            IsMirrorInMirror = serializedObject.FindProperty("IsMirrorInMirror");
            MirrorInMirrorId = serializedObject.FindProperty("MirrorInMirrorId");
            TurnOffOcclusion = serializedObject.FindProperty("TurnOffOcclusion");

            Platform = serializedObject.FindProperty("Platform");

            WorkingType = serializedObject.FindProperty("WorkingType");
            TransparencyClearColor = serializedObject.FindProperty("TransparencyClearColor");
            UpVector = serializedObject.FindProperty("UpVector");
            CameraLODLevel = serializedObject.FindProperty("CameraLODLevel");
            //TextureLODLevel = serializedObject.FindProperty("TextureLODLevel");

            HDR = serializedObject.FindProperty("HDR");
            MSAALevel = serializedObject.FindProperty("MSAALevel");
            FilterMode = serializedObject.FindProperty("FilterMode");
            DisablePixelLights = serializedObject.FindProperty("DisablePixelLights");
            Shadow = serializedObject.FindProperty("Shadow");
            Cull = serializedObject.FindProperty("Cull");
            CullDistance = serializedObject.FindProperty("CullDistance");
            RenderTextureSize = serializedObject.FindProperty("RenderTextureSize");
            ManualSize = serializedObject.FindProperty("ManualSize");
            ClipPlaneOffset = serializedObject.FindProperty("ClipPlaneOffset");
            ReflectLayers = serializedObject.FindProperty("ReflectLayers");

            ReflectiveObjects = serializedObject.FindProperty("ReflectiveObjects");

            EnableDepth = serializedObject.FindProperty("EnableDepth");

            CustomShaders = serializedObject.FindProperty("CustomShaders");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if DEBUG_RENDER
            EditorGUILayout.PropertyField(DebugImage);
            EditorGUILayout.PropertyField(DebugImageDepth);
#endif
            EditorGUILayout.PropertyField(IsEnabled);
            EditorGUILayout.PropertyField(Name);
            EditorGUILayout.PropertyField(TurnOffOcclusion);
            EditorGUILayout.PropertyField(IsMirrorInMirror);
            if (IsMirrorInMirror.boolValue)
            {
                EditorGUILayout.PropertyField(MirrorInMirrorId, new GUIContent { text = space + "Id" });
            }



            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent { text = "Platform Type (Stand-Alone, VR, AR)" }, headerStyle);
            EditorGUILayout.PropertyField(Platform);



            EditorGUILayout.Space();
            //EditorGUILayout.LabelField(new GUIContent { text = "Reflective Objects" }, headerStyle);
            EditorGUILayout.PropertyField(ReflectiveObjects);



            EditorGUILayout.Space();
            MenuCommon = EditorGUILayout.BeginFoldoutHeaderGroup(MenuCommon, new GUIContent { text = "Common" });
            if (MenuCommon)
            {
                EditorGUILayout.PropertyField(WorkingType);
                if (WorkingType.intValue == (int)Mirror.WorkingType.Transparent)
                {
                    EditorGUILayout.PropertyField(TransparencyClearColor, new GUIContent { text = space + "Clear Color" });
                }
                EditorGUILayout.PropertyField(UpVector);
                //EditorGUILayout.PropertyField(TextureLODLevel);
                EditorGUILayout.PropertyField(EnableDepth);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();



            EditorGUILayout.Space();
            MenuCamera = EditorGUILayout.BeginFoldoutHeaderGroup(MenuCamera, new GUIContent { text = "Camera" });
            if (MenuCamera)
            {
                EditorGUILayout.PropertyField(HDR);
                EditorGUILayout.PropertyField(MSAALevel);
                EditorGUILayout.PropertyField(FilterMode);
                EditorGUILayout.PropertyField(DisablePixelLights);
                EditorGUILayout.PropertyField(Shadow);
                EditorGUILayout.PropertyField(Cull);
                if (Cull.boolValue)
                {
                    EditorGUILayout.PropertyField(CullDistance, new GUIContent { text = space + "Distance" });
                }
                EditorGUILayout.PropertyField(RenderTextureSize);
                if (RenderTextureSize.intValue == (int)Mirror.RenderTextureSize.Manual)
                {
                    EditorGUILayout.PropertyField(ManualSize, new GUIContent { text = space + "Size" });
                }
                EditorGUILayout.PropertyField(ClipPlaneOffset);
                EditorGUILayout.PropertyField(CameraLODLevel, new GUIContent { text = "LOD Level" });
                EditorGUILayout.PropertyField(ReflectLayers);

            }
            EditorGUILayout.EndFoldoutHeaderGroup();



            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(CustomShaders);



            if (GUILayout.Button("Apply Changes"))
                (target as MirrorManager).InitializeMirror();



            serializedObject.ApplyModifiedProperties();
        }

    }

}