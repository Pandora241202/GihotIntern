using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelUp : MonoBehaviour
{
    public List<Sprite> lsSpriteIcon = new List<Sprite>();
    public TextMeshProUGUI txtName;
    [SerializeField] private Image imgIcon;
    public void OnChangeInfo(string name)
    {
        switch (name)
        {
            case "health":
                txtName.text = name;
                imgIcon.sprite = lsSpriteIcon[0];
                break;
            case "speed":
                txtName.text = name;
                imgIcon.sprite = lsSpriteIcon[1];
                break;
            case "damage":
                txtName.text = name;
                imgIcon.sprite = lsSpriteIcon[2];
                break;
            case "crit":
                txtName.text = name;
                imgIcon.sprite = lsSpriteIcon[3];
                break;
            case "lifesteal":
                txtName.text = name;
                imgIcon.sprite = lsSpriteIcon[4];
                break;
            default:
                break;
        }
    }
}
