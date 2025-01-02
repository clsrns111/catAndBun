using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    AudioSource audioSource;

    public float bouncePower = 5f;        // 초기 튕기는 힘
    public float gravity = 9.8f;          // 중력 가속도
    public float rotationSpeed = 360f;
    public float groundLevel = -20f;
    float maxHorizontalOffset = 3f;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

    public IEnumerator PopOutEffect()
    {
        float verticalVelocity = bouncePower;
        float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);

        while (transform.position.y > groundLevel)
        {
            // 위쪽으로 상승하는 동안 점점 느려지다가 중력으로 인해 하강
            verticalVelocity -= gravity * Time.deltaTime;
            transform.position += new Vector3(horizontalOffset * Time.deltaTime, verticalVelocity * Time.deltaTime, 0);

            // 회전 효과
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // groundLevel에 근접했을 때
            if (Mathf.Approximately(transform.position.y, groundLevel))
            {
                Debug.Log("D");
                Destroy(this.gameObject);
                yield break;  // Coroutine 종료
            }

            yield return null;
        }
    }
}
