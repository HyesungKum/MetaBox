using UnityEngine;

public delegate void BasicCallBack();
public delegate void IntCallBack(int value);
public delegate void VectorCallBack(Vector3 pos);

static public class EventReciver
{
    //===============================main game delegate==================================
    static public BasicCallBack NewCostomer = null;
    static public BasicCallBack NewOrder = null;
    static public IntCallBack ScoreModi = null;
    static public BasicCallBack DoSubmission = null;

    static public VectorCallBack CorrectIngred = null;
    static public VectorCallBack WrongIngred = null;
    static public BasicCallBack CorrectFood = null;
    static public IntCallBack WrongFood = null;

    static public BasicCallBack TickCount = null;

    static public BasicCallBack SceneStart = null;

    static public BasicCallBack GameStart = null;
    static public BasicCallBack GamePause = null;
    static public BasicCallBack GameResume = null;
    static public BasicCallBack GameOver = null;

    //=============================Loading delegate======================================
    static public BasicCallBack ButtonClicked;

    //customer event
    static public void CallNewComstomer() => NewCostomer?.Invoke();
    static public void CallNewOrder() => NewOrder?.Invoke();

    //score event
    static public void CallScoreModi(int value) => ScoreModi?.Invoke(value);
    static public void CallDoSubmission() => DoSubmission?.Invoke();

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
    static public void CallGamePause() => GamePause?.Invoke();
    static public void CallGameResume() => GameResume?.Invoke();
    static public void CallGameOver() => GameOver?.Invoke();

    //loading event
    static public void CallButtonClicked() => ButtonClicked?.Invoke();
}