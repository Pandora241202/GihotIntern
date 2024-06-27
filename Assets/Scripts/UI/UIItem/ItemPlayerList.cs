using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlayerList
{
    public TextMeshProUGUI txtName;
    public string id;
    public GameObject goPlayerListItem;
    public GameObject goCrown;
    public Button btnKick;

    public ItemPlayerList(string name,string id,GameObject goListPlayerPrefabs)
    {
        this.id = id;
        goPlayerListItem = GameObject.Instantiate(goListPlayerPrefabs);
        txtName = this.goPlayerListItem.transform.Find("txtNamePlayer").GetComponent<TextMeshProUGUI>();
        
        goCrown = goPlayerListItem.transform.Find("imgIconHost").gameObject;
        btnKick=goPlayerListItem.transform.Find("btnKick").gameObject.GetComponent<Button>();
        goCrown.gameObject.SetActive(false);
        btnKick.gameObject.SetActive(false);
        txtName.text = name;
        btnKick.onClick.AddListener(() =>
        {
            Debug.Log(id);
        });
    }

}
