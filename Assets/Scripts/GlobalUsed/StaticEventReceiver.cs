using UnityEngine;

public delegate void BasicCallBack();
public delegate void GameInCallBack(GameName gameName);

public class EventReceiver
{
    static public GameInCallBack gameIn = null;
    static public BasicCallBack gameOut = null;

    static public void CallGameIn(GameName gameName) => gameIn?.Invoke(gameName);
    static public void CallGameOut() => gameOut?.Invoke();
}