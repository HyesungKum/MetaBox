public delegate void BasicCallBack();
public delegate void BoolCallBack(bool exist);
public delegate void SaveCallBack(string id, int CharIndex, bool throughTown);
public delegate void GameInCallBack(GameName gameName);

public delegate bool BoolEvent();

public class EventReceiver
{
    //Title Event
    static public BasicCallBack initEvent = null;
    static public BasicCallBack mainEvnet = null;

    static public BasicCallBack loadingDone = null;
    
    static public BasicCallBack selectEvent = null;
    static public BasicCallBack touchScreen = null;
    static public SaveCallBack saveEvent = null;
    static public BasicCallBack saveDoneEvent = null; 

    //Town Event
    static public BasicCallBack unloadScene = null;
    static public GameInCallBack gameIn = null;
    static public BasicCallBack gameOut = null;

    //application event
    static public BasicCallBack appQuit = null;

    //Title Event Call Back
    static public void CallInitEvent() => initEvent?.Invoke();
    static public void CallMainEvent() => mainEvnet?.Invoke();
    static public void CallLodingDone() => loadingDone?.Invoke();
    static public void CallSelectEvent() => selectEvent?.Invoke();
    static public void CallSaveEvent(string id, int charIndex, bool throughTown) => saveEvent?.Invoke(id, charIndex, throughTown);
    static public void CallSaveDoneEvent() => saveDoneEvent?.Invoke();
    static public void CallTouchScreen() => touchScreen?.Invoke();


    //Town Event Call Back
    static public void CallUnloadScene() => unloadScene?.Invoke();
    static public void CallGameIn(GameName gameName) => gameIn?.Invoke(gameName);
    static public void CallGameOut() => gameOut?.Invoke();

    //application event
    static public void CallAppQuit() => appQuit?.Invoke();
}