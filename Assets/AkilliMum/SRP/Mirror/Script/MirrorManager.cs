//#define DEBUG_RENDER
//#define DEBUG_LOG
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.XR;

// ReSharper disable once CheckNamespace
namespace AkilliMum.SRP.Mirror
{
    [ExecuteAlways]
    public class MirrorManager : MonoBehaviour
    {
#if DEBUG_RENDER
        [Tooltip("Debug texture to visualize output (reflection) on a UI-texture etc.")]
        public UnityEngine.UI.RawImage DebugImage;
        public UnityEngine.UI.RawImage DebugImageDepth;
#endif

        [Tooltip("Please use this to enable/disable the script. DO NOT USE script's enable/disable check box!")]
        public bool IsEnabled = true;
        [Tooltip("You can name your script for to understand what is it for. Because if you use a lot of reflection scripts, you can forget which one is which :)")]
        public string Name;
        [Tooltip("Please use this to enable/disable the mirror in mirror effect. Use careful for performance reasons!")]
        public bool IsMirrorInMirror = false;
        [Tooltip("Please use this to give unique id's to mirrors which will be drawn together. So if you want to see a mirror inside another mirror, their id must be same!")]
        public string MirrorInMirrorId;
        [Tooltip("Check this if you suffer reflection glitches. Because camera may occlude some objects according to unity settings!")]
        public bool TurnOffOcclusion = false;
        //[Tooltip("Use object bounds for viewport? Use this if you have only one mirror object attached to the script.")]
        //public bool UseObjectBoundsForViewport = false;

        //[Header("Platform (Stand-Alone, VR, AR)")]
        [Tooltip("Please select the correct platform, Normal for stand alone like desktop or mobile etc., VR for virtual reality, AR for augmented reality!")]
        public Platform Platform = Platform.StandAlone;


        //[Header("Common")]
        [Tooltip("'Reflect' (mirror, reflective surface, transparent glass etc.) and 'Transparent' (transparent AR surface) is supported only right now.")]
        public WorkingType WorkingType = WorkingType.Reflect;
        [Tooltip("Clear color for transparency. It may create good visuals if you wanna blur a transparent object with that color!")]
        public Color TransparencyClearColor = new Color(0, 0, 0, 0);
        [Tooltip("The mirror object's normal direction. Most of the time default 'GreenY' works perfectly. But try others if you get strange behavior.")]
        public UpVector UpVector = UpVector.GreenY;
        [Range(0, 10)]
        [Tooltip("Starts to drawing at this level of LOD. So if you do not creating perfect mirrors, you can use lower lod for surface and gain fps.")]
        public int CameraLODLevel = 0;
        //[Range(0, 10)]
        //[Tooltip("Creates the mip maps of the texture and uses the Xth value, you can use it specially for blur effects.")]
        //public int TextureLODLevel = 0;

        //[Header("Camera")]
        [Tooltip("Enables the HDR, so post effects will be visible (like bloom) on the reflection.")]
        public bool HDR = false;
        [Tooltip("Enables the anti aliasing (if only enabled in the project settings) for reflection. May decrease the performance, use with caution!")]
        public MSAALevel MSAALevel = MSAALevel.None;
        [Tooltip("Filter mode to render texture (transparent render should render to Point!).")]
        public FilterMode FilterMode = FilterMode.Bilinear;
        [Tooltip("Disables the point and spot lights for the reflection. You may gain the fps, but you will lose the reality of reflection.")]
        public bool DisablePixelLights = false;
        //private IList<SceneLights> _additionalLights;
        [Tooltip("Enables the shadow on reflection. If you disable it; you may gain the fps, but you will lose the reality of reflection.")]
        public bool Shadow = false;
        //[Range(0, float.MaxValue)]
        //public float ShadowDistance = 0; //todo: shadow distance
        [Tooltip("Enables the culling distance calculations.")]
        public bool Cull = false;
        [Tooltip("Cull circle distance, so it just draws the objects in the distance. You may gain the fps, but you will lose the reality of reflection.")]
        public float CullDistance = 0;
        [Tooltip("Easy selection for reflection quality. Select 'Full' for perfect reflections! VR can render half; so select x2 etc. and try to find the best visual!")]
        public RenderTextureSize RenderTextureSize = RenderTextureSize.Manual;
        [Tooltip("The size (quality) of the reflection if manual is selected above! It should be set to width of the screen for perfect reflection! But try lower values to gain fps.")]
        public double ManualSize = 256;
        [Tooltip("Clipping distance to draw the reflection X units from the surface.")]
        public float ClipPlaneOffset = 0.02f;
        [Tooltip("Only these layers will be rendered by the reflection. So you can select what to be reflected with the reflection by putting them on certain layers.")]
        public LayerMask ReflectLayers = -1;
       
        //[Header("Affected Objects and Materials")]
        [Tooltip("Reflective surfaces (gameObjects) must be put here! Script calculates the reflection according to their position etc. and uses their material.")]
        public GameObject[] ReflectiveObjects;

        //[Header("Simple Depth")]
        [Tooltip("Enables depth render (texture), so shader can use some depth based effects.")]
        public bool EnableDepth = false;

