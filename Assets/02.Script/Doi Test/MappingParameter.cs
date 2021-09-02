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
    
    [Header("value")]
    // Geometry
    public int geometryType = 0;
    public float geoValue = 0f;

    // Color
    public float color_A_R = 0f;
    public float color_A_G = 0f;
    public float color_A_B = 0f;
    public float color_A_BW = 0f;

    public float color_B_R = 0f;
    public float color_B_G = 0f;
    public float color_B_B = 0f;
    public float color_B_BW = 0f;

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
    public GameObject[] menu_UI;

    [Header("UI")]
    // General
    public Button menu_left_button;
    public Button menu_right_button;

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
        // 전체 메뉴 <- ->
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
}

    // 전체 메뉴 <- 버튼
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
        }
    }

    // 전체 메뉴 -> 버튼
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
        }
    }

    // Geometry 타입 바꾸기 ->버튼
    public void SetGeoType_Right()
    {
        if(geometryType < 3)
        {
            geometryType += 1;
        }
    }

    // Geometry 타입 바꾸기 <-버튼

    public void SetGeoType_Left()
    {
        if (geometryType > 0)
        {
            geometryType -= 1;
        }
    }

    // Geo 슬라이더 조작 -
    public void GeoValueSub()
    {
        if(geoValue > 0)
        {
            geoValue -= 0.1f;
            geo_slider.value = geoValue;
        }
    }

    // Geo 슬라이더 조작 +
    public void GeoValueAdd()
    {
        if (geoValue < 1f)
        {
            geoValue += 0.1f;
            geo_slider.value = geoValue;
        }
    }
    // Geometry Slider 에서 값 바꿀 때마다 호출
    public void SetGeoValue()
    {
        geoValue = geo_slider.value;
    }

   

    // Color value slider에서 값 바꿀 때 호출
    public void SetColor_R_Value(int targetColor)
    {
        if(targetColor == 0)
        {
            color_A_R = color_A_R_slider.value;
        }
        else
        {
            color_B_R = color_B_R_slider.value;
        }
    }
    // Color value slider에서 값 바꿀 때 호출
    public void SetColor_G_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_G = color_A_G_slider.value;
        }
        else
        {
            color_B_G = color_B_G_slider.value;
        }
    }
    // Color value slider에서 값 바꿀 때 호출
    public void SetColor_B_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_B = color_A_B_slider.value;
        }
        else
        {
            color_B_B = color_B_B_slider.value;
        }
    }
    // Color value slider에서 값 바꿀 때 호출
    public void SetColor_BW_Value(int targetColor)
    {
        if (targetColor == 0)
        {
            color_A_BW = color_A_BW_slider.value;
        }
        else
        {
            color_B_BW = color_B_BW_slider.value;
        }
    }

    // Color Slider 에서 값 바꿀 때 호출
    public void SetColorValue()
    {
        colorValue = color_slider.value;
    }

    // Color 슬라이더 조작 -
    public void ColorValueSub()
    {
        if (colorValue > 0)
        {
            colorValue -= 0.1f;
            color_slider.value = colorValue;
        }
    }
    // Color 슬라이더 조작 +
    public void ColorValueAdd()
    {
        if (colorValue < 1)
        {
            colorValue += 0.1f;
            color_slider.value = colorValue;
        }
    }

    // vfx type 바꾸는 버튼 ->
    public void ChangeVFXType_Left()
    {
        if(vfx_textureType < 3)
        {
            vfx_textureType += 1;
        }
    }

    // vfx type 바꾸는 버튼 <-
    public void ChangeVFXType_Right()
    {
        if (vfx_textureType > 0)
        {
            vfx_textureType -= 1;
        }
    }

    // vfx 슬라이더 조작 -
    public void VfxValueSub()
    {
        if(vfxValue > 0)
        {
            vfxValue -= 0.1f;
            vfx_slider.value = vfxValue;
        }
    }
    // vfx 슬라이더 조작 +
    public void VfxValueAdd()
    {
        if (vfxValue < 1)
        {
            vfxValue += 0.1f;
            vfx_slider.value = vfxValue;
        }
    }
    // vfx Slider 에서 값 바꿀 때마다 호출
    public void SetVFXValue()
    {
        vfxValue = vfx_slider.value;
    }
    // speed 타입 바꾸기 ->버튼
    public void SetSpeedType_Right()
    {
        if (speedType < 3)
        {
            speedType += 1;
        }
    }

    // Speed 타입 바꾸기 <-버튼

    public void SetSpeedType_Left()
    {
        if (speedType > 0)
        {
            speedType -= 1;
        }
    }
    // Speed 슬라이더 조작 -
    public void SpeedValueSub()
    {
        if (speedValue > 0)
        {
            speedValue -= 0.1f;
            speed_slider.value = speedValue;
        }
    }

    // Speed 슬라이더 조작 +
    public void SpeedValueAdd()
    {
        if (speedValue < 1f)
        {
            speedValue += 0.1f;
            speed_slider.value = speedValue;
        }
    }
    // Speed Slider 에서 값 바꿀 때마다 호출
    public void SetSpeedValue()
    {
        speedValue = speed_slider.value;
    }
   
}
