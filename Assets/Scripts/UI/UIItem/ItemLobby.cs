using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemLobby : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtGameMode;

    public void OnGetLobby(string name, string gamemode)
    {
        txtGameMode.text = gamemode;
        txtName.text = name;
    }
}
