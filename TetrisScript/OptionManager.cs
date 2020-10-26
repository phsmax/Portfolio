using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPanel;

    public AudioSource[] sounds;
    public Toggle soundToggle;
    public Slider soundSlider;

    public TouchCotrol TC;
    public Slider HSlider;
    public Text HText;
    public Slider VSlider;
    public Text VText;

    public Canvas wolrdcanvas;

    string SoundToggle = "TOGGLESOUND";
    string SoundSlide = "SLIDESOUND";

    string SensiSave = "SENSISAVE";
    string VSensiSave = "VSENSISAVE";

    void Start()
    {
        LoadConfig();
    }

    void LoadConfig()
    {
        // 사운드 뮤트값 불러오기
        int stg = PlayerPrefs.GetInt(SoundToggle, 1);
        if (stg == 1)
            soundToggle.isOn = true;
        else
            soundToggle.isOn = false;
        ToggleSound();

        //사운드 슬라이드값 불러오기
        float val = PlayerPrefs.GetFloat(SoundSlide, 1);
        soundSlider.value = val;
        SoundSlider();

        //좌우 센시값 불러오기
        float sensi = PlayerPrefs.GetFloat(SensiSave, 0.165f);
        HSlider.value = sensi;
        SensiControl();

        //상하 센시값 불러오기
        int vsensi = PlayerPrefs.GetInt(VSensiSave, 50);
        Debug.Log(vsensi);
        VSlider.value = (float)vsensi;
        VSensiControl();
    }

    public void VSensiDefault() // v센시값 초기화
    {
        int val = 50;
        VSlider.value = val;
        TC.vsensi = val;
        VText.text = val.ToString();
        PlayerPrefs.SetInt(VSensiSave, val);
    }

    public void VSensiControl() // v센시값 조절
    {
        int val = (int)VSlider.value;
        TC.vsensi = val;
        VText.text = val.ToString();
        PlayerPrefs.SetInt(VSensiSave, val);
    }

    public void SensiDefault() // h센시값 초기화
    {
        float val = 0.165f;
        HSlider.value = val;
        TC.sensi = val;
        HText.text = val.ToString();
        PlayerPrefs.SetFloat(SensiSave, val);
    }

    public void SensiControl() // h센시값 조절
    {
        float val = HSlider.value;
        TC.sensi = val;
        HText.text = val.ToString();
        PlayerPrefs.SetFloat(SensiSave,val);
    }

    public void ConfigButton() // 설정버튼 클릭
    {
        optionPanel.SetActive(!optionPanel.activeSelf);

        if (optionPanel.activeSelf)
            wolrdcanvas.sortingOrder = 2;
        else
            wolrdcanvas.sortingOrder = 3;
    }

    public void ToggleSound() // 음소거 체크박스
    {
        if (soundToggle.isOn)
        {
            foreach (AudioSource asource in sounds)
                asource.mute = false;
            PlayerPrefs.SetInt(SoundToggle, 1);
        }
        else
        {
            foreach (AudioSource asource in sounds)
                asource.mute = true;
            PlayerPrefs.SetInt(SoundToggle, 0);
        }
    }

    public void SoundSlider() // 소리음량 슬라이더
    {
        float val = soundSlider.value;
        foreach (AudioSource asource in sounds)
            asource.volume = val;
        PlayerPrefs.SetFloat(SoundSlide, val);
    }
}
