using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DeadZone))]
[RequireComponent(typeof(SpriteRenderer))]
public class TrashBin : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private DeadZone _deadZone;

    [Header("Bin Image")]
    [SerializeField] private Sprite[] binSprites;
    
    [Header("Setting")]
    [SerializeField] private float closeTime = 0.4f;

    private void Awake()
    {
        TryGetComponent(out _renderer);
        TryGetComponent(out _deadZone);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BinOpen();
        Invoke(nameof(BinClose), closeTime);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        BinClose();
    }
    void BinOpen()
    {
        _renderer.sprite = binSprites[1];
    }
    void BinClose()
    {
        if (_deadZone.targetIngreds.Count == 0)
        {
            _renderer.sprite = binSprites[0];
        }
    }
}
