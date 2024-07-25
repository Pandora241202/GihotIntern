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
    public void OnSetUpItemUpgrade(int index,UpgradesConfig config)
    {
        _config = config;
        txtName.text = config.lsUpgradesConfig[index].name;
        txtDes.text = config.lsUpgradesConfig[index].des;
        if (AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lsPermUpgrade[index] == 0)
        {
            Debug.Log("cc");
            price = 100;
        }
        else
        {
            price = (config.lsUpgradesConfig[index].basePrice * AllManager.Instance().playerManager
                .dictPlayers[Player_ID.MyPlayerID].lsPermUpgrade[index]);
        }
        txtPrice.text = price.ToString();
        ChangeStar(AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lsPermUpgrade[index],index);
    }
    
    public void ChangeStar(int level,int index)
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
