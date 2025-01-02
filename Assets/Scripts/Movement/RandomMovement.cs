using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    float moveDistance = 0.2f;       // �̵� �Ÿ� (���� ���� �Ÿ�)
    public float moveDuration;      // �̵� �� ��� �ð�
    public float returnSpeed;
    private Vector3 originalPosition;       // ������Ʈ�� ���� ��ġ

    Customer customer; 

    void Start()
    {
        customer = GetComponent<Customer>();
        returnSpeed = Random.Range(0.1f, 3f);
        moveDuration = Random.Range(0.1f, 3f);
    }

    public void SaveOriginPos(Vector2 targetPos)
    {
        originalPosition = targetPos;
    }

    public IEnumerator RandomMoveAndReturn()
    {
        while (true)
        {

            Vector3 randomDirection = Vector3.zero;
            int randomDirectionIndex = Random.Range(0, 8);  // 8�� �������� ���� ����

            switch (randomDirectionIndex)
            {
                case 0: randomDirection = Vector3.up; break;            // ���� �̵�
                case 1: randomDirection = Vector3.down; break;          // �Ʒ��� �̵�
                case 2: randomDirection = Vector3.left; break;          // �������� �̵�
                case 3: randomDirection = Vector3.right; break;         // ���������� �̵�
                case 4: randomDirection = new Vector3(1, 1, 0).normalized; break;  // ������ ��
                case 5: randomDirection = new Vector3(-1, 1, 0).normalized; break; // ���� ��
                case 6: randomDirection = new Vector3(1, -1, 0).normalized; break; // ������ �Ʒ�
                case 7: randomDirection = new Vector3(-1, -1, 0).normalized; break; // ���� �Ʒ�
            }

            // ���� �������� moveDistance��ŭ �̵�
            Vector3 targetPosition = originalPosition + randomDirection * moveDistance;
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveDuration * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;

            // �ٽ� ���
            yield return new WaitForSeconds(moveDuration);

            // ���� ��ġ�� �ε巴�� ���ư���
            while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
                yield return null;
            }

            // ��Ȯ�� ���� ��ġ�� ����
            transform.position = originalPosition;
        }
    }
}
