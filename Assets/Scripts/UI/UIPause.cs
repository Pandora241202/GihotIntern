using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    public List<Button> lsBtnPause;

    public void OnBtn_Clicked(int index)
    {
        switch (index)
        {
            case 0:
                //Setting
                break;
            case 1:
                //Resume
                UIManager._instance.ResumeGame();
                UIManager._instance.uiPause.gameObject.SetActive(false);
                break;
            case 2:
                //Leave
                
                break;
        }
    }
}
