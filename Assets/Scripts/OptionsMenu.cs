using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    PlayerUI ui;
    private GameObject UI_Instance;

    [SerializeField]
    private Toggle SSAO_button;
    [SerializeField]
    private Toggle MotionBlur_button;
    [SerializeField]
    private Toggle Bloom_button;
    [SerializeField]
    private Toggle SunShafts_button;
    [SerializeField]
    private Toggle Minimap;
    [SerializeField]
    private Toggle Reflections;
    [SerializeField]
    private Toggle DustStorm;

    void Start()
    {
        UI_Instance = CarSetup.Get_UI_Instance();
        ui = UI_Instance.GetComponent<PlayerUI>();
    }

    void Update()
    {
        SSAO_button.isOn = ui.ssao;
        MotionBlur_button.isOn = ui.motionblur;
        Bloom_button.isOn = ui.bloom;
        SunShafts_button.isOn = ui.sunshafts;
        Minimap.isOn = GameManagerCar.instance.MinimapCamera.activeSelf;
        Reflections.isOn = ui.reflections;
        DustStorm.isOn = GameManagerCar.instance.DustParticles.activeSelf;
    }

    public void Toggle_SSAO()
    {
        if (ui.ssao)
            ui.ssao = false;
        else
            ui.ssao = true;
    }

    public void Toggle_MOTIONBLUR()
    {
        if (ui.motionblur)
            ui.motionblur = false;
        else
            ui.motionblur = true;
    }

    public void Toggle_BLOOM()
    {
        if (ui.bloom)
            ui.bloom = false;
        else
            ui.bloom = true;
    }

    public void Toggle_SUNSHAFTS()
    {
        if (ui.sunshafts)
            ui.sunshafts = false;
        else
            ui.sunshafts = true;
    }

    public void Toggle_REFLECTIONS()
    {
        if (ui.reflections)
            ui.reflections = false;
        else
            ui.reflections = true;
    }

    public void Toggle_MINIMAP()
    {
        if (GameManagerCar.instance.MinimapCamera.activeSelf == true)
            GameManagerCar.instance.MinimapCamera.SetActive(false);
        else
            GameManagerCar.instance.MinimapCamera.SetActive(true);
    }

    public void Toggle_PARTICLES()
    {
        if (GameManagerCar.instance.DustParticles.activeSelf == true)
            GameManagerCar.instance.DustParticles.SetActive(false);
        else
            GameManagerCar.instance.DustParticles.SetActive(true);
    }
}
