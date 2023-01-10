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

    

    SpriteRenderer sprite = null;
    Color originalColor;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        //Debug.Log("## 기본 설정 컬러 : "+originalColor.ToString());

        node = new Node(vertexes);
    }

    public void StartPointColor()
    {
        sprite.color = Color.blue;
    }

    public void ColorChange()
    {
        //=== unity supplied default color values ===
        sprite.color = Color.green;
        //Debug.Log("## 변경된 컬러 : " + changedColor);

        // === chang designeated Color ===
        //float r = 0.6228678f;
        //float g = 0.8867924f;
        //float b = 0.4475792f;
        //float a = 1f;

        //sprite.color = new Color(r, g, b, a);
    }

    public void BackOriginalColor()
    {
        sprite.color = originalColor;
    }
}
