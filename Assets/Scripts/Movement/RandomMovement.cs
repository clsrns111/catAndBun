using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    float moveDistance = 0.2f;       // 이동 거리 (아주 작은 거리)
    public float moveDuration;      // 이동 후 대기 시간
    public float returnSpeed;
    private Vector3 originalPosition;       // 오브젝트의 원래 위치

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
            int randomDirectionIndex = Random.Range(0, 8);  // 8개 방향으로 랜덤 선택

            switch (randomDirectionIndex)
            {
                case 0: randomDirection = Vector3.up; break;            // 위로 이동
                case 1: randomDirection = Vector3.down; break;          // 아래로 이동
                case 2: randomDirection = Vector3.left; break;          // 왼쪽으로 이동
                case 3: randomDirection = Vector3.right; break;         // 오른쪽으로 이동
                case 4: randomDirection = new Vector3(1, 1, 0).normalized; break;  // 오른쪽 위
                case 5: randomDirection = new Vector3(-1, 1, 0).normalized; break; // 왼쪽 위
                case 6: randomDirection = new Vector3(1, -1, 0).normalized; break; // 오른쪽 아래
                case 7: randomDirection = new Vector3(-1, -1, 0).normalized; break; // 왼쪽 아래
            }

            // 랜덤 방향으로 moveDistance만큼 이동
            Vector3 targetPosition = originalPosition + randomDirection * moveDistance;
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveDuration * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;

            // 다시 대기
            yield return new WaitForSeconds(moveDuration);

            // 원래 위치로 부드럽게 돌아가기
            while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
                yield return null;
            }

            // 정확히 원래 위치에 설정
            transform.position = originalPosition;
        }
    }
}
