using System;
using UnityEditor;
using UnityEngine;

//1.Custom Node
//해당 위치의 좌표와  f cost, g cost, h cost A*알고리즘 연산에 필요한 가중치 그리고 인접 노드를 Linked List형식으로 참조하고 있음

//2.Custom Nav Mesh 
//초기에 Nav Mesh 오브젝트의 크기 및 해상도 확인

//이에 맞는 노드 개수 초기 설정

//해당 오브젝트 안에 포함된 Child 객체의 collider 컴포넌트들을 확인하여 해당 위치에 있는 Custom Node는 가중치를 높게 가지도록 설정
//(현재는 그냥 position Data 할당 안함)

//custom editor을 이용해 component에 만든 Bake 버튼을 누르면

//길찾기 알고리즘에 사용할 수 있도록 Scriptable Object 안에 CustomNode 정보가 배열로 저장됨

//노드는 일정 간격으로 생성되지만 가중치를 다르게 주는 것으로 수정

//노드를 먼저 생성해서 scriptable 오브젝트로 가지고 있고 이후 awake에서 neibhor node 설정
//이유는 serializable 계층이 깊어져 버그 발생 -> ex)0,0노드의 우측노드는 0,1 0ㅣ0,1노드의 좌측노드는 0,0 반복되어 참조관계가 무한히 이어짐


[RequireComponent(typeof(BoxCollider2D))]
public class NavMesh2D : MonoBehaviour
{
    //===============================================Settings================================================
    [Header("Path Node Resolution")]
    public float res = 1;

    [Header("Nav Mesh Reference")]
    [HideInInspector] public Collider2D _collider;
    public NavMesh2Data navMeshData;

    //==============================================Node Index=============================================== 
    [field: SerializeField] public int ResX { get; private set; }
    [field: SerializeField] public int ResY { get; private set; }

    //==============================================Delegate=================================================
    public delegate void ConnectFinish();
    public ConnectFinish connectFinish;
    public void CallFinished() => connectFinish?.Invoke();

    //=============================Reference Other Component when Attach this component======================
    private void Reset()
    {
        TryGetComponent(out _collider);
    }

    //=======================================Setting Each Neibhor Node========================================
    private void Awake()
    {
        #region null exception handling 
        if (navMeshData == null)
        {
            Debug.Log("##Nav Mesh 2D Error : Nav Mesh Data is Null, Plz reBake or Reference Nav Mesh 2 Data");
            return;
        }
        #endregion

        //setting neibhor node
        try
        {
            for (int i = 0; i < ResX; i++)
            {
                for (int j = 0; j < ResY; j++)
                {
                    NavNode instNode = navMeshData.NavMesh2DVer[i].customNodes[j];

                    if (i != 0)//좌측이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i - 1].customNodes[j], NodeDirection.West);
                    }

                    if (i != 0 && j != 0)//좌상단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i - 1].customNodes[j - 1], NodeDirection.WestNorth);
                    }

                    if (j != 0)//상단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i].customNodes[j - 1], NodeDirection.North);
                    }

                    if (i != ResX - 1 && j != 0)//우상단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i + 1].customNodes[j - 1], NodeDirection.NorthEast);
                    }

