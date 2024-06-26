using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField ] List<Button> lsBtnShowPlayer=new List<Button>();
    [SerializeField] private List<GameObject> lsGOPlayer = new List<GameObject>();
    [SerializeField] private GameObject goOnline;
    [SerializeField] private Button btnOnline;
    [SerializeField] private List<GameObject> lsBtnForPlayer = new List<GameObject>();
    public void OnSetUp()
    {
        lsGOPlayer[0].SetActive(true);
        lsGOPlayer[1].SetActive(false);
        goOnline.SetActive(false);
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
    }

    public void ShowPlayerBtn()
    {
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
    }
    
    
    public void OnBtnClick(int index)
    {
        if (index == 0)
        {
            lsGOPlayer[1].SetActive(true);
            lsGOPlayer[0].SetActive(false);
        }
        else
        {
            lsGOPlayer[0].SetActive(true);
            lsGOPlayer[1].SetActive(false);
        }
    }

    public void OnOnline_Clicked()
    {
        goOnline.SetActive(true);
    }
    
}
