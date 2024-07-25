using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ItemUpgrade : MonoBehaviour
{
    [SerializeField] private List<GameObject> lsLevel = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtDes;
    [SerializeField] private TextMeshProUGUI txtPrice;
    private UpgradesConfig _config;
    public int price;
    public void OnSetUpItemUpgrade(int index, UpgradesConfig config, int value)
    {
        _config = config;
        txtName.text = config.lsUpgradesConfig[index].name;
        txtDes.text = config.lsUpgradesConfig[index].des;
        PermUpdateInfo info = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].info;
        if (value == 0)
        {
            Debug.Log("cc");
            price = config.lsUpgradesConfig[index].basePrice;
        }
        else
        {
            price = (config.lsUpgradesConfig[index].basePrice * value);
        }
        txtPrice.text = price.ToString();
        ChangeStar(index, value);
    }
    
    public void ChangeStar(int index,int level)
    {
        if (level == 0)
        {
            Debug.Log("CC");
            price = _config.lsUpgradesConfig[index].basePrice;
        }
        else
        {
            price = _config.lsUpgradesConfig[index].basePrice * level;
        }
        
        txtPrice.text = price.ToString();
        for (int i = 0; i < lsLevel.Count; i++)
        {
            if (level > i)
            {
                lsLevel[i].SetActive(true);
            }
            else
            {
                lsLevel[i].SetActive(false);
            }
        }
    }
}
