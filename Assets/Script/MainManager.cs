using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject ButtonGroup;
    [SerializeField] GameObject setting;

    [SerializeField] Slider bgmVolume;
    [SerializeField] Slider effectVolume;
    [SerializeField] TMP_Dropdown resolution;


    GameObject[] Buttons;

    private void Awake()
    {
        Buttons = new GameObject[ButtonGroup.transform.childCount];
        for(int i = 0; i < ButtonGroup.transform.childCount; i++)
        {
            Buttons[i] = ButtonGroup.transform.GetChild(i).gameObject;
        }
        ButtonClear();
        OnClick_Back();


        DataManager.Instance.LoadSettingData();

        if (DataManager.Instance.settingData != null)
        {
            bgmVolume.value = DataManager.Instance.settingData.bgmVolume * 2f;
            effectVolume.value = DataManager.Instance.settingData.effectVolume * 2f;
            resolution.value = DataManager.Instance.settingData.resolutionOption;

            GameManager.Instance.resolutionOption = resolution.value;
        }
        SoundManager.Instance.ChangeBackgroundMusic("Hopeof", 0.5f, bgmVolume.value);
    }
    void ButtonClear()
    {
        for(int index = 0; index < Buttons.Count(); index++)
            Buttons[index].SetActive(false);
    }
    GameObject ButtonVisible(string name)
    {
        GameObject findbutton = null;
        foreach(GameObject button in Buttons)
        {
            if (button.name == name)
            {
                findbutton = button;
                break;
            }
        }
        findbutton.SetActive(true);
        return findbutton;
    }
    public void OnClick_Back()
    {
        ButtonClear();

        ButtonVisible("PlayBtn");
        ButtonVisible("SettingBtn");
        ButtonVisible("ExitBtn");
    }
    public void OnClick_Play()
    {
        ButtonClear();

        foreach(var btn in ButtonVisible("PreviousBtn").GetComponentsInChildren<Button>())
            btn.interactable = DataManager.Instance.HasFile();
        
        ButtonVisible("NewGameBtn");
        ButtonVisible("BackBtn");
    }
    public void OnClick_Setting()
    {
        DataManager.Instance.LoadSettingData();

        if (DataManager.Instance.settingData != null)
        {
            bgmVolume.value = DataManager.Instance.settingData.bgmVolume;
            effectVolume.value = DataManager.Instance.settingData.effectVolume;
            resolution.value = DataManager.Instance.settingData.resolutionOption;

            //Debug.Log("BGM: " + bgmVolume.value + "/  Effect: " + effectVolume.value);
            GameManager.Instance.resolutionOption = resolution.value;
        }

        setting.SetActive(true);
    }
    public void OnClick_Exit()
    {
        Application.Quit();
    }

    public void OnClick_PreviousGame()
    {
        GameManager.Instance.LoadGame();
    }
    public void OnClick_NewGame()
    {
        DataManager.Instance.DeleteGameData();
        GameManager.Instance.LoadGame();
    }

    //<Setting>

    public void OnClick_Setting_Apply()
    {
        SoundManager.Instance.BackgroundSoundSource.volume = bgmVolume.value;
        SoundManager.Instance.effectSoundSource.volume = effectVolume.value;

        GameManager.Instance.SetResolution(resolution.value);
        GameManager.Instance.SaveSetting();

        setting.SetActive(false);
    }

    public void OnClick_Setting_Cancel()
    {
        if (DataManager.Instance.settingData != null)
        {
            bgmVolume.value = DataManager.Instance.settingData.bgmVolume;
            effectVolume.value = DataManager.Instance.settingData.effectVolume;
            resolution.value = DataManager.Instance.settingData.resolutionOption;

            GameManager.Instance.resolutionOption = resolution.value;
        }
        setting.SetActive(false);
    }

    public void OnClick_Setting_Initialize()
    {
        bgmVolume.value = 0.5f;
        effectVolume.value = 0.5f;
        resolution.value = 2;
    }

    public void BackgroundVolume()
    {
        SoundManager.Instance.BackgroundSoundSource.volume = bgmVolume.value;
    }

    public void EffectVolume()
    {
        SoundManager.Instance.effectSoundSource.volume = effectVolume.value;
    }
    public void SetResolution()
    {
        GameManager.Instance.SetResolution(resolution.value);
    }
}
