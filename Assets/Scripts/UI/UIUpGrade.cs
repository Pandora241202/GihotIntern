using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpGrade : MonoBehaviour
{
    [SerializeField] private List<Button> btnUpgrade = new List<Button>();
    [SerializeField] private TextMeshProUGUI txtCoin;
    public List<GameObject> lsLevelUpgrade = new List<GameObject>();
    [SerializeField] private Button btnClose;

    public void OnSetUp(Player player)
    {
        PermUpdateInfo info = player.info;
        txtCoin.text = info.coin.ToString();
        Dictionary<int, int> upgrade = new Dictionary<int, int>
        {
            {0, info.health },
            {1, info.critrate },
            {2, info.critdmg },
            {3, info.damage },
            {4, info.lifesteal },
            {5, info.firerate },
        };

        foreach(var pair in upgrade)
        {
            lsLevelUpgrade[pair.Key].gameObject.GetComponent<ItemUpgrade>()
                .OnSetUpItemUpgrade(pair.Key, AllManager.Instance().upgradeConfig, pair.Value);
        }

        btnClose.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }

    public void OnButtonUpgrade_Clicked(int index)
    {
        UIManager._instance.PlaySfx(0);
        if (AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].info.coin >= lsLevelUpgrade[index].GetComponent<ItemUpgrade>().price)
        {
            AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].info.coin -= lsLevelUpgrade[index].GetComponent<ItemUpgrade>().price;
            PermUpdateInfo updateInfo = new PermUpdateInfo();
            PermUpdateInfo temp = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].info;
            int level = 0;
            switch (index)
            {
                case 0:
                    updateInfo.health = temp.health + 1;
                    level = updateInfo.health;
                    updateInfo.fieldToUpdate = "health";
                    break;
                case 1:
                    updateInfo.critrate = temp.critrate + 1;
                    level = updateInfo.critrate;
                    updateInfo.fieldToUpdate = "critrate";
                    break;
                case 2:
                    updateInfo.critdmg = temp.critdmg + 1;
                    level = updateInfo.critdmg;
                    updateInfo.fieldToUpdate = "critdmg";
                    break;
                case 3:
                    updateInfo.damage = temp.damage + 1;
                    level = updateInfo.damage;
                    updateInfo.fieldToUpdate = "damage";
                    break;
                case 4:
                    updateInfo.lifesteal = temp.lifesteal + 1;
                    level = updateInfo.lifesteal;
                    updateInfo.fieldToUpdate = "lifesteal";
                    break;
                case 5:
                    updateInfo.firerate = temp.firerate + 1;
                    level = updateInfo.firerate;
                    updateInfo.fieldToUpdate = "firerate";
                    break;
            }
            if (level > 5) return;
            updateInfo.coin = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].info.coin;
            txtCoin.text = updateInfo.coin.ToString();
            SendData<PermUpdateInfo> data = new SendData<PermUpdateInfo>(updateInfo);
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
        
    }
}