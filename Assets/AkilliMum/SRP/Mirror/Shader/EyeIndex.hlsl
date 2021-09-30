#ifndef EYE_INDEX_INCLUDED
#define EYE_INDEX_INCLUDED

void Index_float (in float2 screenUV, out float Eye, out float2 newScreenUV){

    Eye = 0.;
    newScreenUV = screenUV;

    #if defined(UNITY_STEREO_INSTANCING_ENABLED) 

        Eye = unity_StereoEyeIndex;

    #elif defined(UNITY_SINGLE_PASS_STEREO)

        Eye = unity_StereoEyeIndex;

        // If Single-Pass Stereo mode is active, transform the
        // coordinates to get the correct output UV for the current eye.
    /*    float4 scaleOffset = unity_StereoScaleOffset[Eye];
        newScreenUV = (screenUV - scaleOffset.zw) / scaleOffset.xy;*/

    #else
        // When not using single pass stereo rendering, eye index must be determined by testing the
        // sign of the horizontal skew of the projection matrix.
        //on quest2 IPD-1-2 and 3 gives different Eye index! what the?
       /* #if UNITY_UV_STARTS_AT_TOP
            if (unity_CameraProjection[0][2] > 0) {
                Eye = 0.;
            }
            else {
                Eye = 1.;
            }
        #else
            if (unity_CameraProjection[0][2] > 0) {
                Eye = 1.;
            }
            else {
                Eye = 0.;
            }
        #endif*/

        //so this seems giving correct eye index on quest2!
        Eye = unity_StereoEyeIndex;

    #endif
}

#endif
