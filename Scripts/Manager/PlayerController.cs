using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Customer targetCustomer;
    [SerializeField] BunMovement redBinObejct;
    [SerializeField] BunMovement creamObject;
    [SerializeField] BunMovement JamObject;
    public AudioClip spawnSound;         // ����� ���� Ŭ��
    [SerializeField] private AudioSource audioSource;     // AudioSource ������Ʈ

    public void SetTargetCustomer(Customer customer)
    {
        targetCustomer = customer;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (targetCustomer == null)
                return;

            CheckMouseDir();
            audioSource.PlayOneShot(spawnSound);
        }
    }

    public void CheckMouseDir()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetMouseButtonDown(0))  // ���� ���콺 Ŭ��
        {
            if (mousePosition.x < screenCenter.x)
            {
                BunMovement redbin = PrefabFactory.Instance.Create(redBinObejct, transform.position);
                StartCoroutine(redbin.MoveAlongBezierCurve(worldMousePosition, targetCustomer, BunType.redBin));
            }

            if (mousePosition.x > screenCenter.x)
            {
                BunMovement creamBun = PrefabFactory.Instance.Create(creamObject, transform.position);
                StartCoroutine(creamBun.MoveAlongBezierCurve(worldMousePosition, targetCustomer, BunType.cream));
            }
        }
    }
}
