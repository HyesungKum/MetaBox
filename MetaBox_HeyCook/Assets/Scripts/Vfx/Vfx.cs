using UnityEngine;
using System.Collections;
using ObjectPoolCP;

[RequireComponent(typeof(SpriteRenderer))]
public class Vfx : MonoBehaviour
{
    //====================ref components==============
    [Header("ref componetns")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Texture Infomation")]
    public Sprite[] frames;
    public float fps = 30.0f;

    [Header("Play Duration")]
    [SerializeField] float duration;

    //====================inner varables===============
    float timer = 0f;
    float frameTimer = 0f;
    private int frameIndex;

    private void OnEnable()
    {
        timer = 0f;
        frameTimer = 0f;

        NextFrame();
        StartCoroutine(nameof(Repeat));
    }

    /// <summary>
    /// Repeat Sprite Changing each fps, until duration end 
    /// </summary>
    IEnumerator Repeat()
    {
        while(true)
        {
            timer += Time.unscaledDeltaTime;
            frameTimer += Time.unscaledDeltaTime;

            //image changing
            if ((1f / fps) < frameTimer)
            {
                NextFrame();
                frameTimer = 0f;
            }

            //image vfx disable
            if (timer > duration)
            {
                PoolCp.Inst.DestoryObjectCp(this.gameObject);
                yield break;
            }

            yield return null;
        }
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