using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //public float color_A_BW = 0f;

    public float color_B_R = 0f;
    public float color_B_G = 0f;
    public float color_B_B = 0f;
    //public float color_B_BW = 0f;

    Color colorA;
    Color colorB;
    Color lerpColor;

    public float colorValue = 0f;
    int editingColor = 0;

    // VFX_Texture
    public int vfx_textureType = 0;
    public float vfxValue = 0f;

    // Speed
    public int speedType = 0;
    public float speedValue = 0f;
  

    [Header("General")]
    public int currentOpenMenu = 0;
    public int currentMatchEmotion = 0;
    public GameObject[] menu_UI;
    public int[] matchType;
    public bool[] typeUse;

    [Header("UI")]
    // General
    public Button menu_left_button;
    public Button menu_right_button;
    public Button match_button;

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
    //public Slider color_A_BW_slider;

    public Slider color_B_R_slider;
    public Slider color_B_G_slider;
    public Slider color_B_B_slider;
    //public Slider color_B_BW_slider;

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

    
    private void Awake()
    {
        if (_mappingParameter == null)
            _mappingParameter = this;


    }

    private void Start()
    {
        // ��ü �޴� <- ->
        menu_left_button.onClick.AddListener(GeneralMenu_Left);
        menu_right_button.onClick.AddListener(GeneralMenu_Right);

        // Geometry
        geo_left_button.onClick.AddListener(SetGeoType_Left);
        geo_right_button.onClick.AddListener(SetGeoType_Right);
        geo_sub_button.onClick.AddListener(GeoValueAdd);
        geo_add_button.onClick.AddListener(GeoValueSub);

        // Color
        color_sub_button.onClick.AddListener(ColorValueSub);
        color_add_button.onClick.AddListener(ColorValueAdd);



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

        previewCube.GetComponent<MeshRenderer>().material = new Material(refMaterial);

        colorA = new Color(color_A_R, color_A_G, color_A_B);
        colorB = new Color(color_B_R, color_B_G, color_B_B);
        lerpColor = Color.Lerp(colorA, colorB, 0f);

        // ������� ����
        LerpColorSetColor(colorValue);


        // Ÿ�� �̴ϼ�
        matchType = new int[4];
        typeUse = new bool[4];
        for(int i=0; i<4; i++)
        {
            matchType[i] = 0;
            typeUse[i] = false;
        }

        if (matchType[currentOpenMenu] == 0 && !typeUse[currentMatchEmotion])
        {
            match_button.interactable = true;
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

            if (matchType[currentOpenMenu] == 0 && !typeUse[currentMatchEmotion])
            {
                match_button.interactable = true;
            }
            else
            {
                match_button.interactable = false;

            }
        }

        
    }

    // ��ü �޴� -> ��ư
    public void GeneralMenu_Right()
    {
        if (currentOpenMenu < 3)
        {
            foreach (GameObject menu in menu_UI)
            {
                menu.SetActive(false);
            }
            currentOpenMenu += 1;

            menu_UI[currentOpenMenu].SetActive(true);

            if (matchType[currentOpenMenu] == 0 && !typeUse[currentMatchEmotion])
            {
                match_button.interactable = true;
            }
            else
            {
                match_button.interactable = false;

            }
        }

        
    }

    // CubeMaterial���� ȣ��
    public void SetCurrentEmotion(int num)
    {
        currentMatchEmotion = num;

        if (matchType[currentOpenMenu] == 0 && !typeUse[currentMatchEmotion])
        {
            match_button.interactable = true;
        }
    }

    // match ��ư Ŭ��
    public void MatchButtonClicked()
    {
        matchType[currentOpenMenu] = currentMatchEmotion;
        typeUse[currentMatchEmotion] = true;
        match_button.interactable = false;
        
    }

    // cancel ��ư Ŭ��
    public void CancelButtonClicked()
    {
        matchType[currentOpenMenu] = 0;
        typeUse[currentMatchEmotion] = false;

        if (matchType[currentOpenMenu] == 0 && !typeUse[currentMatchEmotion])
        {
            match_button.interactable = true;
        }
    }

    bool CheckTypeUse()
    {
        return typeUse[currentMatchEmotion];
    }

    public bool CheckAllMappingEmotion()
    {
        int count = 0;
        bool result = false;

        for(int i=0; i<4; i++)
        {
            if(matchType[i] == 0)
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

    // Geometry Ÿ�� �ٲٱ� ->��ư
    public void SetGeoType_Right()
    {
        if(geometryType < 3)
        {
            geometryType += 1;
        }
    }

    // Geometry Ÿ�� �ٲٱ� <-��ư

    public void SetGeoType_Left()
    {
        if (geometryType > 0)
        {
            geometryType -= 1;
        }
    }

    // Geo �����̴� ���� -
    public void GeoValueSub()
    {
        if(geoValue > 0)
        {
            geoValue -= 0.1f;
            geo_slider.value = geoValue;
        }
    }

    // Geo �����̴� ���� +
    public void GeoValueAdd()
    {
        if (geoValue < 1f)
        {
            geoValue += 0.1f;
            geo_slider.value = geoValue;
        }
    }
    // Geometry Slider ���� �� �ٲ� ������ ȣ��
    public void SetGeoValue()
    {
        geoValue = geo_slider.value;
    }

   

    // Color value slider���� �� �ٲ� �� ȣ��
    public void SetColor_R_Value(int targetColor)
    {
        if(targetColor == 0)
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
            colorA.b = color_B_B_slider.value;
            LerpColorSetColor(colorValue);
        }
        else
        {
            color_B_B = color_B_B_slider.value;
            colorB.b = color_B_B_slider.value;
            LerpColorSetColor(colorValue);

        }
    }
    // Color value slider���� �� �ٲ� �� ȣ��
    //public void SetColor_BW_Value(int targetColor)
    //{
    //    if (targetColor == 0)
    //    {
    //        color_A_BW = color_A_BW_slider.value;
    //        colorA.a = color_A_BW_slider.value;
    //        LerpColorSetColor(colorValue);
    //    }
    //    else
    //    {
    //        color_B_BW = color_B_BW_slider.value;
    //        colorB.a = color_B_BW_slider.value;
    //        LerpColorSetColor(colorValue);
    //    }
    //}

    // Color Slider ���� �� �ٲ� �� ȣ��
    public void SetColorValue()
    {
        colorValue = color_slider.value;
        LerpColorSetColor(colorValue);
    }

    // Color �����̴� ���� -
    public void ColorValueSub()
    {
        if (colorValue > 0)
        {
            colorValue -= 0.1f;
            color_slider.value = colorValue;
            LerpColorSetColor(colorValue);

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

        }
    }

    // ������ ť�꿡 ���� ������ �żҵ�
    void LerpColorSetColor(float value)
    {
        lerpColor = Color.Lerp(colorA, colorB, value);
        previewCube.GetComponent<MeshRenderer>().material.color = lerpColor;
    }

    // vfx type �ٲٴ� ��ư ->
    public void ChangeVFXType_Left()
    {
        if(vfx_textureType < 3)
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
        if(vfxValue > 0)
        {
            vfxValue -= 0.1f;
            vfx_slider.value = vfxValue;
        }
    }
    // vfx �����̴� ���� +
    public void VfxValueAdd()
    {
        if (vfxValue < 1)
        {
            vfxValue += 0.1f;
            vfx_slider.value = vfxValue;
        }
    }
    // vfx Slider ���� �� �ٲ� ������ ȣ��
    public void SetVFXValue()
    {
        vfxValue = vfx_slider.value;
    }
    // speed Ÿ�� �ٲٱ� ->��ư
    public void SetSpeedType_Right()
    {
        if (speedType < 3)
        {
            speedType += 1;
        }
    }

    // Speed Ÿ�� �ٲٱ� <-��ư

    public void SetSpeedType_Left()
    {
        if (speedType > 0)
        {
            speedType -= 1;
        }
    }
    // Speed �����̴� ���� -
    public void SpeedValueSub()
    {
        if (speedValue > 0)
        {
            speedValue -= 0.1f;
            speed_slider.value = speedValue;
        }
    }

    // Speed �����̴� ���� +
    public void SpeedValueAdd()
    {
        if (speedValue < 1f)
        {
            speedValue += 0.1f;
            speed_slider.value = speedValue;
        }
    }
    // Speed Slider ���� �� �ٲ� ������ ȣ��
    public void SetSpeedValue()
    {
        speedValue = speed_slider.value;
    }
   
}
