using UnityEngine;

public delegate void BasicCallBack();
public delegate void IntCallBack(int value);
public delegate void ImmeCallBack(float timing, float speed);
public delegate void VectorCallBack(Vector3 pos);

public delegate void LoadCallBack(string id);
public delegate void SaveCallBack(int level, long score);

public class EventReceiver
{
    //===============================main game delegate==================================
    static public BasicCallBack NewGuestR = null;
    static public BasicCallBack NewGuestL = null;
    static public BasicCallBack NewOrderR = null;
    static public BasicCallBack NewOrderL = null;
    static public IntCallBack ScoreModi = null;
    static public IntCallBack ScoreModiR = null;
    static public IntCallBack ScoreModiL = null;
    static public BasicCallBack DoSubmissionR = null;
    static public BasicCallBack DoSubmissionL = null;

    static public VectorCallBack CorrectIngred = null;
    static public VectorCallBack WrongIngred = null;
    static public BasicCallBack CorrectFood = null;
    static public IntCallBack WrongFood = null;

    static public BasicCallBack TickCount = null;

    static public BasicCallBack SceneStart = null;

    static public BasicCallBack GameStart = null;
    static public ImmeCallBack GameImminent = null;
    static public BasicCallBack GamePause = null;
    static public BasicCallBack GameResume = null;
    static public BasicCallBack GameOver = null;

    //=============================Loading delegate======================================
    static public BasicCallBack ButtonClicked = null;
    static public BasicCallBack PlayerFall = null;
    static public BasicCallBack PlayerRise = null;

    //=============================Save Load delegate====================================
    static public SaveCallBack saveCallBack = null;
    static public LoadCallBack loadCallBack = null;

    //customer event
    static public void CallNewGuestR() => NewGuestR?.Invoke();
    static public void CallNewGuestL() => NewGuestL?.Invoke();
    static public void CallNewOrderR() => NewOrderR?.Invoke();
    static public void CallNewOrderL() => NewOrderL?.Invoke();

    //score event
    static public void CallScoreModiR(int value) => ScoreModiR?.Invoke(value);
    static public void CallScoreModiL(int value) => ScoreModiL?.Invoke(value);
    static public void CallDoSubmissionR() => DoSubmissionR?.Invoke();
    static public void CallDoSubmissionL() => DoSubmissionL?.Invoke();

    //ingred event
    static public void CallCorrectIngred(Vector3 pos) => CorrectIngred?.Invoke(pos);
    static public void CallWrongIngred(Vector3 pos) => WrongIngred?.Invoke(pos);
    static public void CallCorrectFood() => CorrectFood?.Invoke();
    static public void CallWrongFood(int wrongLevel) => WrongFood?.Invoke(wrongLevel);

    //timer event
    static public void CallTickCount() => TickCount?.Invoke();

    //scene event
    static public void CallSceneStart() => SceneStart?.Invoke();

    //game routine event
    static public void CallGameStart() => GameStart?.Invoke();
    static public void CallGameImminent(float timing, float speed) => GameImminent?.Invoke(timing, speed);
    static public void CallGamePause() => GamePause?.Invoke();
    static public void CallGameResume() => GameResume?.Invoke();
    static public void CallGameOver() => GameOver?.Invoke();

    //loading event
    static public void CallButtonClicked() => ButtonClicked?.Invoke();
    static public void CallPlayerFall() => PlayerFall?.Invoke();
    static public void CallPlayerRise() => PlayerRise?.Invoke();

    //save load event
    static public void CallSaveCallBack(int level, long score) => saveCallBack?.Invoke(level,score);
    static public void CallLoadCallBack(string id) => loadCallBack?.Invoke(id);
}