        //[Header("Extend")]
        [Tooltip("You can add your custom shader paths here (like 'Shader Graphs/MyShader') and use mirror texture on your own shader!")]
        public string[] CustomShaders;

#if (VIVE_STEREO_STEAMVR)
        SteamVRParams _steamVRParams = new SteamVRParams();
#endif
#if (VIVE_STEREO_WAVEVR)
        WaveVRParams _waveVRParams = new WaveVRParams();
#endif

#if DEBUG_RENDER
        private float _deltaTime = 0.0f;
#endif

        [NonSerialized]
        public Camera _camera; //srp cam
        [NonSerialized]
        public ScriptableRenderContext _context;

        public CameraManager _cameraManager;
        public RendererManager _rendererManager;
        public RenderTextureManager _renderTextureManager;
        public OptionManager _optionManager;


        // ReSharper disable once UnusedMember.Global
        protected virtual void OnEnable()
        {
            //Debug.Log("Base on enable");

            InitializeMirror();
        }

        public void InitializeMirror()
        {
            //destroy previous ones if any
            _cameraManager?.Destroy();
            _rendererManager?.Destroy();
            _renderTextureManager?.Destroy();
            _optionManager?.Destroy();

            _cameraManager = new CameraManager(GetCameraSettings());
            _rendererManager = new RendererManager(ReflectiveObjects, GetRendererSettings());
            _renderTextureManager = new RenderTextureManager(GetRenderTextureSettings());
            _optionManager = new OptionManager(GetOptionSettings());

            //ugly hack to not register 2 times :)
            RenderPipelineManager.beginCameraRendering -= ExecuteBeforeCameraRender;
            RenderPipelineManager.beginCameraRendering += ExecuteBeforeCameraRender;
        }

        RenderTextureManager.RenderTextureSettings GetRenderTextureSettings()
        {
            return new RenderTextureManager.RenderTextureSettings
            {
                WorkingType = WorkingType,
                Size = RenderTextureSize,
                //LODLevel = TextureLODLevel,
                ManualSize = ManualSize,
                HDR = HDR,
                MSAALevel = MSAALevel,
                FilterMode = FilterMode
            };
        }

        RendererManager.RendererSettings GetRendererSettings()
        {
            return new RendererManager.RendererSettings
            {
                UpVector = UpVector,
                CustomShaders = CustomShaders?.ToList()
            };
        }

        CameraManager.CameraSettings GetCameraSettings()
        {
            return new CameraManager.CameraSettings
            {
                WorkingType = WorkingType,
                HDR = HDR,
                MSAALevel = MSAALevel,
                Platform = Platform,
                TransparencyClearColor = TransparencyClearColor,
                TurnOffOcclusion = TurnOffOcclusion,
                Cull = Cull,
                CullDistance = CullDistance,
                ReflectLayers = ReflectLayers,
                Shadow = Shadow,
                ClipPlaneOffset = ClipPlaneOffset,
                EnableDepth = EnableDepth,
            };
        }

        OptionManager.OptionSettings GetOptionSettings()
        {
            return new OptionManager.OptionSettings
            {
                LODLevel = CameraLODLevel,
                DisablePixelLights = DisablePixelLights
            };
        }


        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
#if DEBUG_RENDER
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
#endif
        }

