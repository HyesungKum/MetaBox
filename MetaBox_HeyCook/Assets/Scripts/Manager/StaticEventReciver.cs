using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public delegate void NewCostomer();
public delegate void NewOrder();
public delegate void ScoreModi(int value);
public delegate void DoSubmission();

public delegate void CorrectIngred(Vector3 pos);
public delegate void WrongIngred(Vector3 pos);
public delegate void CorrectFood();
public delegate void WrongFood(int wrongLevel);

public delegate void TickCount();

public delegate void GameSceneStart();
public delegate void GameSceneEnded();

public delegate void GameStart();
public delegate void GamePause();
public delegate void GameResume();
public delegate void GameOver();

static public class EventReciver
{
    static public NewCostomer NewCostomer = null;
    static public NewOrder NewOrder = null;
    static public ScoreModi ScoreModi = null;
    static public DoSubmission DoSubmission = null;

    static public CorrectIngred CorrectIngred = null;
    static public WrongIngred WrongIngred = null;
    static public CorrectFood CorrectFood = null;
    static public WrongFood WrongFood = null;

    static public TickCount TickCount = null;

    static public GameSceneStart GameSceneStart = null;
    static public GameSceneEnded GameSceneEnded = null;

    static public GameStart GameStart = null;
    static public GamePause GamePause = null;
    public static GameResume GameResume = null;
    static public GameOver GameOver = null;

    static public void CallNewComstomer() => NewCostomer?.Invoke();
    static public void CallNewOrder() => NewOrder?.Invoke();
    static public void CallScoreModi(int value) => ScoreModi?.Invoke(value);
    static public void CallDoSubmission() => DoSubmission?.Invoke();

    static public void CallCorrectIngred(Vector3 pos) => CorrectIngred?.Invoke(pos);
    static public void CallWrongIngred(Vector3 pos) => WrongIngred?.Invoke(pos);
    static public void CallCorrectFood() => CorrectFood?.Invoke();
    static public void CallWrongFood(int wrongLevel) => WrongFood?.Invoke(wrongLevel);

    static public void CallTickCount() => TickCount?.Invoke();

    static public void CallGameSceneStart() => GameSceneStart?.Invoke();
    static public void CallGameSceneEnded() => GameSceneEnded?.Invoke();

    static public void CallGameStart() => GameStart?.Invoke();
    static public void CallGamePause() => GamePause?.Invoke();
    static public void CallGameResume() => GameResume?.Invoke();
    static public void CallGameOver() => GameOver?.Invoke();
}