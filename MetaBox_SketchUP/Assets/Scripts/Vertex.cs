using NODE;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField] private Vertex[] vertexes = null;
    Node node = null;
    [SerializeField] private string nodeName = "";

    public string GetNodeName() => nodeName;
    public int GetNodeLength() => vertexes.Length;
    public string GetNextNodeName(int index) => vertexes[index].GetNodeName();

    void Start()
    {
        node = new Node(vertexes);
    }
}
