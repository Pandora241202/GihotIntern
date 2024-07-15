using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelUp : MonoBehaviour
{
    [SerializeField] private List<ItemLevelUp> lsItemLevel;
    public void OnSetUp(List<string> lsLevelUp)
    {
        int i = 0;
        this.gameObject.SetActive(true);
        foreach (var buff in lsItemLevel)
        {
            buff.OnChangeInfo(lsLevelUp[i]);
            i++;
        }
    }

    public void OnItemLevelUp_Clicked(int index)
    {
        //TODO Hung
        AllLevelUpConfig allLevelUpConfig = AllManager.Instance().playerManager.allLevelUpConfig;
        LevelUpConfig levelUpConfig = allLevelUpConfig.allLevelUpConfigList[0];
        
        levelUpConfig.ApplyBaseStat(lsItemLevel[index].txtName.text);
        //TODO sent Chosed Tung
    }
}
