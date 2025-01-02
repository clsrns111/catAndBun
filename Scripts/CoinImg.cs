using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinImg : MonoBehaviour
{
    [SerializeField] Image image;
    public float scaleUpFactor = 1.5f;   // Ŀ���� ����
    public float duration = 0.2f;         // �ִϸ��̼� �ð�
    Vector3 originalScale;
    Vector3 targetScale;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform ��������
        originalScale = rectTransform.localScale;  // ���� ũ�� ����
        targetScale = originalScale * scaleUpFactor;  // ��ǥ ũ��
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public IEnumerator PopCoinImg()
    {
        Vector3 originalPos = rectTransform.position;  // ���� ��ġ ����

        float time = 0f;

        // ũ�Ⱑ Ŀ����
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            rectTransform.position = originalPos;  // ��ġ�� ����
            time += Time.deltaTime;
            yield return null;
        }

        // ũ�� �ִ�� Ŀ�� ��, ���� ũ��� ���ư���
        time = 0f;
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
            rectTransform.position = originalPos;  // ��ġ�� ����
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = originalScale;  // ��Ȯ�ϰ� ���� ũ���
        rectTransform.position = originalPos;  // ��ġ ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
