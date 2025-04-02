using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("마스터 볼륨")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Toggle masterMuteToggle;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    
    [Header("BGM 볼륨")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Toggle bgmMuteToggle;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    
    [Header("효과음 볼륨")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle sfxMuteToggle;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    
    [Header("UI 사운드")]
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Toggle uiMuteToggle;
    [SerializeField] private TextMeshProUGUI uiVolumeText;
    
    [Header("음성 볼륨")]
    [SerializeField] private Slider voiceVolumeSlider;
    [SerializeField] private Toggle voiceMuteToggle;
    [SerializeField] private TextMeshProUGUI voiceVolumeText;
    
    [Header("테스트 사운드")]
    [SerializeField] private string testBGM = "TestBGM";
    [SerializeField] private string testSFX = "TestSFX";
    [SerializeField] private string testUI = "TestUI";
    [SerializeField] private string testVoice = "TestVoice";
    
    // 저장된 음소거 상태
    private bool masterMuted, bgmMuted, sfxMuted, uiMuted, voiceMuted;
    
    private void Start()
    {
        InitializeUI();
        RegisterEvents();
    }
    
    private void OnEnable()
    {
        // 패널이 켜질 때마다 현재 설정으로 UI 업데이트
        UpdateUI();
    }
    
    private void InitializeUI()
    {
        // PlayerPrefs에서 음소거 상태 로드
        masterMuted = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        bgmMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        uiMuted = PlayerPrefs.GetInt("UIMuted", 0) == 1;
        voiceMuted = PlayerPrefs.GetInt("VoiceMuted", 0) == 1;
        
        // UI 초기 상태 설정
        UpdateUI();
    }
    
    private void RegisterEvents()
    {
        // 슬라이더 이벤트 연결
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            
        if (uiVolumeSlider != null)
            uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
            
        if (voiceVolumeSlider != null)
            voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
            
        // 토글 이벤트 연결
        if (masterMuteToggle != null)
            masterMuteToggle.onValueChanged.AddListener(OnMasterMuteToggled);
            
        if (bgmMuteToggle != null)
            bgmMuteToggle.onValueChanged.AddListener(OnBGMMuteToggled);
            
        if (sfxMuteToggle != null)
            sfxMuteToggle.onValueChanged.AddListener(OnSFXMuteToggled);
            
        if (uiMuteToggle != null)
            uiMuteToggle.onValueChanged.AddListener(OnUIMuteToggled);
            
        if (voiceMuteToggle != null)
            voiceMuteToggle.onValueChanged.AddListener(OnVoiceMuteToggled);
    }
    
    private void UpdateUI()
    {
        // 슬라이더 값 업데이트
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            UpdateVolumeText(masterVolumeText, masterVolumeSlider.value);
        }
        
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.7f);
            UpdateVolumeText(bgmVolumeText, bgmVolumeSlider.value);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            UpdateVolumeText(sfxVolumeText, sfxVolumeSlider.value);
        }
        
        if (uiVolumeSlider != null)
        {
            uiVolumeSlider.value = PlayerPrefs.GetFloat("UIVolume", 1.0f);
            UpdateVolumeText(uiVolumeText, uiVolumeSlider.value);
        }
        
        if (voiceVolumeSlider != null)
        {
            voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1.0f);
            UpdateVolumeText(voiceVolumeText, voiceVolumeSlider.value);
        }
        
        // 토글 상태 업데이트
        if (masterMuteToggle != null) masterMuteToggle.isOn = masterMuted;
        if (bgmMuteToggle != null) bgmMuteToggle.isOn = bgmMuted;
        if (sfxMuteToggle != null) sfxMuteToggle.isOn = sfxMuted;
        if (uiMuteToggle != null) uiMuteToggle.isOn = uiMuted;
        if (voiceMuteToggle != null) voiceMuteToggle.isOn = voiceMuted;
    }
    
    private void UpdateVolumeText(TextMeshProUGUI text, float value)
    {
        if (text != null)
        {
            text.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }
    
    // 볼륨 슬라이더 이벤트 핸들러
    private void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        UpdateVolumeText(masterVolumeText, value);
    }
    
    private void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioType.BGM, value);
        UpdateVolumeText(bgmVolumeText, value);
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioType.SFX, value);
        UpdateVolumeText(sfxVolumeText, value);
    }
    
    private void OnUIVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioType.UI, value);
        UpdateVolumeText(uiVolumeText, value);
    }
    
    private void OnVoiceVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioType.Voice, value);
        UpdateVolumeText(voiceVolumeText, value);
    }
    
    // 음소거 토글 이벤트 핸들러
    private void OnMasterMuteToggled(bool muted)
    {
        masterMuted = muted;
        AudioManager.Instance.SetMasterVolume(muted ? 0 : masterVolumeSlider.value);
        PlayerPrefs.SetInt("MasterMuted", muted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void OnBGMMuteToggled(bool muted)
    {
        bgmMuted = muted;
        AudioManager.Instance.SetVolume(AudioManager.AudioType.BGM, muted ? 0 : bgmVolumeSlider.value);
        PlayerPrefs.SetInt("BGMMuted", muted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void OnSFXMuteToggled(bool muted)
    {
        sfxMuted = muted;
        AudioManager.Instance.SetVolume(AudioManager.AudioType.SFX, muted ? 0 : sfxVolumeSlider.value);
        PlayerPrefs.SetInt("SFXMuted", muted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void OnUIMuteToggled(bool muted)
    {
        uiMuted = muted;
        AudioManager.Instance.SetVolume(AudioManager.AudioType.UI, muted ? 0 : uiVolumeSlider.value);
        PlayerPrefs.SetInt("UIMuted", muted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void OnVoiceMuteToggled(bool muted)
    {
        voiceMuted = muted;
        AudioManager.Instance.SetVolume(AudioManager.AudioType.Voice, muted ? 0 : voiceVolumeSlider.value);
        PlayerPrefs.SetInt("VoiceMuted", muted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // 테스트 사운드 재생 버튼 메서드
    public void PlayTestBGM()
    {
        AudioManager.Instance.PlayBGM(testBGM);
    }
    
    public void PlayTestSFX()
    {
        AudioManager.Instance.PlaySFX(testSFX);
    }
    
    public void PlayTestUI()
    {
        AudioManager.Instance.PlayUISound(testUI);
    }
    
    public void PlayTestVoice()
    {
        AudioManager.Instance.PlayVoice(testVoice);
    }
}