        // ReSharper disable once UnusedMember.Local
        void OnGUI()
        {
#if DEBUG_RENDER
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 25);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 25;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
#endif
        }

        private void ExecuteBeforeCameraRender(
           ScriptableRenderContext context,
           Camera cameraSrp)
        {
            if(_camera == null)
            {
                _camera = GetComponent<Camera>();
                if (_camera == null)
                    _camera = Camera.main;
            }
            //_camera = cameraSrp;

            _context = context;

            if (IsMirrorInMirror) //camera shade multi manager will handle the events!!
                return;

            RenderReflective(null);
        }
        
        public virtual IList<Camera> RenderReflective(CameraShadeMultiManager manager, Camera sentCamera = null, bool invert = true, bool renderCam = true)
        {
            var cameras = new List<Camera>();

            //todo: check openxr, it may work different!
            //if (Platform == Platform.OpenXR && Application.isPlaying)
            //{
            //    //_stereoEye = 0;
            //    var camera0 = RenderMe(manager, StereoTargetEyeMask.Left, sentCamera, invert, renderCam);
            //    cameras.Add(camera0);

            //    //_stereoEye = 1;
            //    //todo: OPENXR single pass instanced, only one pass is enough???
            //    //var camera1 = RenderMe(manager, StereoTargetEyeMask.Right, sentCamera, invert, renderCam);
            //    //cameras.Add(camera1);
            //}
            if (Platform == Platform.VR && Application.isPlaying)
            {  //draw the scene twice for single pass VR

                //_stereoEye = 0;
                var camera0 = RenderMe(manager, StereoTargetEyeMask.Left, sentCamera, invert, renderCam);
                cameras.Add(camera0);

                //_stereoEye = 1;
                var camera1 = RenderMe(manager, StereoTargetEyeMask.Right, sentCamera, invert, renderCam);
                cameras.Add(camera1);
            }
            else
            {
                //_stereoEye = -1;
                var camera0 = RenderMe(manager, StereoTargetEyeMask.None, sentCamera, invert, renderCam);
                cameras.Add(camera0);
            }

            return cameras;
        }

        private Camera GetCamera(Camera sentCamera)
        {
            var cameraToUse = sentCamera; // ?? _camera;

            if (!cameraToUse || cameraToUse == null)
                cameraToUse = _camera;

            return cameraToUse;
        }

        public virtual void CommonRender(
           ScriptableRenderContext context,
           Camera cameraSrp)
        {
            //common logic to call from inherited codes
        }

        private Camera RenderMe(CameraShadeMultiManager manager, StereoTargetEyeMask stereoTargetEyeMask, Camera sentCamera, bool invert = true, bool renderCam = true)
        {
            var cameraToUse = GetCamera(sentCamera);

            if (!Application.isPlaying || cameraToUse?.cameraType == CameraType.SceneView)
            {
                //ignore, render on scene view
            }
            //do not render if not enabled or not visible
            else if (
                !IsEnabled ||
                //todo: reflection probe baking?
                //cameraToUse.cameraType == CameraType.Reflection ||
                //cameraToUse.cameraType == CameraType.Preview ||
                !_rendererManager.IsObjectVisible(cameraToUse) ||
                //todo: solve below problem
                Time.frameCount < 10 //it may run before the screen initialized, render textures maybe created in wrong dimension!
                                     //screen.height returns 30 etc.sometimes (related to camera initialization, this may run before main cam initalized
                                     //and calculated the real dimensions of screen!
            )
                return null;

            CommonRender(_context, _camera);

            try
            {
                _optionManager.Start();

                //use this cam as ref afterwards
                _cameraManager.SetReferenceCamera(cameraToUse);

                //create mirror cam and set options etc.
                _cameraManager.CreateMirrorCamera();

                //create textures if necessary
                _renderTextureManager.CreateRenderTextures();

                //draw scene
                _cameraManager.Draw(_rendererManager, _renderTextureManager,
                    _context, manager, stereoTargetEyeMask, invert, renderCam);

                //set materials if necessary
                _rendererManager.UpdateMaterials(_renderTextureManager);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            finally
            {
                _optionManager.End();
            }

#if DEBUG_RENDER
            if (DebugImage)
                DebugImage.texture = _renderTextureManager.GetLeftOrCenterRenderTexture();
            if (DebugImageDepth)
                DebugImageDepth.texture = _renderTextureManager.GetLeftOrCenterDepthTexture();
#endif

            return _cameraManager.GetMirrorCamera();
        }

        // ReSharper disable once UnusedMember.Global
        protected virtual void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= ExecuteBeforeCameraRender;

            _renderTextureManager?.Destroy();
            _cameraManager?.Destroy();
        }
    }

    public enum UpVector
    {
        None = 0,
        RedX = 1,
        RedX_Negative = 4,
        GreenY = 2,
        GreenY_Negative = 5,
        BlueZ = 3,
        BlueZ_Negative = 6
    }

    public enum MSAALevel
    {
        None = 0,
        X2 = 2,
        X4 = 4,
        X8 = 8
    }

    public enum WorkingType
    {
        Reflect = 1,
        //Direct = 20,
        Transparent = 30,
        //Water = 40
    }

    public enum RenderTextureSize
    {
        Manual = 0,
        x4 = 6,
        x2 = 5,
        Full = 1,
        Half = 2,
        Quarter = 4
    }

    public enum Platform
    {
        StandAlone = 0,
        VR = 10,
        //OpenXR = 20,
        //Oculus = 30,
        AR = 100
    }

    public class RendererManager : ManagerBase
    {
        private List<Renderer> _renderers;
        private List<Material> _materials;

        private RendererSettings _settings;

        public struct RendererSettings
        {
            public UpVector UpVector { get; set; }
            public IList<string> CustomShaders { get; set; }
        }

        public RendererManager(GameObject[] reflectiveObjects, RendererSettings renderSettings)
        {
            SetSettings(renderSettings); //call first!
            SetReflectiveObjects(reflectiveObjects);
        }

        public void SetSettings(RendererSettings renderSettings)
        {
            _settings = renderSettings;
        }
        
        /// <summary>
        /// Finds the materials to set reflection textures on them. So shader can draw them.
        /// </summary>
        /// <param name="reflectiveObjects"></param>
        public void SetReflectiveObjects(GameObject[] reflectiveObjects)
        {
            _renderers = new List<Renderer>();
            _materials = new List<Material>();

            if (reflectiveObjects != null && reflectiveObjects.Length > 0)
            {
                foreach (var reflectiveObject in reflectiveObjects)
                {
                    if (reflectiveObject != null && reflectiveObject.GetComponent<Renderer>() != null)
                        _renderers.Add(reflectiveObject.GetComponent<Renderer>());
                }
            }
            
            if (_renderers != null)
            {
                foreach (var ren in _renderers)
                {
                    if (ren != null)
                    {
                        foreach (var mat in ren.sharedMaterials)
                        {
                            if (mat != null &&
                                (mat.shader.name == "Shader Graphs/AMMirrorOneEye" ||
                                 mat.shader.name == "Shader Graphs/AMMirrorTwoEyes" ||
                                 //this our hand written old school way shader :)
                                 mat.shader.name == "AkilliMum/URP/Mirrors/Complex"))
                            {
                                _materials.Add(mat);
                            }
                            else if (mat != null &&
                                     _settings.CustomShaders != null && 
                                     _settings.CustomShaders.Contains(mat.shader.name))
                            {
                                Debug.Log("Found custom shader: "+ mat.shader.name);
                                _materials.Add(mat);
                            }
                        }
                    }
                }
            }
        }

        public bool IsObjectVisible(Camera cam)
        {
            var visible = false;
            foreach (var ren in _renderers)
            {
                if (ren.IsVisibleFrom(cam) && ren.gameObject.activeSelf) //if any of renderer is visible
                {
                    visible = true;
                    break;
                }
            }

            return visible;
        }

        public Vector3 GetPosition()
        {
            return _renderers[0].gameObject.transform.position;
        }

        public Vector3 GetNormal(bool invert)
        {
            Vector3 normal;
            if (_settings.UpVector == UpVector.GreenY)
            {
                normal = _renderers[0].gameObject.transform.up; //all items must be on same vector direction :) so we can use first one
            }
            else if (_settings.UpVector == UpVector.GreenY_Negative)
            {
                normal = -_renderers[0].gameObject.transform.up;
            }
            else if (_settings.UpVector == UpVector.BlueZ)
            {
                normal = _renderers[0].gameObject.transform.forward;
            }
            else if (_settings.UpVector == UpVector.BlueZ_Negative)
            {
                normal = -_renderers[0].gameObject.transform.forward;
            }
            else if (_settings.UpVector == UpVector.RedX)
            {
                normal = _renderers[0].gameObject.transform.right;
            }
            else //if (UpVector == UpVector.RedX_Negative)
            {
                normal = -_renderers[0].gameObject.transform.right;
            }
            normal *= (invert ? 1 : -1); //flip normals if drawing reflections reflection :)
            return normal;
        }

        /// <summary>
        /// Updates material properties, it only runs if <see cref="RendererManager.IsDirty"/> is true
        /// </summary>
        /// <param name="renderTextureManager"></param>
        public void UpdateMaterials(RenderTextureManager renderTextureManager)
        {
            //re-run if only class is dirty :)
            if (!IsDirty)
                return;
            IsDirty = false;

            Debug.Log("renderer manager update materials called...");

            foreach (var mat in _materials)
            {
                if (mat.HasProperty("_LeftOrCenterTexture"))
                    mat.SetTexture("_LeftOrCenterTexture", renderTextureManager.GetLeftOrCenterRenderTexture());

                if (mat.HasProperty("_RightTexture"))
                    mat.SetTexture("_RightTexture", renderTextureManager.GetRightRenderTexture());

                if (mat.HasProperty("_LeftOrCenterDepthTexture"))
                    mat.SetTexture("_LeftOrCenterDepthTexture", renderTextureManager.GetLeftOrCenterDepthTexture());

                if (mat.HasProperty("_RightDepthTexture"))
                    mat.SetTexture("_RightDepthTexture", renderTextureManager.GetRightDepthTexture());
            }
        }

        public override void Destroy()
        {

        }
    }

    public abstract class ManagerBase
    {
        private bool _isDirty = true;
        public bool IsDirty
        {
            get => _isDirty;
            set => _isDirty = value;
        }

        public abstract void Destroy();

        public void DestroyObject(UnityEngine.Object obj)
        {
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(obj);
            else
                UnityEngine.Object.Destroy(obj);
        }
    }

    /// <summary>
    /// Manages the render textures
    /// </summary>
    public class RenderTextureManager : ManagerBase
    {
        private RenderTexture _leftOrCenterRT;
        private RenderTexture _rightRT;

        private RenderTexture _leftOrCenterDepthRT;
        private RenderTexture _rightDepthRT;

        private RenderTextureSettings _settings;

        public struct RenderTextureSettings
        {
            public bool HDR { get; set; }
            public RenderTextureSize Size { get; set; }
            public double ManualSize { get; set; }
            public WorkingType WorkingType { get; set; }
            public MSAALevel MSAALevel { get; set; }
            public FilterMode FilterMode { get; set; }
            //public int LODLevel { get; set; }
        }

        public RenderTextureManager(RenderTextureSettings settings)
        {
            SetSettings(settings);
        }

        public void SetSettings(RenderTextureSettings settings)
        {
            _settings = settings;
        }

        public RenderTexture GetLeftOrCenterRenderTexture()
        {
            return _leftOrCenterRT;
        }

        public RenderTexture GetRightRenderTexture()
        {
            return _rightRT;
        }

        public RenderTexture GetLeftOrCenterDepthTexture()
        {
            return _leftOrCenterDepthRT;
        }

        public RenderTexture GetRightDepthTexture()
        {
            return _rightDepthRT;
        }

        /// <summary>
        /// Creates the render textures, looks into the <see cref="RenderTextureManager.IsDirty"/> if recreation must be done
        /// </summary>
        public void CreateRenderTextures()
        {
            //re-run if only class is dirty :)
            if (!IsDirty)
                return;
            IsDirty = false;

            Debug.Log("texture manager create render textures called...");

            //reflectionCamera = null;
            int depth = 24;

            var textureSizes = GetTextureSizes();

            RenderTextureFormat textureFormatHDR;
            RenderTextureFormat textureFormat;

            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
                textureFormatHDR = RenderTextureFormat.ARGBFloat;
            else
                textureFormatHDR = RenderTextureFormat.DefaultHDR;

            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
                textureFormat = RenderTextureFormat.ARGB32;
            else
                textureFormat = RenderTextureFormat.Default;

            Debug.Log("will create render texture sized: width:" + textureSizes[0] + " height: " + textureSizes[1]);

            _leftOrCenterRT = Create(textureSizes[0], textureSizes[1], depth, textureFormat, textureFormatHDR);
            _rightRT = Create(textureSizes[0], textureSizes[1], depth, textureFormat, textureFormatHDR);

            _leftOrCenterDepthRT = CreateDepth(textureSizes[0], textureSizes[1], depth);
            _rightDepthRT = CreateDepth(textureSizes[0], textureSizes[1], depth);
        }

        private RenderTexture Create(int width, int height, int depth, RenderTextureFormat textureFormat, RenderTextureFormat textureFormatHDR)
        {
            RenderTexture renderTexture;
            if (_settings.HDR)
                renderTexture = new RenderTexture(width, height, depth, textureFormatHDR);
            else
                renderTexture = new RenderTexture(width, height, depth, textureFormat);

            renderTexture.name = nameof(renderTexture);

            if((int)_settings.MSAALevel > 0)
                renderTexture.antiAliasing = (int)_settings.MSAALevel;
            renderTexture.filterMode = _settings.FilterMode;
            
            renderTexture.isPowerOfTwo = true;
            renderTexture.hideFlags = HideFlags.DontSave;
            //if (_settings.LODLevel > 0)
            //{
            //    renderTexture.useMipMap = true;
            //    renderTexture.autoGenerateMips = false;
            //}
            //else
            //{
                renderTexture.useMipMap = false;
            //}

            return renderTexture;
        }

        private RenderTexture CreateDepth(int width, int height, int depth)
        {
            RenderTexture renderTexture;
            renderTexture = new RenderTexture(width, height, depth, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear);
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.name = nameof(renderTexture);
            renderTexture.isPowerOfTwo = true;
            renderTexture.hideFlags = HideFlags.DontSave;
            return renderTexture;
        }

        private int[] GetTextureSizes()
        {
            //Calculate the render size
            double width = Screen.width;
            double height = Screen.height;
            Debug.Log("screen width height: " + width + " " + height);

            switch (_settings.Size)
            {
                case RenderTextureSize.Quarter:
                    _settings.ManualSize = width / 4;
                    break;
                case RenderTextureSize.Half:
                    _settings.ManualSize = width / 2;
                    break;
                case RenderTextureSize.Full:
                    _settings.ManualSize = width;
                    break;
                case RenderTextureSize.x2:
                    _settings.ManualSize = width * 2;
                    break;
                case RenderTextureSize.x4:
                    _settings.ManualSize = width * 4;
                    break;
                    //default:
                    //    break; //do not change Manual size
            }

            double textureSize = _settings.ManualSize + _settings.ManualSize % 2; //calculate the width and height according to aspect
            if (textureSize <= 128)
                textureSize = 128;

            //int ops creates 0 (zero)!!!
            double textureSizeHeight = textureSize * (height / width);
            textureSizeHeight = textureSizeHeight + textureSizeHeight % 2;

            return new[] { (int)textureSize, (int)textureSizeHeight };
        }

        public override void Destroy()
        {
            DestroyObject(_leftOrCenterRT);
            _leftOrCenterRT = null;
            DestroyObject(_rightRT);
            _rightRT = null;

            DestroyObject(_leftOrCenterDepthRT);
            _leftOrCenterDepthRT = null;
            DestroyObject(_rightDepthRT);
            _rightDepthRT = null;
        }
    }

    /// <summary>
    /// Manages the reflection camera
    /// </summary>
    public class CameraManager : ManagerBase
    {
        private const string CameraNameStart = "Mirror camera for ";

        private Camera _mainCamera;

        private GameObject _mirrorCameraContainer;
        private Camera _mirrorCamera;

        //private List<XRNodeState> nodeStates = new List<XRNodeState>();

        private CameraSettings _settings;

        public struct CameraSettings
        {
            public WorkingType WorkingType { get; set; }
            public Color TransparencyClearColor { get; set; }
            public Platform Platform { get; set; }
            public bool HDR { get; set; }
            public bool TurnOffOcclusion { get; set; }
            public MSAALevel MSAALevel { get; set; }
            public bool Cull { get; set; }
            public float CullDistance { get; set; }
            public LayerMask ReflectLayers { get; set; }
            public bool Shadow { get; set; }
            public float ClipPlaneOffset { get; set; }
            public bool EnableDepth { get; set; }
        }

        public CameraManager(CameraSettings settings)
        {
            _settings = settings;
        }

        public void SetSettings(CameraSettings settings)
        {
            _settings = settings;
        }

        public void SetReferenceCamera(Camera camera)
        {
            //so we will create mirror cam
            if (_mainCamera is null ||
                _mirrorCamera is null ||
                _mirrorCamera.name != CameraNameStart + camera.GetInstanceID())
            {
                _mainCamera = camera;
                IsDirty = true;
            }
        }

        public Camera GetMirrorCamera()
        {
            return _mirrorCamera;
        }

        /// <summary>
        /// Creates the reflection camera and returns it, recreation is done only if <see cref="CameraManager.IsDirty"/> is true
        /// </summary>
        /// <returns></returns>
        public void CreateMirrorCamera()
        {
            //re-run if only class is dirty :)
            if (!IsDirty)
                return;
            IsDirty = false;

            Debug.Log("camera manager create mirror camera called...");

            _mirrorCameraContainer = new GameObject("Mirror object for " + _mainCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
            _mirrorCamera = _mirrorCameraContainer.GetComponent<Camera>();
            _mirrorCamera.name = CameraNameStart + _mainCamera.GetInstanceID();
            _mirrorCamera.enabled = false;
            //todo: position rotation needed here?
            //reflectionCamera.transform.position = transform.position;
            //reflectionCamera.transform.rotation = transform.rotation;
            _mirrorCamera.gameObject.AddComponent<FlareLayer>();
            _mirrorCameraContainer.hideFlags = HideFlags.HideAndDontSave;

            //try to get camData
            var urpCamData = _mirrorCamera.gameObject.GetComponent<UniversalAdditionalCameraData>();
            //if it is not added before, add
            if (urpCamData == null)
            {
                urpCamData = _mirrorCamera.gameObject.AddComponent(typeof(UniversalAdditionalCameraData)) as UniversalAdditionalCameraData;
            }
            if (urpCamData != null)
            {
                urpCamData.requiresColorOption = CameraOverrideOption.Off;
                urpCamData.requiresDepthOption = CameraOverrideOption.Off;
                urpCamData.renderShadows = _settings.Shadow; // turn on-off shadows for the reflection camera
            }

            UpdateCameraModes();
        }

        private void UpdateCameraModes()
        {
            if (_settings.WorkingType == WorkingType.Transparent)
            {
                //Because on full transparency we make transparent black (0,0,0,1) pixels! And no MSAA must be on!
                _mirrorCamera.clearFlags = CameraClearFlags.Color;
                _mirrorCamera.backgroundColor = _settings.TransparencyClearColor; // new Color(1, 1, 1, 0); //we will use that to clear background on shader
            }
            else if (_settings.Platform == Platform.AR)
            {
                _mirrorCamera.clearFlags = CameraClearFlags.Skybox;
                _mirrorCamera.backgroundColor = _mainCamera.backgroundColor;
            }
            else
            {
                _mirrorCamera.clearFlags = _mainCamera.clearFlags;//CameraClearFlags.SolidColor; // src.clearFlags;
                _mirrorCamera.backgroundColor = _mainCamera.backgroundColor;
                //todo: test
                if (_mainCamera.clearFlags == CameraClearFlags.Skybox)
                {
                    Skybox sky = _mainCamera.GetComponent(typeof(Skybox)) as Skybox;
                    Skybox mysky = _mainCamera.GetComponent(typeof(Skybox)) as Skybox;
                    if (mysky != null)
                    {
                        if (!sky || !sky?.material)
                        {
                            mysky.enabled = false;
                        }
                        else
                        {
                            mysky.enabled = true;
                            mysky.material = sky?.material;
                        }
                    }
                }
            }

            // update other values to match current camera.
            // even if we are supplying custom camera&projection matrices,
            // some of values are used elsewhere (e.g. skybox uses far plane)
            //todo: manuel clip planes? it does not work actually, cause view matrix etc. changes them
            _mirrorCamera.nearClipPlane = _mainCamera.nearClipPlane;
            _mirrorCamera.farClipPlane = _mainCamera.farClipPlane;

            _mirrorCamera.orthographic = _mainCamera.orthographic;
            _mirrorCamera.orthographicSize = _mainCamera.orthographicSize;

            //todo: test this!
            //if (_settings.Platform != Platform.GeneralVR)
            _mirrorCamera.fieldOfView = _mainCamera.fieldOfView;

            if (_settings.EnableDepth)
            {
                _mirrorCamera.depthTextureMode = DepthTextureMode.Depth;
            }

            _mirrorCamera.aspect = _mainCamera.aspect;
            _mirrorCamera.renderingPath = _mainCamera.renderingPath;
            _mirrorCamera.allowHDR = _settings.HDR;
            
            _mirrorCamera.allowMSAA = (int)_settings.MSAALevel > 0;
            
            _mirrorCamera.useOcclusionCulling = !_settings.TurnOffOcclusion;
            _mirrorCamera.cameraType = _mainCamera.cameraType;

            //set cull distance if selected
            if (_settings.Cull)
            {
                float[] distances = new float[32]; //for all layers :)
                for (int i = 0; i < distances.Length; i++)
                {
                    distances[i] = _settings.Cull ? _settings.CullDistance : _mirrorCamera.farClipPlane; //the culling distance
                }
                _mirrorCamera.layerCullDistances = distances;
                _mirrorCamera.layerCullSpherical = _settings.Cull;
            }

            //_mirrorCamera.cullingMask = ~(1 << 4) & _settings.ReflectLayers.value; // never render water layer
            _mirrorCamera.cullingMask = _settings.ReflectLayers.value;
        }

        // Calculates reflection matrix around the given plane
        private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
            reflectionMat.m01 = (-2F * plane[0] * plane[1]);
            reflectionMat.m02 = (-2F * plane[0] * plane[2]);
            reflectionMat.m03 = (-2F * plane[3] * plane[0]);

            reflectionMat.m10 = (-2F * plane[1] * plane[0]);
            reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
            reflectionMat.m12 = (-2F * plane[1] * plane[2]);
            reflectionMat.m13 = (-2F * plane[3] * plane[1]);

            reflectionMat.m20 = (-2F * plane[2] * plane[0]);
            reflectionMat.m21 = (-2F * plane[2] * plane[1]);
            reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
            reflectionMat.m23 = (-2F * plane[3] * plane[2]);

            reflectionMat.m30 = 0F;
            reflectionMat.m31 = 0F;
            reflectionMat.m32 = 0F;
            reflectionMat.m33 = 1F;
        }

        // Given position/normal of the plane, calculates plane in camera space.
        private Vector4 CameraSpacePlane(Matrix4x4 worldToCameraMatrix, Vector3 pos, Vector3 normal, float sideSign)
        {
            Vector3 offsetPos = pos + normal * _settings.ClipPlaneOffset;
            Vector3 cpos = worldToCameraMatrix.MultiplyPoint(offsetPos);
            Vector3 cnormal = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
        }

        private void MakeProjectionMatrixOblique(ref Matrix4x4 matrix, Vector4 clipPlane)
        {
            Vector4 q;

            // Calculate the clip-space corner point opposite the clipping plane
            // as (sgn(clipPlane.x), sgn(clipPlane.y), 1, 1) and
            // transform it into camera space by multiplying it
            // by the inverse of the projection matrix

            q.x = (sgn(clipPlane.x) + matrix[8]) / matrix[0];
            q.y = (sgn(clipPlane.y) + matrix[9]) / matrix[5];
            q.z = -1.0F;
            q.w = (1.0F + matrix[10]) / matrix[14];

            // Calculate the scaled plane vector
            Vector4 c = clipPlane * (2.0F / Vector3.Dot(clipPlane, q));

            // Replace the third row of the projection matrix
            matrix[2] = c.x;
            matrix[6] = c.y;
            matrix[10] = c.z + 1.0F;
            matrix[14] = c.w;
        }

        // Extended sign: returns -1, 0 or 1 based on sign of a
        private float sgn(float a)
        {
            if (a > 0.0f) return 1.0f;
            if (a < 0.0f) return -1.0f;
            return 0.0f;
        }


        private List<XRNodeState> nodeStates = new List<XRNodeState>();
        public void Draw(RendererManager rendererManager,
            RenderTextureManager renderTextureManager,
            ScriptableRenderContext context,
            CameraShadeMultiManager manager, StereoTargetEyeMask stereoTargetEyeMask, bool invert, bool renderCam)
        {
            //Debug.Log("Drawing frame: " + Time.frameCount);

            //todo: is it necessary for none-VR?
            //_mirrorCamera.stereoTargetEye = stereoTargetEyeMask;
            //_mainCamera.stereoTargetEye = stereoTargetEyeMask; //todo: needed?

            // find out the reflection plane: position and normal in world space
            Vector3 pos = rendererManager.GetPosition();
            Vector3 normal = rendererManager.GetNormal(invert);

            // Reflect camera around reflection plane
            float d = -Vector3.Dot(normal, pos) - _settings.ClipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

            Matrix4x4 reflection = Matrix4x4.zero;
            CalculateReflectionMatrix(ref reflection, reflectionPlane);

            Matrix4x4 worldToCameraMatrix = _mainCamera.worldToCameraMatrix * reflection;
            if (stereoTargetEyeMask != StereoTargetEyeMask.None)
            {
                //Thanks to "SJAM" :)
                //Debug.Log("_mainCamera.stereoSeparation: "+ _mainCamera.stereoSeparation);
                //float fix = _mainCamera.stereoSeparation != 0 ? _mainCamera.stereoSeparation + 0.01F : .032F; //for normal 64 mm IPD
                //float fix = _mainCamera.stereoSeparation != 0 ? _mainCamera.stereoSeparation + 0.012F : .034F; //for wide 68 mm IPD
                //float fix = _mainCamera.stereoSeparation != 0 ? _mainCamera.stereoSeparation + 0.007F : .029F; //for narrow 64 mm IPD
                //default IPD is 0.064 for the Unity, so we add half of it to fix camera matrix, but is that true on all cases?
                //stereoseperation is coming 0.022 for unity cam! what the :)
                //for IPD 64 is fairly right for middle seperation, but for narrow or wide seperation fix value should be set slightly
                //different, but unity is not giving me that!
                //IPD Range   Lens Spacing Setting
                //61 mm or smaller    1(narrowest, 58 mm)
                //61 mm to 66 mm  2(middle, 63mm)
                //66mm or larger  3(widest, 68mm)

                //todo: move to new nodelist
                var distance = Vector3.Distance(InputTracking.GetLocalPosition(XRNode.LeftEye),
                    InputTracking.GetLocalPosition(XRNode.RightEye));
                //so our fix should be half of this separation! //todo? is that breaks performance?
                var fix = distance / 2;

                worldToCameraMatrix[12] += stereoTargetEyeMask == StereoTargetEyeMask.Left ? fix : -fix;




                //Quaternion eyeRotation;
                //InputTracking.GetNodeStates(nodeStates);
                //if (stereoTargetEyeMask == StereoTargetEyeMask.Left)
                //{
                //    var state = nodeStates.FirstOrDefault(node => node.nodeType == XRNode.LeftEye);
                //    state.TryGetRotation(out eyeRotation);
                //}
                //else
                //{
                //    var state = nodeStates.FirstOrDefault(node => node.nodeType == XRNode.RightEye);
                //    state.TryGetRotation(out eyeRotation);
                //}
                //_mirrorCamera.transform.rotation = eyeRotation; //!!!
            }

            _mirrorCamera.worldToCameraMatrix = worldToCameraMatrix;
            
            // Setup oblique projection matrix so that near plane is our reflection
            // plane. This way we clip everything below/above it for free.
            Vector4 clipPlane = CameraSpacePlane(worldToCameraMatrix, pos, normal, invert ? 1.0f : -1.0f);

            Matrix4x4 projectionMatrix = _mainCamera.projectionMatrix;
            if (stereoTargetEyeMask != StereoTargetEyeMask.None)
            {
                projectionMatrix = _mainCamera.GetStereoProjectionMatrix(
                    stereoTargetEyeMask == StereoTargetEyeMask.Left
                        ? Camera.StereoscopicEye.Left
                        : Camera.StereoscopicEye.Right);
            }

            if (_mirrorCamera.orthographic)
            {
                projectionMatrix = _mainCamera.CalculateObliqueMatrix(clipPlane);
            }
            else
            {
                MakeProjectionMatrixOblique(ref projectionMatrix, clipPlane);
            }
            _mirrorCamera.projectionMatrix = projectionMatrix;
            
            var oldInvertCulling = GL.invertCulling;
            GL.invertCulling = invert;
            
            if (renderCam)
            {
                //set targets
                if (stereoTargetEyeMask != StereoTargetEyeMask.Right)
                    _mirrorCamera.targetTexture = renderTextureManager.GetLeftOrCenterRenderTexture();
                else
                    _mirrorCamera.targetTexture = renderTextureManager.GetRightRenderTexture();
                //_mirrorCamera.stereoSeparation = 0.064F;
                UniversalRenderPipeline.RenderSingleCamera(manager == null ? context : manager._context, _mirrorCamera);

                if (_settings.EnableDepth)
                {
                    //set targets
                    if (stereoTargetEyeMask != StereoTargetEyeMask.Right)
                        _mirrorCamera.targetTexture = renderTextureManager.GetLeftOrCenterDepthTexture();
                    else
                        _mirrorCamera.targetTexture = renderTextureManager.GetRightDepthTexture();
                    //_mirrorCamera.stereoSeparation = 0.064F;
                    UniversalRenderPipeline.RenderSingleCamera(manager == null ? context : manager._context, _mirrorCamera);
                }
            }

            GL.invertCulling = oldInvertCulling;
        }

        public override void Destroy()
        {
            _mirrorCamera = null;
            _mirrorCameraContainer = null;
        }
    }

    public class OptionManager : ManagerBase
    {
        private bool _previousFog;
        private int _previousLODLevel;
        private int _previousMaxAdditionalLightsCount;
        private OptionSettings _settings;

        public struct OptionSettings
        {
            public int LODLevel { get; set; }
            public bool DisablePixelLights { get; set; }
        }

        public OptionManager(OptionSettings settings)
        {
            SetSettings(settings);
        }

        private void SetSettings(OptionSettings settings)
        {
            _settings = settings;
        }

        public void Start()
        {
            //Debug.Log("Option manager start called...");

            //do not draw fog
            _previousFog = RenderSettings.fog;
            RenderSettings.fog = false;
            
            // Optionally disable pixel lights for reflection/refraction
            if (_settings.DisablePixelLights)
            {
                //save previous
                _previousMaxAdditionalLightsCount =
                    ((UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset).maxAdditionalLightsCount;
                //disable
                ((UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset).maxAdditionalLightsCount = 0;
            }

            //set lod level
            _previousLODLevel = QualitySettings.maximumLODLevel;
            QualitySettings.maximumLODLevel = _settings.LODLevel;
        }

        public void End()
        {
            //Debug.Log("Option manager end called...");

            RenderSettings.fog = _previousFog;

            if (_settings.DisablePixelLights)
            {
                //revert
                ((UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset).maxAdditionalLightsCount =
                    _previousMaxAdditionalLightsCount;
            }

            QualitySettings.maximumLODLevel = _previousLODLevel;
        }

        public override void Destroy()
        {

        }
    }
}