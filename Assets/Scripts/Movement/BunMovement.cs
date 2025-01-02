using UnityEngine;
using System.Collections;

public class BunMovement : MonoBehaviour
{
    private float duration = 0.5f;       // �̵� �ð�
    [SerializeField] private float controlOffset = 2f;   // ��� �߰� ���� ����
    [SerializeField] private Vector2 endPoint;
    private float targetTime = 1f;
    bool isChangTargetCustomer = false;

    private void Awake()
    {
        endPoint = GameManager.instance.waitSpots[0].transform.position;   
    }

    public IEnumerator MoveAlongBezierCurve(Vector2 startPoint, Customer targetCustomer, BunType bun)
    {
        float t = 0f; // ���� ���� (0���� 1���� ����)
        Vector2 controlPoint = (startPoint + endPoint) / 2 + Vector2.up * controlOffset;

        while (t < targetTime)
        {
            if(targetCustomer.orderEnd)
            {
                if(!isChangTargetCustomer)
                {
                    targetCustomer = GameManager.instance.playerController.targetCustomer;
                    isChangTargetCustomer = true;

                    duration += targetCustomer.moveTime;
                    targetTime += targetCustomer.moveTime;
                }
            }

            t += Time.deltaTime / duration; // ���� ����

            Vector2 newPos = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            yield return null; // ���� �����ӱ��� ���
        }

        transform.position = new Vector3(endPoint.x, endPoint.y, transform.position.z);
        targetCustomer.UpdateRequire(bun);

        Destroy(gameObject);
    }


    private Vector2 CalculateBezierPoint(float t, Vector2 start, Vector2 control, Vector2 end)
    {
        float oneMinusT = 1f - t;
        return Mathf.Pow(oneMinusT, 2) * start
               + 2f * oneMinusT * t * control
               + Mathf.Pow(t, 2) * end;
    }
}
