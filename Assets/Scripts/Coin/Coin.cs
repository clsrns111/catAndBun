using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public float bouncePower = 5f;        // �ʱ� ƨ��� ��
    public float gravity = 9.8f;          // �߷� ���ӵ�
    public float rotationSpeed = 360f;
    public float groundLevel = -10f;
    float maxHorizontalOffset = 3f;

    void Start()
    {
    }

    public IEnumerator PopOutEffect()
    {
        float verticalVelocity = bouncePower;
        float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);

        while (transform.position.y > groundLevel)
        {
            // �������� ����ϴ� ���� ���� �������ٰ� �߷����� ���� �ϰ�
            verticalVelocity -= gravity * Time.deltaTime;
            transform.position += new Vector3(horizontalOffset * Time.deltaTime , verticalVelocity * Time.deltaTime, 0);

            if (Mathf.Approximately(transform.position.y, groundLevel))
            {
                Destroy(this.gameObject);
                yield break;  // Coroutine ����
            }

            yield return null;
        }

        Destroy(gameObject);

    }
}
