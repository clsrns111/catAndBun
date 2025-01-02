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
            if (child.gameObject.activeSelf) // 활성화된 이미지만 계산
            {
                totalWidth += child.rect.width * child.lossyScale.x; // 스케일 고려
                activeImageCount++;
            }
        }

        if (activeImageCount == 1)
            return;

        // CoinImage의 위치를 새로 계산
        Vector3 newPosition = Vector3.zero; // 초기 위치를 고정
        newPosition.x = totalWidth + (spacing * (activeImageCount - 1)); // 총 너비 + 간격
        coinImage.localPosition = newPosition;

    }
}