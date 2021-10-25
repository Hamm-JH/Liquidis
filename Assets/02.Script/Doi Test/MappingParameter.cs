using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;



public class MappingParameter : MonoBehaviour
{
    private static MappingParameter _mappingParameter;
    public static MappingParameter instance
    {
        get
        {
            if (_mappingParameter == null)
                return null;
            else
                return _mappingParameter;
        }
    }

    

    [Header("object")]
    public GameObject previewCube;
    public Material refMaterial;

    [Header("value")]
    // Geometry
    public int geometryType = 0;
    public float geoValue = 0f;

    // Color

    public float color_A_R = 1f;
    public float color_A_G = 1f;
    public float color_A_B = 1f;
    public float color_A_BW = 0f;

    public float color_B_R = 0f;
    public float color_B_G = 0f;
    public float color_B_B = 0f;
    public float color_B_BW = 0f;

    public Color colorA;
    public Color colorB;

    public Color fixedColorA;
    public Color fixedColorB;

    Color lerpColor;

    public float colorValue = 0f;
    public int targetColor = 0;

    // VFX_Texture
    public int vfx_textureType = 0;
    public float vfxValue = 0f;

    // Speed
    public int speedType = 0;
    public float speedValue = 0f;
    public float speedInterval = 3f;
    public float speedLerpInterval = 0f;
    public int speedIntervalType = 0;
    public float speedIntervalAMin = 0.1f;
    public float speedIntervalAMax = 2f;
    public float speedIntervalBMin = 2f;
    public float speedIntervalBMax = 0.1f;




    [Header("General")]
    public int currentOpenMenu = 0;
    public int currentMatchEmotion = 0;
    public GameObject[] menu_UI;
    public int[] matchType;

    public VisualEffect vfxObject;

    public GameObject waitingPreview;
    public GameObject meetingtHead;

    public int playerNum = 0;

    //public bool[] typeUse;
    public Material[] geoMaterials_preview_origin;
    public Material[] geoMaterials_stencil_origin;
    public Material[] geoMaterials_speed_origin;
    public Material[] geoPreviewMaterials;
    public Material[] stencilStencilMaterials;
    public Material[] stencilSpeedMaterials;


    public Material color_preview_origin;
    public Material color_stencil_origin;
    public Material colorStencil;
    public Material colorPreview;
    public Material colorCancelDefault;

    //public Material Color_preview_origin
    //{
    //    get
    //    {
    //        if(color_preview_origin == null)
    //        {
    //            color_preview_origin = Resources.Load<Material>("target");
    //        }
    //        return color_preview_origin;
    //    }
    //}

    public GameObject[] stencilWindows;

    Material targetMaterial;

    public GameObject[] stencilSpheres;

    public enum scene {
        SELECT,
        WAITING,
        MEETING
    };
    public scene currentScene;
   

    [Header("UI")]
    // General
    public Button menu_left_button;
    public Button menu_right_button;
    public Button match_button;
    public Button match_cancel_button;
    public TextMeshProUGUI emotionText;
    public Image emotionSlider;
    public Button ready_button;

    // Geometry
    public Button geo_left_button;
    public Button geo_right_button;
    public Button geo_sub_button;
    public Button geo_add_button;
    public Slider geo_slider;


    // Color
    public Slider color_A_R_slider;
    public Slider color_A_G_slider;
    public Slider color_A_B_slider;
    public Slider color_A_BW_slider;

    public Slider color_B_R_slider;
    public Slider color_B_G_slider;
    public Slider color_B_B_slider;
    public Slider color_B_BW_slider;

    public ColorPicker colorPicker;
    public Button setColorA_button;
    public Button setColorB_button;


    public Button color_sub_button;
    public Button color_add_button;
    public Slider color_slider;


    // VFX_Texture
    public Button vfx_left_button;
    public Button vfx_right_button;
    public Button vfx_sub_button;
    public Button vfx_add_button;
    public Slider vfx_slider;



    // Speed
    public Button speed_left_button;
    public Button speed_right_button;
    public Slider speed_slider;
    public Button speed_sub_button;
    public Button speed_add_button;
    public Button speed_interval_left_button;
    public Button speed_interval_right_button;
    public GameObject[] speed_type_images;
    public GameObject[] speed_interval_images;


