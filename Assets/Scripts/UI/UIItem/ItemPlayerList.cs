using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPlayerList
{
    public TextMeshProUGUI txtName;
    public string id;
    public GameObject goPlayerListItem;

    public ItemPlayerList(string name,string id,GameObject goListPlayerPrefabs)
    {
        this.id = id;
        goPlayerListItem = GameObject.Instantiate(goListPlayerPrefabs);
        txtName = this.goPlayerListItem.GetComponent<TextMeshProUGUI>();
        txtName.text = name;
    }

}
