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
            case "Health":
                txtName.text = "Health";
                txtDes.text = "Increase Player's Max Health Point.";
                imgIcon.sprite = lsSpriteIcon[0];
                break;
            case "Speed":
                txtName.text = "Speed";
                txtDes.text = "Increase Player's Movement Speed.";
                imgIcon.sprite = lsSpriteIcon[1];
                break;
            case "Damage":
                txtName.text = "Damage";
                txtDes.text = "Increase the base damage of the bullet.";
                imgIcon.sprite = lsSpriteIcon[2];
                break;
            case "CRIT":
                txtName.text = "CRIT";
                txtDes.text = "Increase the CRIT Rate and CRIT Damage of the bullet.";
                imgIcon.sprite = lsSpriteIcon[3];
                break;
            case "Life Steal":
                txtName.text = "Life Steal";
                txtDes.text = "Increase the chance of healing 1 HP per hit.";
                imgIcon.sprite = lsSpriteIcon[4];
                break;
            case "Fire Rate":
                txtName.text = "Fire Rate";
                txtDes.text = "Increase the fire rate of the bullet.";
                imgIcon.sprite = lsSpriteIcon[5];
                break;
            case "AOE Meteor":
                txtName.text = "AOE Meteor";
                txtDes.text = "Summon a meteor that deals high AOE damage.";
                imgIcon.sprite = lsSpriteIcon[6];
                break;
            case "Time Warp":
                txtName.text = "Time Warp";
                txtDes.text = "Slow the speed of enemies down";
                imgIcon.sprite = lsSpriteIcon[7];
                break;
            default:
                break;
        }
    }
}
