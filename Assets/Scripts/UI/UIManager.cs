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

    public void PlaySfx(int index)
    {
        AllManager._instance.audioSource.clip = AllManager._instance.lsAudioClip[index];
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

    public void OnLogin()
    {
        uiLogin.gameObject.SetActive(false);
        uiMainMenu.gameObject.SetActive(true);
    }
}