    private void Awake()
    {
        if (_mappingParameter == null)
        {
            _mappingParameter = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(_mappingParameter != this)
            {
                Destroy(this.gameObject);
            }
        }

        // �̺�Ʈ�� �޼��� ����
        
    }

    private void Start()
    {

        if(currentScene == scene.SELECT)
        {
            // ��ü �޴� <- ->
            menu_left_button.onClick.AddListener(GeneralMenu_Left);
            menu_right_button.onClick.AddListener(GeneralMenu_Right);

            // Geometry
            geo_left_button.onClick.AddListener(SetGeoType_Left);
            geo_right_button.onClick.AddListener(SetGeoType_Right);
            geo_sub_button.onClick.AddListener(GeoValueAdd);
            geo_add_button.onClick.AddListener(GeoValueSub);


            // geo material �ʱ�ȭ 
            for (int i = 0; i < geoMaterials_preview_origin.Length; i++)
            {
                geoPreviewMaterials[i] = new Material(geoMaterials_preview_origin[i]);
               

            }
            for (int i = 0; i < geoMaterials_stencil_origin.Length; i++)
            {
              
                stencilStencilMaterials[i] = new Material(geoMaterials_stencil_origin[i]);
              

            }
            for (int i = 0; i < geoMaterials_speed_origin.Length; i++)
            {

                stencilSpeedMaterials[i] = new Material(geoMaterials_speed_origin[i]);


            }


            colorStencil = new Material(color_stencil_origin);
            colorPreview = new Material(color_preview_origin);


            previewCube.GetComponent<MeshRenderer>().material = geoPreviewMaterials[geometryType];

            // Color
            color_sub_button.onClick.AddListener(ColorValueSub);
            color_add_button.onClick.AddListener(ColorValueAdd);
            //setColorA_button.onClick.AddListener(SetColorTargetA);
            //setColorB_button.onClick.AddListener(SetColorTargetB);



            // VFX_Texture
            vfx_left_button.onClick.AddListener(ChangeVFXType_Left);
            vfx_right_button.onClick.AddListener(ChangeVFXType_Right);
            vfx_sub_button.onClick.AddListener(VfxValueSub);
            vfx_add_button.onClick.AddListener(VfxValueAdd);


            // Speed
            speed_left_button.onClick.AddListener(SetSpeedType_Left);
            speed_right_button.onClick.AddListener(SetSpeedType_Right);
            speed_sub_button.onClick.AddListener(SpeedValueSub);
            speed_add_button.onClick.AddListener(SpeedValueAdd);
            speed_interval_left_button.onClick.AddListener(SetSpeedIntervalType_Left);
            speed_interval_right_button.onClick.AddListener(SetSpeedIntervalType_Right);

            //previewCube.GetComponent<MeshRenderer>().material = new Material(refMaterial);

            colorA = new Color(1, 1, 1);
            colorB = new Color(0, 0, 0);
            fixedColorA = new Color(0, 0, 0);
            fixedColorB = new Color(0, 0, 0);

            lerpColor = Color.Lerp(colorA, colorB, 0f);



            // ������� ����
            LerpColorSetColor(colorValue);


            // Ÿ�� �̴ϼ�
            matchType = new int[3];
            //typeUse = new bool[4];
            for (int i = 0; i < 3; i++)
            {
                matchType[i] = 0;

            }
            //for (int i = 0; i < 4; i++)
            //{

            //    typeUse[i] = false;
            //}

            // speed interval �ʱ�ȭ
            InitialSpeedIntervalValue();

            // window stencil �ʱ�ȭ
            for (int i = 0; i < stencilWindows.Length; i++)
            {
                stencilWindows[i].GetComponent<Renderer>().material.SetInt("_StencilRef", 0);
            }


            if (matchType[currentOpenMenu] == 0 && !IsMappedEmotion(currentMatchEmotion))
            {
                match_button.gameObject.SetActive(true);
            }

            // initialize vFX
            vfxObject.SetFloat("SphereRadius", 0.3f);
            vfxObject.SetFloat("MinLifeTime", 0.1f);
            vfxObject.SetFloat("MaxLifeTime", 1f);
            vfxObject.SetFloat("ConformSphere", 0.4f);
            vfxObject.SetInt("AnimationCurve", 0);


        }
        else if(currentScene == scene.WAITING)
        {
            // geo material �ʱ�ȭ 
            // �ؾ� �Ǵ���?
            for (int i = 0; i < geoMaterials_stencil_origin.Length; i++)
            {

                stencilStencilMaterials[i] = new Material(geoMaterials_stencil_origin[i]);


            }

            // initialize vFX
            vfxObject.SetFloat("SphereRadius", 0.3f);
            vfxObject.SetFloat("MinLifeTime", 0.1f);
            vfxObject.SetFloat("MaxLifeTime", 1f);
            vfxObject.SetFloat("ConformSphere", 0.4f);
            vfxObject.SetFloat("ParticleSize", 0.1f);
            vfxObject.SetInt("AnimationCurve", 0);

            //waitingPreview.GetComponent<Renderer>().material = stencilStencilMaterials[geometryType];
            //LerpColorWaitingCube(colorValue);
        }
        else if(currentScene == scene.MEETING)
        {
            // geo material �ʱ�ȭ 
            for (int i = 0; i < geoMaterials_preview_origin.Length; i++)
            {
                geoPreviewMaterials[i] = new Material(geoMaterials_preview_origin[i]);


            }

            // initialize vFX
            vfxObject.SetFloat("SphereRadius", 30f);
            vfxObject.SetFloat("MinLifeTime", 5f);
            vfxObject.SetFloat("MaxLifeTime", 10f);
            vfxObject.SetFloat("ConformSphere", 40f);
            vfxObject.SetInt("AnimationCurve", 1);
        }

       






    }

