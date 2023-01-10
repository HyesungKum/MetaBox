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
    #region �̱���
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

    
    public void LoadGameData()
    {
        loadGameData();
        loadStageData();
        loadThiefData();
    }

    void loadGameData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PoliceGame");

        string[] lines = ta.text.Split("\r\n");
        
        for (int i = 2; i < lines.Length - 2; ++i)
        {
            // ������ 1���� �ĸ��� ����
            string[] columes = lines[i].Split(',');

            GameData gameData = new GameData();
            gameData.level = int.Parse(columes[0]);
            gameData.gameGroup = int.Parse(columes[1]);
            gameData.stageGroup = int.Parse(columes[2]);
            gameData.stageCount = int.Parse(columes[3]);
            gameData.playTime = int.Parse(columes[4]);
            gameData.playerSpeed = int.Parse(columes[5]);
            gameData.playerArea = int.Parse(columes[6]);

            listGameData.Add(gameData);
        }
    }


    void loadStageData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PoliceStage");

        string[] lines = ta.text.Split("\r\n");

        for (int i = 2; i < lines.Length - 2; ++i)
        {
            // ������ 1���� �ĸ��� �����Ѵ�.
            string[] columes = lines[i].Split(',');

            StageData stageData = new StageData();
            stageData.stageGroup = int.Parse(columes[1]);
            stageData.thiefGroup = int.Parse(columes[2]);
            stageData.thiefCount = int.Parse(columes[3]);
            stageData.wantedCount = int.Parse(columes[4]);
            stageData.startCountdown = int.Parse(columes[5]);
            stageData.penaltyPoint = int.Parse(columes[6]);

            listStageData.Add(stageData);
        }
    }


    void loadThiefData()
    {
        TextAsset ta = Resources.Load<TextAsset>("Thieves");

        string[] lines = ta.text.Split("\r\n");

        for (int i = 2; i < lines.Length - 2; ++i)
        {
            // ������ 1���� �ĸ��� ����
            string[] columes = lines[i].Split(',');

            ThiefData thiefData = new ThiefData();
            thiefData.id = int.Parse(columes[0]);
            thiefData.thiefGroup = int.Parse(columes[2]);
            thiefData.moveSpeed = int.Parse(columes[3]);
            thiefData.moveTime = int.Parse(columes[4]);

            listThiefData.Add(thiefData);
        }
    }




    // ������ �˻��ϴ� �Լ�
    public GameData FindGameDataByLevel(int level)
    {
        GameData gameData = listGameData.Find(gameData => gameData.level == level);
        return gameData;
    }

    public List<StageData> FindStageDatasByStageGroup(int stageGroup, int stageCount)
    {
        List<StageData> stages = new List<StageData>();

        for (int i = 0; i < listStageData.Count; ++i)
        {
            if (listStageData[i].stageGroup == stageGroup)
            {
                stages.Add(listStageData[i]);
            }
        }

        while (stages.Count > stageCount)
        {
            stages.RemoveAt(Random.Range(0, stages.Count));
        }
        return stages;
    }



    /*
     * 
     * public class SpawnArea_Ver2 : MonoBehaviour
{
	[Header("[��������]")]
	[SerializeField] float width = 1f; // X �� �������� ũ��
	[SerializeField] float height = 1f; // Z �� �������� ũ��
	[Header("[��������]")]
	[SerializeField] float spawnInterval = 3f;
	[SerializeField] SpawnInfo[] SpawnInfos = null;
	//[SerializeField] Monster prefabMob = null;   // ���� ������

	// Start is called before the first frame update
	void Start()
	{
		GameDataMgr.Inst.LoadGameData();
	}


	//
	// GameMgr �� Wave ������ ��, waveNum �� �Է��ؼ� ȣ���� �ش�.
	public void Go(int waveNum)
	{
		StartCoroutine(processSpawn(waveNum));
	}

	//
	// �����ð� ���� ���͸� ����(����)�Ѵ�.
	IEnumerator processSpawn(int waveNum)
	{
		for(int i = 0; i < SpawnInfos[waveNum-1].Count; ++i)
		{
			// ����Ǯ�� ���� 1���� ���� ��û
			Monster mob = MonsterPool.Inst.CreateMonster(getRandomPos());
			mob.SetData(GameDataMgr.Inst.FindMonsterDataBy(SpawnInfos[waveNum - 1].MobLevel));

			// spawnInterval ��ŭ ��ٸ��� 
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
		// ���(��ġ�̵�, ȸ��, ������)�� �̿��ؼ� ������ġ�� ��Ȯ�� ���� ����Ѵ�.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);

		Vector3 randomPos = rMat.MultiplyPoint(new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)));
		randomPos.y = 0.0f;
		return randomPos;
	}
	// �����Ϳ����� �����ϵ���
#if UNITY_EDITOR
	// ����� �׸� �� ȣ��Ǵ� �Լ�
	private void OnDrawGizmos()
	{
		drawCube(Color.yellow);
	}

	// ����� ������ �Ǿ��� �� ȣ��Ǵ� �Լ�
	void OnDrawGizmosSelected()
	{
		drawCube(Color.green);
	}

	//
	// ������ �������� ť�� 1�� �׸���
	void drawCube(Color drawColor)
	{
		Gizmos.color = drawColor;
		Vector3 size = transform.lossyScale;
		size.x *= width;
		size.z *= height;

		// ��ġ�� ȸ���� �������� ����� ����� ���ؼ�
		// Gizmos �� �����ϸ� ���� �׸��� Cube�� ����� ����(��ġ�̵�, ȸ��, ������)�� �޴´�.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);
		Gizmos.matrix = rMat;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
#endif // UNITY_EDITOR
}
    */
}
