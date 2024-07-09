using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Slider sliderHealth;
    public Slider sliderLevel;
    public TextMeshProUGUI txtLevel;
    [SerializeField] private Button btnPause;

    public void OnSetUp(float maxHealth, float maxExp)
    {
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth; 
        sliderLevel.maxValue = maxExp; 
        sliderLevel.value = 0;
        txtLevel.text = Constants.PlayerBaseLevel.ToString();
    }

    public void OnPause_Clicked()
    {
        SendData<PauseEvent> data = new SendData<PauseEvent>(new PauseEvent());
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        sliderHealth.value = currentHealth;
    }


    public void UpdateLevelSlider(float expProgress)
    {
        sliderLevel.value = expProgress; 
    }

    public void LevelUpdateSlider(float expRequire)
    {
        sliderLevel.maxValue = expRequire;
        txtLevel.text = AllManager._instance.playerManager.level.ToString();
        sliderLevel.value = 0;
        sliderHealth.maxValue = AllManager._instance.playerManager.GetMaxHealthFromLevel();
    }
}