    // ��ü �޴� <- ��ư
    public void GeneralMenu_Left()
    {
        if (currentOpenMenu > 0)
        {
            foreach (GameObject menu in menu_UI)
            {
                menu.SetActive(false);
            }
            currentOpenMenu -= 1;

            menu_UI[currentOpenMenu].SetActive(true);

            if (matchType[currentOpenMenu] == 0)
            {
                match_button.gameObject.SetActive(true);
                match_cancel_button.gameObject.SetActive(false);
            }
            else
            {
                match_button.gameObject.SetActive(false);
                match_cancel_button.gameObject.SetActive(true);

            }
        }
        if (currentOpenMenu == 0)
        {
            previewCube.GetComponent<Renderer>().material = geoPreviewMaterials[geometryType];

        }
        else if (currentOpenMenu == 1)
        {
            previewCube.GetComponent<Renderer>().material = colorPreview;
            LerpColorSetColor(colorValue);

        }else if(currentOpenMenu == 2)
        {
            previewCube.GetComponent<Renderer>().material = geoPreviewMaterials[geometryType];
            SpeedColorSetColor(colorValue);

        }


    }

    // ��ü �޴� -> ��ư
    public void GeneralMenu_Right()
    {
        if (currentOpenMenu < 2)
        {
            foreach (GameObject menu in menu_UI)
            {
                menu.SetActive(false);
            }
            currentOpenMenu += 1;

            menu_UI[currentOpenMenu].SetActive(true);

            if (matchType[currentOpenMenu] == 0)
            {
                match_button.gameObject.SetActive(true);
                match_cancel_button.gameObject.SetActive(false);
            }
            else
            {
                match_button.gameObject.SetActive(false);
                match_cancel_button.gameObject.SetActive(true);

            }
        }

        if (currentOpenMenu == 0)
        {
            previewCube.GetComponent<Renderer>().material = geoPreviewMaterials[geometryType];

        }
        else if (currentOpenMenu == 1)
        {
            previewCube.GetComponent<Renderer>().material = colorPreview;
            LerpColorSetColor(colorValue);

        }
        else if (currentOpenMenu == 2)
        {
            previewCube.GetComponent<Renderer>().material = geoPreviewMaterials[geometryType];
            SpeedColorSetColor(colorValue);

        }

    }

    // CubeMaterial���� ȣ��
    public void SetCurrentEmotion(int num)
    {
        currentMatchEmotion = num;

        if (matchType[currentOpenMenu] == 0)
        {
            match_button.gameObject.SetActive(true);
            match_cancel_button.gameObject.SetActive(false);
        }

        //if(currentMatchEmotion == 1)
        //{
        //    emotionText.text = "����";

        //}else if(currentMatchEmotion == 2)
        //{
        //    emotionText.text = "��/����";

        //}
        //else if(currentMatchEmotion == 3)
        //{
        //    emotionText.text = "���";

        //}
    }

