using UnityEngine;
using System.Collections;

public class BunMovement : MonoBehaviour
{
    public float duration = 0.4f;       
    public float controlOffset = 2f;

    public IEnumerator MoveAlongBezierCurve(Vector2 startPoint, Customer targetCustomer, BunType bun)
    {
        float t = 0f;  
        Vector2 endPoint = targetCustomer.transform.position;

        Vector2 controlPoint = (startPoint + endPoint) / 2;
        controlPoint += Vector2.up * controlOffset;  

        while (t < 1f)
        {
            t += Time.deltaTime / duration;  

            Vector2 newPos = Mathf.Pow(1 - t, 2) * startPoint
                             + 2 * (1 - t) * t * controlPoint
                             + Mathf.Pow(t, 2) * endPoint;

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(endPoint.x, endPoint.y, transform.position.z);

        if(BunType.cream == bun)
        {
            targetCustomer.UpdateRequire(BunType.cream);
        }
        else
        {
            targetCustomer.UpdateRequire(BunType.redBin);
        }

        Destroy(gameObject);
    }
}
