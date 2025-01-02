using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Customer targetCustomer;
    [SerializeField] BunMovement redBinObejct;
    [SerializeField] BunMovement creamObject;
    public Camera mainCamera; // 카메라 연
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }
    public void SetTargetCustomer(Customer customer)
    {
        targetCustomer = customer;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("OptionButton"))
                    return;
            }

            if (gm.isPaused || !gm.isGameStart || targetCustomer == null)
                return;

            if(gm.isFirstTouch)
                gm.isFirstTouch = false;    

            CheckMouseDir();
            SoundManager.Instance.PlayEffectSound(3,"pop");
        }
    }

    public void CheckMouseDir()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

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
