using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public float dropSpeed = 2f; 
    public Vector3 startOffset = new Vector3(0, 30, 0); 
    private Vector3 targetPosition; // 최종 위치

    void Start()
    {
        targetPosition = transform.localPosition; 
        transform.localPosition += startOffset;

        StartCoroutine(DropAnimation());
    }

    private IEnumerator DropAnimation()
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * dropSpeed);
            yield return null;
        }

        transform.localPosition = targetPosition;
    }
}
