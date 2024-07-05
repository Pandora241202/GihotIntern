using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Slider sliderLevel;
    [SerializeField] private Button btnPause;

    public void OnSetUp()
    {
        sliderHealth.maxValue = 1;
        sliderHealth.value=sliderHealth.maxValue;
        sliderLevel.maxValue = 1;
        sliderLevel.value = 0;
    }

    public void OnPause_Clicked()
    {
        UIManager._instance.PauseGame();
        UIManager._instance.uiPause.gameObject.SetActive(true);
    }
}
