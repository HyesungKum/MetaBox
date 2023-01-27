using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField] private Vertex[] vertexes = null;
    [SerializeField] private string vertexName = "";

    public Vertex MoveVertex(int index) => vertexes[index];
    public string GetNodeName() => vertexName;
    public int GetNodeLength() => vertexes.Length;
    public string GetNextNodeName(int index) => vertexes[index].GetNodeName();

    SpriteRenderer sprite = null;
    Color originalColor;

    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    public void StartPointColor()
    {
        sprite.color = Color.blue;
    }
    public void ColorChange()
    {
        sprite.color = Color.green;
    }
    public void BackOriginalColor()
    {
        sprite.color = originalColor;
    }
}