public delegate void BasicCallBack();
public delegate void BoolCallBack(bool exist);
public delegate void SaveCallBack(string id, int CharIndex, bool throughTown);
public delegate void GameInCallBack(GameName gameName);

public delegate bool BoolEvent();

public class EventReceiver
{
    //Title Event
    static public BoolCallBack saveCheck = null;
    static public BasicCallBack init = null;
    static public BasicCallBack selectDone = null;
    static public BasicCallBack touchScreen = null;
    static public SaveCallBack save = null;

    //Town Event
    static public GameInCallBack gameIn = null;
    static public BasicCallBack gameOut = null;

    //Title Event Call Back
    static public void CallSaveCheck(bool exist) => saveCheck?.Invoke(exist);
    static public void CallInit() => init?.Invoke();
    static public void CallSelectDone() => selectDone?.Invoke();
    static public void CallTouchScreen() => touchScreen?.Invoke();
    static public void CallSave(string id,int charIndex, bool throughTown) => save?.Invoke(id, charIndex, throughTown);

    //Town Event Call Back
    static public void CallGameIn(GameName gameName) => gameIn?.Invoke(gameName);
    static public void CallGameOut() => gameOut?.Invoke();
}