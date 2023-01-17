using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelSet : MonoBehaviour
{
    [SerializeField] Button backStartPanelBut = null;
    [SerializeField] GameObject startPanel = null;

    [Header("[Audio Slider]")]
    [SerializeField] Slider bgmSlider = null;
    [SerializeField] Slider audioSlider = null;

    // ======= ���� ���� ���� ��� �߰� =====

    void Awake()
    {
        backStartPanelBut.onClick.AddListener(() => OnClickBakcButton());
    }

    void OnClickBakcButton()
    {
        startPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
