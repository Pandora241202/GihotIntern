using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIPause uiPause;
    public UIDefeat uiDefeat;
    public UIGamePlay uiGameplay;
    public UIOnlineLobby uiOnlineLobby;
    public UIMainMenu uiMainMenu;
    public UILogin uiLogin;
    public UIChoseGun uiChoseGun;
    
    public UILoading uiLoading;

    public UILevelUp uiLevelUp;
    public FixedJoystick _joystick;
    public FloatingJoystick _fjoystick;
 
    public static UIManager _instance { get; private set; }
    public GameObject prefUILevel;
    public List<GameObject> lsLevelUp = new List<GameObject>();
    public float sfxCDTime = 0.25f;
    private float lastSfxTime = 0;

    public void MyPlaySfx(int sfxIndex, float volume = 1.0f, float sfxCooldownTime = 0.25f)
    {
        sfxCDTime = sfxCooldownTime;
        if (Time.time >= lastSfxTime + sfxCDTime)
        {
            PlaySfx(sfxIndex, volume);
            lastSfxTime = Time.time;
        }
    }
    public void PauseGame()
    {
        uiPause.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        uiPause.gameObject.SetActive(false);
    }
    private void Awake()
    {
        _joystick.gameObject.SetActive(false);
        _fjoystick.gameObject.SetActive(false);
        uiOnlineLobby.OnSetUp();
        uiMainMenu.OnSetUp();
        uiChoseGun.OnSetUp();
        uiLogin.OnSetUp();

        uiLoading.OnSetUp();

        
        uiDefeat.gameObject.SetActive(false);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
            //new

    }

    public void PlaySfx(int index, float volume = 1f)
    {
        AllManager._instance.audioSource.clip = AllManager._instance.lsAudioClip[index];
        AllManager._instance.audioSource.volume = volume;
        AllManager._instance.audioSource.Play();
    }
    private void Update()
    {
        // Debug.Log(Player_ID.MyPlayerID);
    }

    public void OnClose_Clicked()
    {
        this.gameObject.SetActive(false);
    }

    public void OnInstanceLevel(List<string> lsLevelUp)
    {
        uiGameplay.goWaiting.SetActive(true);
        GameObject on = GameObject.Instantiate(prefUILevel);
        on.gameObject.GetComponent<UILevelUp>().OnSetUp(lsLevelUp);
        on.gameObject.transform.SetParent(this.transform);
        on.gameObject.GetComponent<RectTransform>().offsetMax=Vector2.zero;
        on.gameObject.GetComponent<RectTransform>().offsetMin=Vector2.zero;
    }
    public void OnLogin()
    {
        uiLogin.gameObject.SetActive(false);
        uiMainMenu.gameObject.SetActive(true);
    }

    public void MuteBGM()
    {
        gameObject.GetComponent<AudioSource>().mute = !gameObject.GetComponent<AudioSource>().mute;
        AllManager._instance.audioSource.mute = !AllManager._instance.audioSource.mute;
    }
}