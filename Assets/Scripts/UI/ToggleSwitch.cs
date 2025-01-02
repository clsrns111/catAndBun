using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public Toggle toggle; // 연결할 Toggle
    public Image backgroundImage; // Background 이미지
    public Image checkmarkImage; // Checkmark 이미지
    public Sprite spriteOn; // 활성 상태 스프라이트
    public Sprite spriteOff; // 비활성 상태 스프라이트

    private void Start()
    {
        UpdateSprites(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateSprites);
    }

    private void UpdateSprites(bool isOn)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = isOn ? null : spriteOff; // Off 상태 스프라이트
        }

        if (checkmarkImage != null)
        {
            checkmarkImage.sprite = isOn ? spriteOn : null; // On 상태 스프라이트
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(UpdateSprites);
    }
}
