﻿Shader "AkilliMum/URP/Mirrors/2019"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0
        
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
        
        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        _ReceiveShadows("Receive Shadows", Float) = 1.0
        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
        
        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
        [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
        [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0
        
        
        
        
        //new values
        [HideInInspector]_ReflectionTex("Reflection", 2D) = "white" { } //left or all
        [HideInInspector]_ReflectionTexOther("ReflectionOther", 2D) = "white" { } //right
        [HideInInspector]_ReflectionIntensity("Reflection Intensity", Float) = 0.5
        [HideInInspector]_LODLevel("LOD Level", Float) = 0.
        //[HideInInspector]_WetLevel("WetLevel", Float) = 0.
        [HideInInspector]_WorkType("Work Type", Float) = 1.
        [HideInInspector]_DeviceType("Device Type", Float) = 1.
        [HideInInspector]_MixBlackColor("Mix Black Color", Float) = 0.

        [HideInInspector]_EnableRefraction("Enable Refraction", Float) = -1.0
        [HideInInspector]_RefractionTex("Refraction", 2D) = "bump" {}
        [HideInInspector]_ReflectionRefraction("Reflection Refraction", Float) = 0.0
        
        [HideInInspector]_EnableDepthBlur("Enable Depth Blur", Float) = -1.0
        
        [HideInInspector]_EnableSimpleDepth("Enable Simple Depth", Float) = -1.0
        [HideInInspector]_SimpleDepthCutoff("Simple Depth Cutoff", Float) = 0.5
        //[HideInInspector]_DepthBlur("Depth Blur", Float) = 1
        [HideInInspector]_ReflectionTexDepth("ReflectionDepth", 2D) = "white" { } //left or all
        [HideInInspector]_ReflectionTexOtherDepth("ReflectionOtherDepth", 2D) = "white" { } //right
        [HideInInspector]_NearClip("Near Clip", Float) = 0.3
        [HideInInspector]_FarClip("Far Clip", Float) = 1000
        
        [HideInInspector]_EnableMask("Enable Mask", Float) = -1.0
        [HideInInspector]_MaskTex("Mask", 2D) = "white" {}
        [HideInInspector]_MaskCutoff("Mask Cutoff", Float) = 0.5
        [HideInInspector]_MaskEdgeDarkness("Mask Edge Darkness", Float) = 1.0
        [HideInInspector]_MaskTiling("Mask Tiling", Vector) = (1,1,1,1)
        
        [HideInInspector]_EnableWave("Enable Waves", Float) = -1.0
        [HideInInspector]_WaveNoiseTex("Wave Noise Tex", 2D) = "white" {}
        [HideInInspector]_WaveSize("Wave Size", float) = 12.0
        [HideInInspector]_WaveDistortion("Wave Distortion", Float) = 0.02
        [HideInInspector]_WaveSpeed("Wave Speed", Float) = 3.0
        
        [HideInInspector]_EnableRipple("Enable Ripples", Float) = -1.0
        [HideInInspector]_RippleTex("Ripple", 2D) = "bump" {}
        [HideInInspector]_RippleSize("Ripple Size", Float) = 2.0
        [HideInInspector]_RippleRefraction("Ripple Refraction", Float) = 0.02
        [HideInInspector]_RippleDensity("Ripple Density", Float) = 1.0
        [HideInInspector]_RippleSpeed("Ripple Speed", Float) = 0.3

        [HideInInspector]_EnableLocallyCorrection("Enable Locally Correction", Float) = 0.
        [HideInInspector]_BBoxMin("BBox Min", Vector) = (0,0,0,0)
        [HideInInspector]_BBoxMax("BBox Max", Vector) = (0,0,0,0)
        [HideInInspector]_EnviCubeMapPos("CubeMap Pos", Vector) = (0,0,0,0)
    }

    SubShader
    {
        // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Universal Render Pipeline and Builtin Unity Pipeline
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 300

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _FULLMIRROR
            #pragma shader_feature _LOCALLYCORRECTION
            #pragma shader_feature _FULLWATER
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _OCCLUSIONMAP

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "LitInput.hlsl"
            #include "LitForwardPass.hlsl"
            
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "LitInput.hlsl"
            #include "ShadowCasterPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "LitInput.hlsl"
            #include "DepthOnlyPass.hlsl"

            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}
            
            Cull Off
            
            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMeta

            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _FULLMIRROR
            #pragma shader_feature _LOCALLYCORRECTION
            #pragma shader_feature _FULLWATER
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma shader_feature _SPECGLOSSMAP

            #include "LitInput.hlsl"
            #include "LitMetaPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON

            #include "LitInput.hlsl"
            #include "Universal2D.hlsl"

            ENDHLSL
        }


    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "AkilliMum.SRP.Mirror.MirrorEditor"
}
