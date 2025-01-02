using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinImgPositioning : MonoBehaviour
{
    public RectTransform numberImageParent; 
    public RectTransform coinImage;       
    public float spacing = 50;
    static public CoinImgPositioning Instance;
    float totalWidth = 0f;
    int activeImageCount = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void UpdatePosition()
    {
        activeImageCount = 0;
        totalWidth = 0f;    

        foreach (RectTransform child in numberImageParent)
        {
            if (child.gameObject.activeSelf) // Ȱ��ȭ�� �̹����� ���
            {
                totalWidth += child.rect.width * child.lossyScale.x; // ������ ���
                activeImageCount++;
            }
        }

        if (activeImageCount == 1)
            return;

        // CoinImage�� ��ġ�� ���� ���
        Vector3 newPosition = Vector3.zero; // �ʱ� ��ġ�� ����
        newPosition.x = totalWidth + (spacing * (activeImageCount - 1)); // �� �ʺ� + ����
        coinImage.localPosition = newPosition;

    }
}