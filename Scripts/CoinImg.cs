using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinImg : MonoBehaviour
{
    [SerializeField] Image image;
    public float scaleUpFactor = 1.5f;   // 커지는 배율
    public float duration = 0.2f;         // 애니메이션 시간
    Vector3 originalScale;
    Vector3 targetScale;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform 가져오기
        originalScale = rectTransform.localScale;  // 원래 크기 저장
        targetScale = originalScale * scaleUpFactor;  // 목표 크기
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public IEnumerator PopCoinImg()
    {
        Vector3 originalPos = rectTransform.position;  // 원래 위치 저장

        float time = 0f;

        // 크기가 커지기
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            rectTransform.position = originalPos;  // 위치를 고정
            time += Time.deltaTime;
            yield return null;
        }

        // 크기 최대로 커진 후, 원래 크기로 돌아가기
        time = 0f;
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
            rectTransform.position = originalPos;  // 위치를 고정
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = originalScale;  // 정확하게 원래 크기로
        rectTransform.position = originalPos;  // 위치 고정
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
