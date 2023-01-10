using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameData
{
    public int level;
    public int gameGroup;
    public int stageGroup;
    public int stageCount;
    public int playTime;
    public int playerSpeed;
    public int playerArea;
}

public struct StageData
{
    public int stageGroup;
    public int thiefGroup;
    public int thiefCount;
    public int wantedCount;
    public int startCountdown;
    public int penaltyPoint;
}

public struct ThiefData
{
    public int id;
    public int thiefGroup;
    public int moveSpeed;
    public int moveTime;
}

public class DataManager
{
    #region 싱글턴
    static DataManager instance = null;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }
    #endregion

    List<GameData> listGameData = new List<GameData>();
    List<StageData> listStageData = new List<StageData>();
    List<ThiefData> listThiefData = new List<ThiefData>();

    /*
    public void LoadGameData()
    {
        loadGameData();
        loadStageData();
        loadThiefData();
    }

    public static class MyExtentions
{
    public static void Recycle(this GameObject go)
    {
        Monster mob = go.GetComponent<Monster>();
        MonsterPool.Inst.DestroyMonster(mob);
    }
    
}

    [System.Serializable]
public class SpawnInfo
{
	public int MobLevel = 1;
	public int Count = 3;
}

public class SpawnArea_Ver2 : MonoBehaviour
{
	[Header("[영역정보]")]
	[SerializeField] float width = 1f; // X 축 기준으로 크기
	[SerializeField] float height = 1f; // Z 축 기준으로 크기
	[Header("[몬스터정보]")]
	[SerializeField] float spawnInterval = 3f;
	[SerializeField] SpawnInfo[] SpawnInfos = null;
	//[SerializeField] Monster prefabMob = null;   // 몬스터 프리팹

	// Start is called before the first frame update
	void Start()
	{
		GameDataMgr.Inst.LoadGameData();
	}


	//
	// GameMgr 가 Wave 시작할 때, waveNum 을 입력해서 호출해 준다.
	public void Go(int waveNum)
	{
		StartCoroutine(processSpawn(waveNum));
	}

	//
	// 일정시간 마다 몬스터를 생성(스폰)한다.
	IEnumerator processSpawn(int waveNum)
	{
		for(int i = 0; i < SpawnInfos[waveNum-1].Count; ++i)
		{
			// 몬스터풀에 몬스터 1개를 생성 요청
			Monster mob = MonsterPool.Inst.CreateMonster(getRandomPos());
			mob.SetData(GameDataMgr.Inst.FindMonsterDataBy(SpawnInfos[waveNum - 1].MobLevel));

			// spawnInterval 만큼 기다리기 
			yield return new WaitForSeconds(spawnInterval);
		}
	}





	// 
	// 
	Vector3 getRandomPos()
	{
		Vector3 size = transform.lossyScale;
		size.x *= width;
		size.z *= height;
		// 행렬(위치이동, 회전, 스케일)을 이용해서 랜덤위치의 정확한 값을 계산한다.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);

		Vector3 randomPos = rMat.MultiplyPoint(new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)));
		randomPos.y = 0.0f;
		return randomPos;
	}
	// 에디터에서만 동작하도록
#if UNITY_EDITOR
	// 기즈모를 그릴 때 호출되는 함수
	private void OnDrawGizmos()
	{
		drawCube(Color.yellow);
	}

	// 기즈모가 선택이 되었을 때 호출되는 함수
	void OnDrawGizmosSelected()
	{
		drawCube(Color.green);
	}

	//
	// 지정된 색상으로 큐브 1개 그리기
	void drawCube(Color drawColor)
	{
		Gizmos.color = drawColor;
		Vector3 size = transform.lossyScale;
		size.x *= width;
		size.z *= height;

		// 위치와 회전과 스케일이 적용된 행렬을 구해서
		// Gizmos 에 적용하면 이후 그리는 Cube는 행렬의 영향(위치이동, 회전, 스케일)을 받는다.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);
		Gizmos.matrix = rMat;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
#endif // UNITY_EDITOR
}

    void loadGameData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PlayerData");

        // 데이터의 모든 라인을 포함하고 있다.
        string[] lines = ta.text.Split("\r\n");
        // 반복문을 1부터 한 이유는 헤더를 제외하기 위해서
        for (int i = 1; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분한다.
            string[] columes = lines[i].Split(',');

            PlayerData playerData = new PlayerData();
            playerData.Level = int.Parse(columes[0]);   // 레벨
            playerData.Exp = int.Parse(columes[1]);     // 경험치 누적값
            playerData.AttackPower = float.Parse(columes[2]);  // 공격력
            playerData.MaxHP = int.Parse(columes[3]);  // max hp

            listPlayerData.Add(playerData);

            //Debug.Log($"몬스터데이터 {i} : Level {mobData.Level} Name {mobData.Name} AP {mobData.AttackPower} MaxHP {mobData.MaxHP}");
        }
    }

    //
    // 몬스터 데이터
    void loadMonsterData()
    {
        TextAsset ta = Resources.Load<TextAsset>("MonsterData");

        string[] lines = ta.text.Split("\r\n");
        for (int i = 1; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분한다.
            string[] columes = lines[i].Split(',');

            MonsterData mobData = new MonsterData();
            mobData.Level = int.Parse(columes[0]); // 레벨
            mobData.Name = columes[1].Trim('\"');  // 이름  : "ddd" -> ddd
            mobData.AttackPower = float.Parse(columes[2]);  // 공격력
            mobData.MaxHP = int.Parse(columes[3]);  // max hp

            listMonsterData.Add(mobData);

            //Debug.Log($"몬스터데이터 {i} : Level {mobData.Level} Name {mobData.Name} AP {mobData.AttackPower} MaxHP {mobData.MaxHP}");
        }
    }




    //
    //
    // 데이터 검색하는 함수들..
    public MonsterData FindMonsterDataBy(int level)
    {
        MonsterData mobData = listMonsterData.Find(mobData => mobData.Level == level);
        return mobData;
    }

    public PlayerData FindPlayerDataBy(int level)
    {
        PlayerData playerData = listPlayerData.Find(pData => pData.Level == level);
        return playerData;
    }

    // 경험치를 기준으로 플레이어 데이터 구하기
    public PlayerData FindPlayerDataByExp(int exp)
    {
        for (int i = 0; i < listPlayerData.Count; ++i)
        {
            if (listPlayerData[i].Exp >= exp)
            {
                return listPlayerData[i];
            }
        }

        return listPlayerData[listPlayerData.Count - 1];
    }
    */
}
