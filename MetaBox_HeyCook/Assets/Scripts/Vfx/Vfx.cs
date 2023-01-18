using UnityEngine;
using System.Collections;
using ObjectPoolCP;

[RequireComponent(typeof(SpriteRenderer))]
public class Vfx : MonoBehaviour
{
    //====================ref components================
    [Header("ref componetns")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Texture Infomation")]
    public Sprite[] frames;
    public float fps = 30.0f;

    [Header("Play Duration")]
    [SerializeField] float duration;

    //====================inner varables=================
    private float timer = 0f;
    private float frameTimer = 0f;
    private int frameIndex = 0;

    private void OnEnable()
    {
        timer = 0f;
        frameTimer = 0f;
        frameIndex = 0;

        StartCoroutine(nameof(Repeat));
    }

    /// <summary>
    /// Repeat Sprite Changing each fps, until duration end 
    /// </summary>
    IEnumerator Repeat()
    {
        while(timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            frameTimer += Time.unscaledDeltaTime;

            //image changing
            if ((1f / fps) < frameTimer)
            {
                NextFrame();
                frameTimer = 0f;
            }

            yield return null;
        }

        PoolCp.Inst.DestoryObjectCp(this.gameObject);
    }
    /// <summary>
    /// sprite change next index
    /// </summary>
    void NextFrame()
    {
        spriteRenderer.sprite = frames[frameIndex];
        frameIndex = (frameIndex + 1) % frames.Length;
    }
}