using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDefeat : MonoBehaviour
{
    [SerializeField] private Button btnNext;
    [SerializeField] private ItemDefeat goItem;
    [SerializeField] private GameObject goMidBoard;
    
    public void OnSetUp(GameEnd arrScore)
    {
        this.gameObject.SetActive(true);
        foreach (Transform child in goMidBoard.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = arrScore.result.Length-1; i >= 0; i--)
        {
            ItemDefeat item = Instantiate(goItem, goMidBoard.transform);
            item.OnShowUp(AllManager._instance.playerManager.dictPlayers[arrScore.result[i].player_id].name,arrScore.result[i].enemy_kill);
            item.transform.SetParent(goMidBoard.transform);
        }
    }

    public void OnNextButtonClicked()
    {
        this.gameObject.SetActive(false);
        UIManager._instance.PlaySfx(0);
        AllManager.Instance().StartCoroutine(AllManager.Instance().GameEnd());
    }
}
