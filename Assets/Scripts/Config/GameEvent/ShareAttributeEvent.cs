using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ShareAttributeEventConfig", menuName = "Config/GameEventConfig/ShareAttributeEvent")]
public class ShareAttributeEvent : GameEventConfig
{
    public override void Activate(GameEvent gameEvent)
    {
        base.Activate(gameEvent);
        UIManager._instance.uiGameplay.ChangeSliderEvent(gameEvent.maxHP);
        UIManager._instance.uiGameplay.SetHealthSliderValue(gameEvent.curHP);
    }

    public override void Apply(GameEvent gameEvent)
    {
        base.Apply(gameEvent);
        if (gameEvent.maxHP != UIManager._instance.uiGameplay.sliderHealth.maxValue) UIManager._instance.uiGameplay.ChangeSliderEvent(gameEvent.maxHP);
        if(gameEvent.curHP != UIManager._instance.uiGameplay.sliderHealth.value) UIManager._instance.uiGameplay.SetHealthSliderValue(gameEvent.curHP);

        int dmg = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].dmgRecieved;
        int hpGain = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].hpGain;
        
        if (dmg == 0 && hpGain==0) return;
        
        SendData<ShareAttrEventDamaged> ev = new SendData<ShareAttrEventDamaged>(new ShareAttrEventDamaged(dmg-hpGain));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].dmgRecieved = 0;
        AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].hpGain = 0;
    }

    public override void End(GameEvent gameEvent)
    {
        base.End(gameEvent);
        AllManager._instance.playerManager.ProcessExpGain(AllManager._instance.playerManager.expRequire);
        Debug.Log(AllManager._instance.playerManager.GetMaxHealthFromLevel());
    }
}
