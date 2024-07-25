using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIUpGrade : MonoBehaviour
{
    [SerializeField] private List<Button> btnUpgrade = new List<Button>();
    [SerializeField] private List<GameObject> lsLevelUpgrade = new List<GameObject>();
    [SerializeField] private Button btnClose;

    public void OnSetUp(Player player)
    {
        for (int i = 0; i < lsLevelUpgrade.Count; i++)
        {
            lsLevelUpgrade[i].gameObject.GetComponent<ItemUpgrade>()
                .OnSetUpItemUpgrade(i, AllManager.Instance().upgradeConfig);
        }

        btnClose.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }

    public void OnButtonUpgrade_Clicked(int index)
    {
        UIManager._instance.PlaySfx(0);
        if (AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].coin >=
            lsLevelUpgrade[index].GetComponent<ItemUpgrade>().price)
        {
            //

            AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lsPermUpgrade[index]++;
            lsLevelUpgrade[index].GetComponent<ItemUpgrade>().ChangeStar(
                AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lsPermUpgrade[index], index);
        }
    }
}