                    if (i != ResX - 1)//우측이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i + 1].customNodes[j], NodeDirection.East);
                    }

                    if (i != ResX - 1 && j != ResY - 1) //우하단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i + 1].customNodes[j + 1], NodeDirection.EastSouth);
                    }

                    if (j != ResY - 1)//하단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i].customNodes[j + 1], NodeDirection.South);
                    }

                    if (i != 0 && j != ResY - 1)//하단이 아닐때
                    {
                        instNode.SetNeighbor(navMeshData.NavMesh2DVer[i - 1].customNodes[j + 1], NodeDirection.SouthWest);
                    }
                }
            }
        }
        catch(Exception e)
        {
            #if UNITY_EDITOR
            Debug.Log("##Nav Mesh 2D Error : node setting didnt match mesh" + e);
            #endif
        }

        //call finished
        CallFinished();
    }


    //======================================make nav mesh and reference========================================
    public void Bake()
    {
        //Create Scriptable Assets Instance
        NavMesh2Data navMeshData = ScriptableObject.CreateInstance<NavMesh2Data>();

        //Create 2X2 array
        navMeshData.NavMesh2DVer = new NavMesh2Hor[ResX];
        for (int i = 0; i < ResX; i++)
        {
            navMeshData.NavMesh2DVer[i] = new NavMesh2Hor();

            for (int j = 0; j < navMeshData.NavMesh2DVer.Length; j++)
            {
                navMeshData.NavMesh2DVer[i].customNodes = new NavNode[ResY];
            }
        }

        //continous pitch each node distance and pivot
        float stepX = transform.localScale.x / ResX;
        float halfX = transform.localScale.x * 0.5f;

        float stepY = transform.localScale.y / ResY;
        float halfY = transform.localScale.y * 0.5f;

        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < ResX; i++)
        {
            for (int j = 0; j < ResY; j++)
            {
                Vector3 pos = new(-halfX + stepX * i, -halfY + stepY * j, 0);
                averagePos += pos;
            }
        }
        averagePos /= ResX * ResY;


        //check obstacle position
        NavObstacle2D[] obstacles = GetComponentsInChildren<NavObstacle2D>();
        Collider2D[] colliders = new Collider2D[obstacles.Length];

        for (int i = 0; i < obstacles.Length; i++)
        {
            colliders[i] = obstacles[i]._collider;
        }

        for (int i = 0; i < ResX; i++)
        {
            for (int j = 0; j < ResY; j++)
            {
                Vector3 pos = new(-halfX + stepX * i, -halfY + stepY * j, 0);

                bool inCollider = false;
                foreach (Collider2D collider in colliders)
                {
                    Vector3 pos1 = transform.position + pos - averagePos;
                    if (collider.bounds.Contains(pos1))
                    {
                        inCollider = true;
                        break;
                    }
                }

                navMeshData.NavMesh2DVer[i].customNodes[ResY - 1 - j] = new NavNode(transform.position + pos - averagePos);
                navMeshData.NavMesh2DVer[i].customNodes[ResY - 1 - j].x = i;
                navMeshData.NavMesh2DVer[i].customNodes[ResY - 1 - j].y = ResY - 1 - j;

                //set obstacle
                if (inCollider)
                {
                    navMeshData.NavMesh2DVer[i].customNodes[ResY - 1 - j].obstacle = true;
                }
            }
        }

        //Save Nav Mesh Assets
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/NavMesh2Data.asset");
        AssetDatabase.CreateAsset(navMeshData, path);
        AssetDatabase.SaveAssets();

        //Nav Mesh Reference
        this.navMeshData = navMeshData;
    }

    //============================================Poisition Check Function=====================================
    /// <summary>
    /// Get Nav Mesh Node corresponding to Position
    /// </summary>
    /// <param name="position">target position</param>
    /// <returns>Custom Node</returns>
    public NavNode PosToNode(Vector3 position)
    {
        if (navMeshData == null) Debug.Log("##Nav Mesh Error : Mesh Data is Null, Bake New Mesh or Reference");
        if (!CheckPosOnMesh(position)) return null;
    
        //find current node
        int x = (int)((position.x - _collider.bounds.min.x) * ResX / this.transform.localScale.x);
        int y = (int)((_collider.bounds.max.y - position.y) * ResY / this.transform.localScale.y);
    
        //exception of edge
        if (x >= ResX || x < 0) return null;
        if (y >= ResY || y < 0) return null;
    
        //apply current Node
        return navMeshData.NavMesh2DVer[x].customNodes[y];
    }
    /// <summary>
    /// check position on nav mesh area
    /// </summary>
    /// <param name="position"> target position </param>
    /// <returns> IsField is that position on Field </returns>
    public bool CheckPosOnMesh(Vector3 position)
    {
        position = new Vector3(position.x, position.y, this.transform.position.z);

        if (navMeshData == null) return false;

        if (_collider.bounds.Contains(position)) return true;
        else return false;
    }

    #region Editor Button & gizmo
#if UNITY_EDITOR
    //========================================Create Inspector "Bake" Button======================================
    [CustomEditor(typeof(NavMesh2D))]
    public class NavMesh2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            NavMesh2D navMesh = (NavMesh2D)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Bake"))
            {
                navMesh.Bake();
            }
        }
    }

    //==========================================Viewing path findalbe area========================================
    private void OnDrawGizmos()
    {
        if (res > 20) res = 20;

        ResX = (int)(transform.localScale.x * res);
        ResY = (int)(transform.localScale.y * res);

        float stepX = transform.localScale.x / ResX;
        float stepY = transform.localScale.y / ResY;
        float halfX = transform.localScale.x * 0.5f;
        float halfY = transform.localScale.y * 0.5f;

        Vector3 averagePos = Vector3.zero;

        for (int i = 0; i < ResX; i++)
        {
            for (int j = 0; j < ResY; j++)
            {
                Vector3 pos = new(-halfX + stepX * i, -halfY + stepY * j, 0);
                averagePos += pos;
            }
        }

        averagePos /= ResX * ResY;

        Gizmos.color = new Color(0.2f, 1f, 0.2f);

        NavObstacle2D[] obstacles = GetComponentsInChildren<NavObstacle2D>();
        Collider2D[] colliders = new Collider2D[obstacles.Length];

        for (int i = 0; i < obstacles.Length; i++)
        {
            colliders[i] = obstacles[i]._collider;
        }

        for (int i = 0; i < ResX; i++)
        {
            for (int j = 0; j < ResY; j++)
            {
                Vector3 pos = new(-halfX + stepX * i, -halfY + stepY * j, 0);

                bool inCollider = false;
                foreach (Collider2D collider in colliders)
                {
                    
                    if (collider.bounds.Contains(transform.position + pos - averagePos))
                    {
                        inCollider = true;
                        break;
                    }
                }

                if (!inCollider)
                {
                    Gizmos.DrawSphere(transform.position + pos - averagePos, 0.2f / res);
                }
            }
        }

        Gizmos.color = new(0f, 0f, 1f, 0.4f);
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, this.transform.GetComponent<BoxCollider2D>().size);
    }
    #endif
    #endregion
}