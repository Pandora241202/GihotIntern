using System;
[Serializable]
public class SendData<T>
{
    public string player_id = Player_ID.MyPlayerID;
    public T _event;
    public SendData(T _event)
    {
        this._event = _event;
    }
}