    // match ��ư Ŭ��
    public void MatchButtonClicked()
    {
        

        if (IsMappedEmotion(currentMatchEmotion)== false)
        {
            matchType[currentOpenMenu] = currentMatchEmotion;

            


            switch (currentOpenMenu)
            {
                case 0:
                    targetMaterial = stencilStencilMaterials[geometryType];
                    SetGeoValueToLerp(0, geoValue);
                    break;
                case 1:
                    targetMaterial = colorStencil;
                    SetColorValueToLerp(1, colorValue);
                    FixColor();
                    break;
                case 2:
                    targetMaterial = stencilSpeedMaterials[geometryType];
                    SpeedColorSetColor(colorValue);
                    SelectManager.instance.Request(currentMatchEmotion, speedInterval);
                    //previewCube.GetComponent<Renderer>().material = targetMaterial;
                    break;
               
            }

            for(int i=0; i<3; i++)
            {
                if(i == currentMatchEmotion-1)
                {
                    stencilWindows[i].GetComponent<Renderer>().material.SetInt("_StencilRef", currentOpenMenu + 1);

                }
                else
                {
                    if (matchType[currentOpenMenu] == 0)
                    {
                        stencilWindows[i].GetComponent<Renderer>().material.SetInt("_StencilRef", 0);
                        
                    }
                }
            }

            stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material = targetMaterial;

            if(currentOpenMenu == 1)
            {
                LerpColorStencilSphere(colorValue);
            }else if(currentOpenMenu == 2)
            {
                SpeedColorSetColor(colorValue);
            }

            match_button.gameObject.SetActive(false);
            match_cancel_button.gameObject.SetActive(true);

            emotionSlider.fillAmount += 1f/3f;
        }

        // ��� ��ġ �Ǹ� ���� ��ư Ȱ��ȭ
        if (AllMappedEmotionCheck())
        {
            ready_button.interactable = true;
        }
        else
        {
            ready_button.interactable = false;
        }

    }

    // cancel ��ư Ŭ��
    public void CancelButtonClicked()
    {
        int tempEmotion = matchType[currentOpenMenu];
        matchType[currentOpenMenu] = 0;
        stencilWindows[tempEmotion-1].GetComponent<Renderer>().material.SetInt("_StencilRef", 0);

        // ���ٽ� ����Ʈ material���� �ٲ�
        stencilSpheres[tempEmotion - 1].GetComponent<Renderer>().material = colorCancelDefault;

       


        if (matchType[currentOpenMenu] == 0)
        {
            match_button.gameObject.SetActive(true);
            match_cancel_button.gameObject.SetActive(false);

            emotionSlider.fillAmount -= 1f/3f;

        }
    }

    // select ��� �� �ʱ�ȭ
    public void RemoveWaitingRoomParameters()
    {
        geoValue = 0f;
        colorValue = 0f;
        speedValue = 0f;
        vfxValue = 0f;

    }
    // waiting room ������ �� �ʱ�ȭ
    public void InitialWaitingRoomParameters(GameObject targetSphere, VisualEffect _vfx)
    {
        waitingPreview = targetSphere;
        previewCube = targetSphere;
        vfxObject = _vfx;
        currentScene = scene.WAITING;

        // initialize vFX
        vfxObject.SetFloat("SphereRadius", 0.3f);
        vfxObject.SetFloat("MinLifeTime", 0.1f);
        vfxObject.SetFloat("MaxLifeTime", 1f);
        vfxObject.SetFloat("ConformSphere", 0.4f);
        vfxObject.SetInt("AnimationCurve", 0);

        waitingPreview.GetComponent<Renderer>().material = stencilStencilMaterials[geometryType];
        LerpColorWaitingCube(colorValue);

    }
    // meeting room ������ �� �ʱ�ȭ
    public void InitialMeeintRoomParameters(GameObject _meetinghead, VisualEffect _vfxObj)
    {
        meetingtHead = _meetinghead;
        vfxObject = _vfxObj;
        currentScene = scene.MEETING;

        // initialize cloud unlit
        //geoPreviewMaterials[1].SetFloat("",);

        // initialize vFX
        vfxObject.SetFloat("SphereRadius", 30f);
        vfxObject.SetFloat("MinLifeTime", 5f);
        vfxObject.SetFloat("MaxLifeTime", 10f);
        vfxObject.SetFloat("ConformSphere", 40f);
        vfxObject.SetInt("AnimationCurve", 1);

        meetingtHead.GetComponent<Renderer>().material = geoPreviewMaterials[geometryType];

        geoPreviewMaterials[1].SetFloat("_3dNoiseSizeA", 0.12f);
        geoPreviewMaterials[1].SetVector("_NoiseScaleA", new Vector4(0.49f, 1f, 1f, 0));
        geoPreviewMaterials[1].SetFloat("_NoiseScaleC", 1f);
        geoPreviewMaterials[1].SetFloat("_3dNoiseSizeC", 10f);
        geoPreviewMaterials[1].SetVector("_Tiling", new Vector4(-0.01f, -0.01f, 0, 0));
        

        LerpColorMeetingFace(colorValue);

    }
    //bool CheckTypeUse()
    //{
    //    return typeUse[currentMatchEmotion];
    //}

