using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public Toggle toggle; // ������ Toggle
    public Image backgroundImage; // Background �̹���
    public Image checkmarkImage; // Checkmark �̹���
    public Sprite spriteOn; // Ȱ�� ���� ��������Ʈ
    public Sprite spriteOff; // ��Ȱ�� ���� ��������Ʈ

    private void Start()
    {
        UpdateSprites(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateSprites);
    }

    private void UpdateSprites(bool isOn)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = isOn ? null : spriteOff; // Off ���� ��������Ʈ
        }

        if (checkmarkImage != null)
        {
            checkmarkImage.sprite = isOn ? spriteOn : null; // On ���� ��������Ʈ
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(UpdateSprites);
    }
}
