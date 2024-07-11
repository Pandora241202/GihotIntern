
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UILoading : MonoBehaviour
{
    public Image imgLoad;
    public TextMeshProUGUI txtPercent;
    public TextMeshProUGUI txtLoading;

    public void OnSetUp()
    {
        imgLoad.fillAmount = 0;
        txtLoading.text = "Loading...";
        txtPercent.text = "0%";
    }

    public void OnProgress(float progress)
    {
        while (true)
        {
            if (imgLoad.fillAmount >= progress) break;
            imgLoad.fillAmount += .1f;
        }
        txtPercent.text = (progress * 100).ToString() + "%";
        int rand = Random.Range(1, 3);

    }
}