    // meeting room ������ �� üũ��. LobbyManager.cs
    public bool AllMappedEmotionCheck()
    {
        int count = 0;
        bool result = false;

        for (int i = 0; i < 3; i++)
        {
            if (matchType[i] == 0)
            {
                count++;
            }

        }

        if (count == 0)
            result = true;
        else
            result = false;

        return result;
    }

    bool IsMappedEmotion(int target)
    {
        bool result = false;
        int count = 0;


        for (int i = 0; i < 3; i++)
        {
            if (matchType[i] == target)
            {
                count++;
            }

        }

        if (count == 0)
            result = false;
        else
            result = true;

        return result;
    }

    // Geometry Ÿ�� �ٲٱ� ->��ư
    public void SetGeoType_Right()
    {
        if (geometryType < 2)
        {
            geometryType += 1;
            previewCube.GetComponent<MeshRenderer>().material = geoPreviewMaterials[geometryType];
            
        }
    }

    // Geometry Ÿ�� �ٲٱ� <-��ư

    public void SetGeoType_Left()
    {
        if (geometryType > 0)
        {
            geometryType -= 1;
            previewCube.GetComponent<MeshRenderer>().material = geoPreviewMaterials[geometryType];

        }
    }

    // Geo �����̴� ���� -
    public void GeoValueSub()
    {
        if (geoValue > 0)
        {
            geoValue -= 0.1f;
            geo_slider.value = geoValue;

            if (geometryType == 0)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion-1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
            else if(geometryType == 1)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }

        }
    }

    // Geo �����̴� ���� +
    public void GeoValueAdd()
    {
        if (geoValue < 1f)
        {
            geoValue += 0.1f;
            geo_slider.value = geoValue;

            if (geometryType == 0)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
            else if (geometryType == 1)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
        }
    }
    // Geometry Slider ���� �� �ٲ� ������ ȣ��
    public void SetGeoValue()
    {
        if(currentScene == scene.SELECT)
        {
            geoValue = geo_slider.value;

            if (geometryType == 0)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
            else if (geometryType == 1)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
        }else if(currentScene == scene.WAITING)
        {
            geoValue = geo_slider.value;
     
            if (geometryType == 0)
            {
                waitingPreview.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
            else if (geometryType == 1)
            {
                waitingPreview.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

            }
        }
    }

    // ������ǥ�� ���̴��� ���ν�Ų��
    // value �� ���� ���̴� �� ����
    public void SetGeoValueToLerp(int emotionType, float _value)
    {
        SelectManager.instance.Request(emotionType, _value);
    }

    // ������ǥ�� ���̴��� ���ν�Ų��
    // value �� ���� ���̴� �� ����
    public void SetColorValueToLerp(int emotionType, float _value)
    {
        SelectManager.instance.Request(emotionType, _value);
    }
    // select manager���� ���� �� ���� ���´�
    public void GetGeoValueFromLerp(float _value)
    {
        //geoValue = _value;

        if(currentScene == scene.SELECT)
        {
            if (geometryType == 0)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", _value);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", _value);


            }
            else if (geometryType == 1)
            {
                previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", _value);
                stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", _value);
                //stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_3dNoiseSizeA", 0.12f);
                //stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetVector("_NoiseScaleA", new Vector4(0.49f, 1f, 1f, 0));
                //stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_NoiseScaleC", 1f);
                //stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_3dNoiseSizeC", 10f);
                //stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetVector("_Tiling", new Vector4(-0.01f,-0.01f, 0,0));
               


            }
        }
       
    }
  
   


    public void RecieveGeoValueToSpeed(float _value)
    {
        
        if (geometryType == 0)
        {
            previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
            stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

        }
        else if (geometryType == 1)
        {
            previewCube.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);
            stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

        }
    }


    // meeting room geo head ����
    public void SetGeoValueMeeting(float _value)
    {
        geoValue = _value;
        if (geometryType == 0)
        {
            meetingtHead.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

        }
        else if (geometryType == 1)
        {
            meetingtHead.GetComponent<Renderer>().material.SetFloat("_Noise", geoValue);

        }
    }

    #region ���� �÷� ��Ŀ
    //// Color picker ���� �� �ٲ� �� ȣ��
    //public void SetColor()
    //{
    //    if (targetColor == 0)
    //    {
    //        colorA = colorPicker.pickedColor;
    //        LerpColorSetColor(colorValue);

    //    }
    //    else
    //    {
    //        colorB = colorPicker.pickedColor;
    //        LerpColorSetColor(colorValue);

    //    }
    //}

    //public void SetColorTargetA()
    //{
    //    targetColor = 0;

    //    //�����̴� ��������
    //    colorValue = 0f;
    //    color_slider.value = colorValue;
    //    LerpColorSetColor(colorValue);
    //}
    //public void SetColorTargetB()
    //{
    //    targetColor = 1;

    //    //�����̴� ����������
    //    colorValue = 1f;
    //    color_slider.value = colorValue;
    //    LerpColorSetColor(colorValue);
    //}
    #endregion

    public void SetColor_R_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_R = color_A_R_slider.value;
            colorA.r = color_A_R_slider.value;
            LerpColorSetColor(colorValue);
        }
        else
        {
            color_B_R = color_B_R_slider.value;
            colorB.r = color_B_R_slider.value;
            LerpColorSetColor(colorValue);

        }
    }
    // Color value slider���� �� �ٲ� �� ȣ��
    public void SetColor_G_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_G = color_A_G_slider.value;
            colorA.g = color_A_G_slider.value;
            LerpColorSetColor(colorValue);

        }
        else
        {
            color_B_G = color_B_G_slider.value;
            colorB.g = color_B_G_slider.value;
            LerpColorSetColor(colorValue);

        }
    }
    // Color value slider���� �� �ٲ� �� ȣ��
    public void SetColor_B_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_B = color_A_B_slider.value;
            colorA.b = color_A_B_slider.value;
            LerpColorSetColor(colorValue);
        }
        else
        {
            color_B_B = color_B_B_slider.value;
            colorB.b = color_B_B_slider.value;
            LerpColorSetColor(colorValue);

        }
    }
    //Color value slider���� �� �ٲ� �� ȣ��
    public void SetColor_BW_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_BW = color_A_BW_slider.value;
            colorA.a = color_A_BW_slider.value;
            LerpColorSetColor(colorValue);
        }
        else
        {
            color_B_BW = color_B_BW_slider.value;
            colorB.a = color_B_BW_slider.value;
            LerpColorSetColor(colorValue);
        }
    }

    // Color Slider ���� �� �ٲ� �� ȣ��
    public void SetColorValue()
    {
        if(currentScene == scene.SELECT)
        {
            colorValue = color_slider.value;
            LerpColorSetColor(colorValue);
            LerpColorStencilSphere(colorValue);

        }else if(currentScene == scene.WAITING)
        {
            colorValue = color_slider.value;
            LerpColorWaitingCube(colorValue);
        }

    }

    // Color �����̴� ���� -
    public void ColorValueSub()
    {
        if (colorValue > 0)
        {
            colorValue -= 0.1f;
            color_slider.value = colorValue;
            LerpColorSetColor(colorValue);
            LerpColorStencilSphere(colorValue);


        }
    }
    // Color �����̴� ���� +
    public void ColorValueAdd()
    {
        if (colorValue < 1)
        {
            colorValue += 0.1f;
            color_slider.value = colorValue;
            LerpColorSetColor(colorValue);
            LerpColorStencilSphere(colorValue);

        }
    }

    void FixColor()
    {
        fixedColorA.r = color_A_R;
        fixedColorA.g = color_A_G;
        fixedColorA.b = color_A_B;

        fixedColorB.r = color_B_R;
        fixedColorB.g = color_B_G;
        fixedColorB.b = color_B_B;
    }

    // ������ ť�꿡 ���� ������ �żҵ�
    void LerpColorSetColor(float value)
    {
        lerpColor = Color.Lerp(colorA, colorB, value);
        previewCube.GetComponent<Renderer>().material.SetVector("_AlbedoColor", lerpColor);
    }
    // ������ ť�갡 ���ǵ� �϶� ���� ������ �޼ҵ�
    public void LerpColorSpeedSetColor(float value)
    {
        lerpColor = Color.Lerp(colorA, colorB, value);

        previewCube.GetComponent<Renderer>().material.SetVector("_TextureColor", lerpColor);
    }

    // ���ٽǿ� ���� ������ �޼ҵ� (match ��ư Ŭ��)
    void LerpColorStencilSphere(float value)
    {
        lerpColor = Color.Lerp(colorA, colorB, value);
        //colorStencil.SetFloat("_AlbedoColor", value);
       
        stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetVector("_AlbedoColor", lerpColor);
    }
    // ���ǵ� �� ��� ���ٽ� ���ǵ忡 ���� ������ �޼ҵ�
    public void SpeedColorSetColor(float value)
    {
       
        //Debug.Log("color value : " + value);
        lerpColor = Color.Lerp(fixedColorA, fixedColorB, value);

        if (currentScene == scene.SELECT)
            stencilSpheres[currentMatchEmotion - 1].GetComponent<Renderer>().material.SetVector("_TextureColor", lerpColor);
    }
    // waiting room stencil cube�� ���� ������ �޼ҵ�
    public void LerpColorWaitingCube(float value)
    {
        colorValue = value;
        lerpColor = Color.Lerp(fixedColorA, fixedColorB, value);
        waitingPreview.GetComponent<Renderer>().material.SetVector("_TextureColor", lerpColor);

    }

    // meeting room head�� ���� ������ �޼ҵ�
    public void LerpColorMeetingFace(float _value)
    {
        colorValue = _value;
        lerpColor = Color.Lerp(fixedColorA, fixedColorB, _value);
        meetingtHead.GetComponent<Renderer>().material.SetVector("_TextureColor", lerpColor);

    }

    // vfx type �ٲٴ� ��ư ->
    public void ChangeVFXType_Left()
    {
        if (vfx_textureType < 3)
        {
            vfx_textureType += 1;
        }
    }

    // vfx type �ٲٴ� ��ư <-
    public void ChangeVFXType_Right()
    {
        if (vfx_textureType > 0)
        {
            vfx_textureType -= 1;
        }
    }

    // vfx �����̴� ���� -
    public void VfxValueSub()
    {
        if (vfxValue > 0)
        {
            vfxValue -= 0.1f;
            vfx_slider.value = vfxValue;
            vfxObject.SetFloat("SpawnRate", vfxValue);

        }
    }
    // vfx �����̴� ���� +
    public void VfxValueAdd()
    {
        if (vfxValue < 1)
        {
            vfxValue += 0.1f;
            vfx_slider.value = vfxValue;
            vfxObject.SetFloat("SpawnRate", vfxValue);

        }
    }
    // vfx Slider ���� �� �ٲ� ������ ȣ��
    public void SetVFXValue()
    {
        vfxValue = vfx_slider.value;
        vfxObject.SetFloat("SpawnRate", vfxValue);
    }

    public void SetVFXValueMeeting(float _value)
    {
        vfxValue = _value;
        vfxObject.SetFloat("SpawnRate", vfxValue);

    }
    // speed Ÿ�� �ٲٱ� ->��ư
    public void SetSpeedType_Right()
    {
        if (speedType < 2)
        {
            speedType += 1;

            foreach( GameObject target in speed_type_images)
            {
                target.SetActive(false);
            }

            speed_type_images[speedType].SetActive(true);
        }
    }

    // Speed Ÿ�� �ٲٱ� <-��ư

    public void SetSpeedType_Left()
    {
        if (speedType > 0)
        {
            speedType -= 1;

            foreach (GameObject target in speed_type_images)
            {
                target.SetActive(false);
            }

            speed_type_images[speedType].SetActive(true);
        }
    }

    // speed interval Ÿ�� �ٲٱ� ->��ư
    public void SetSpeedIntervalType_Right()
    {
        if (speedIntervalType < 1)
        {
            speedIntervalType += 1;

            foreach (GameObject target in speed_interval_images)
            {
                target.SetActive(false);
            }

            speed_interval_images[speedIntervalType].SetActive(true);


            InitialSpeedIntervalValue();
        }
    }

    // Speed interval Ÿ�� �ٲٱ� <-��ư

    public void SetSpeedIntervalType_Left()
    {
        if (speedIntervalType > 0)
        {
            speedIntervalType -= 1;

            foreach (GameObject target in speed_interval_images)
            {
                target.SetActive(false);
            }

            speed_interval_images[speedIntervalType].SetActive(true);


            InitialSpeedIntervalValue();
        }
    }

    public void SetSpeedTypeToLerp()
    {
        // ??????

        if (currentScene == scene.SELECT)
        {
            SelectManager.instance.SetIntervalType(speedInterval, speedType);

        }
        else if (currentScene == scene.WAITING)
        {
            WaitingRoom.instance.SetIntervalType(speedInterval);
        }
        else if (currentScene == scene.MEETING)
        {
            MeetingRoom.instance.SetIntervalType(speedInterval);
        }
    }


    
    public void SetSpeedInterval(float value)
    {
        speedInterval = value;
        SetSpeedTypeToLerp();
    }

    public void SetSpeedType(int type)
    {
        speedType = type;
        SetSpeedTypeToLerp();

    }

    void InitialSpeedIntervalValue()
    {
        if(speedIntervalType == 0)
            speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, speedValue);
        else
            speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, speedValue);

        SetSpeedInterval(speedLerpInterval);
    }

    // Speed �����̴� ���� -
    public void SpeedValueSub()
    {
        if (speedValue > 0)
        {
            speedValue -= 0.1f;
            speed_slider.value = speedValue;

            if(speedIntervalType == 0)
            {
                speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, speedValue);

            }else if(speedIntervalType == 1)
            {
                speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, speedValue);

            }

            SetSpeedInterval(speedLerpInterval);
        }
    }

    // Speed �����̴� ���� +
    public void SpeedValueAdd()
    {
        if (speedValue < 1f)
        {
            speedValue += 0.1f;
            speed_slider.value = speedValue;

            if (speedIntervalType == 0)
            {
                speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, speedValue);

            }
            else if (speedIntervalType == 1)
            {
                speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, speedValue);

            }

            SetSpeedInterval(speedLerpInterval);

        }
    }
    // Speed Slider ���� �� �ٲ� ������ ȣ��
    public void SetSpeedValue()
    {
        speedValue = speed_slider.value;
        
        if (speedIntervalType == 0)
        {
            speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, speedValue);

        }
        else if (speedIntervalType == 1)
        {
            speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, speedValue);

        }

        SetSpeedInterval(speedLerpInterval);

    }

    //public void SetSpeedValue(float value)
    //{
    //    //speedValue = value;

    //    if (speedIntervalType == 0)
    //    {
    //        speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, speedValue);

    //    }
    //    else if (speedIntervalType == 1)
    //    {
    //        speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, speedValue);

    //    }

    //    SetSpeedInterval(speedLerpInterval);

    //}

    // set speed interval from external
    public void SetSpeedValueWaitingMeeting(float value)
    {
        if (speedIntervalType == 0)
        {
            speedLerpInterval = Mathf.Lerp(speedIntervalAMin, speedIntervalAMax, value);

        }
        else if (speedIntervalType == 1)
        {
            speedLerpInterval = Mathf.Lerp(speedIntervalBMin, speedIntervalBMax, value);

        }

        SetSpeedInterval(speedLerpInterval);
    }

  


}
