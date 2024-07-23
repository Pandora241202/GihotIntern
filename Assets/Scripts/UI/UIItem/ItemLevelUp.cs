using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelUp : MonoBehaviour
{
    public List<Sprite> lsSpriteIcon = new List<Sprite>();
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtDes;
    [SerializeField] private Image imgIcon;
    public void OnChangeInfo(string name)
    {
        switch (name)
        {
            case "health":
                txtName.text = "Health";
                txtDes.text = "Increase Player's Max Health Point.";
                imgIcon.sprite = lsSpriteIcon[0];
                break;
            case "speed":
                txtName.text = "Speed";
                txtDes.text = "Increase Player's Movement Speed.";
                imgIcon.sprite = lsSpriteIcon[1];
                break;
            case "damage":
                txtName.text = "Damage";
                txtDes.text = "Increase the base damage of the bullet.";
                imgIcon.sprite = lsSpriteIcon[2];
                break;
            case "crit":
                txtName.text = "CRIT";
                txtDes.text = "Increase the CRIT Rate and CRIT Damage of the bullet.";
                imgIcon.sprite = lsSpriteIcon[3];
                break;
            case "lifesteal":
                txtName.text = "Life Steal";
                txtDes.text = "Increase the chance of healing 1HP per hit.";
                imgIcon.sprite = lsSpriteIcon[4];
                break;
            default:
                break;
        }
    }
}
