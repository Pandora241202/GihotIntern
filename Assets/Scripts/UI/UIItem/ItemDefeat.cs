using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDefeat : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtScore;

    public void OnShowUp(string name, int score)
    {
        txtName.text = name;
        txtScore.text = score.ToString();
    }
}
