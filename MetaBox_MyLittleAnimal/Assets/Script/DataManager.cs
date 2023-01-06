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

    void loadGameData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PlayerData");

        // �������� ��� ������ �����ϰ� �ִ�.
        string[] lines = ta.text.Split("\r\n");
        // �ݺ����� 1���� �� ������ ����� �����ϱ� ���ؼ�
        for (int i = 1; i < lines.Length - 1; ++i)
        {
            // ������ 1���� �ĸ��� �����Ѵ�.
            string[] columes = lines[i].Split(',');

            PlayerData playerData = new PlayerData();
            playerData.Level = int.Parse(columes[0]);   // ����
            playerData.Exp = int.Parse(columes[1]);     // ����ġ ������
            playerData.AttackPower = float.Parse(columes[2]);  // ���ݷ�
            playerData.MaxHP = int.Parse(columes[3]);  // max hp

            listPlayerData.Add(playerData);

            //Debug.Log($"���͵����� {i} : Level {mobData.Level} Name {mobData.Name} AP {mobData.AttackPower} MaxHP {mobData.MaxHP}");
        }
    }

    //
    // ���� ������
    void loadMonsterData()
    {
        TextAsset ta = Resources.Load<TextAsset>("MonsterData");

        string[] lines = ta.text.Split("\r\n");
        for (int i = 1; i < lines.Length - 1; ++i)
        {
            // ������ 1���� �ĸ��� �����Ѵ�.
            string[] columes = lines[i].Split(',');

            MonsterData mobData = new MonsterData();
            mobData.Level = int.Parse(columes[0]); // ����
            mobData.Name = columes[1].Trim('\"');  // �̸�  : "ddd" -> ddd
            mobData.AttackPower = float.Parse(columes[2]);  // ���ݷ�
            mobData.MaxHP = int.Parse(columes[3]);  // max hp

            listMonsterData.Add(mobData);

            //Debug.Log($"���͵����� {i} : Level {mobData.Level} Name {mobData.Name} AP {mobData.AttackPower} MaxHP {mobData.MaxHP}");
        }
    }




    //
    //
    // ������ �˻��ϴ� �Լ���..
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

    // ����ġ�� �������� �÷��̾� ������ ���ϱ�
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
