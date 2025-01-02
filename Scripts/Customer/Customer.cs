using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Customer : MonoBehaviour
{
    [SerializeField] int requireRedBin;
    [SerializeField] int requireCream;
    public GameManager gm;
    public SpeechBubble speechBubble;
    public SpeechBubble shortSpeechBubblePrefab;
    public SpeechBubble longSpeechBubblePrefab;

    SpriteRenderer spriteRenderer;

    bool orderEnd = false;
    bool orderFail = false;
    public bool isMoving = false;

    float outSpeed = 7f;

    public Vector3 targetPosition;  // 목표 위치
    private float t = 0f;            // 시간 비율 (0~1)

    [SerializeField] public Sprite sprites;
    [SerializeField] public  RandomMovement randomMovement;
    [SerializeField] public Coin coin;

    private Coroutine moveCoroutine; // 코루틴을 저장할 변수

    private void Start()
    {
        gm = GameManager.instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        randomMovement = GetComponent<RandomMovement>();

        ApplyRandomSprite();
    }

    public virtual void ApplyRandomSprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cats/Normal");

        int randomIndex = UnityEngine.Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    IEnumerator MoveToOut()
    {
        isMoving = true;
        MoveCoroutineHandler();

        while (true)
        {
            transform.position += Time.deltaTime * outSpeed * Vector3.left;
            
            if(orderFail)
                transform.Rotate(0, 0, 360f * Time.deltaTime);

            if (IsObjectOutsideCamera())
                Destroy(gameObject);

            yield return null;
        }
    }

    public IEnumerator MoveToLine(Vector2 target, float time)
    {
        isMoving = true;
        MoveCoroutineHandler();

        yield return new WaitForSeconds(0.1f);

        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(startPosition, target, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;

        MoveCoroutineHandler();
    }

    public void SetRequire(int randomRedBin , int randomCream)
    {
        if (randomRedBin == 0)
        {
            speechBubble = PrefabFactory.Instance.Create(shortSpeechBubblePrefab);
            speechBubble.SetRandomBunImage(BunType.cream);
        }
        else if (randomCream == 0)
        {
            speechBubble = PrefabFactory.Instance.Create(shortSpeechBubblePrefab);
            speechBubble.SetRandomBunImage(BunType.redBin);
        }
        else
        {
            speechBubble = PrefabFactory.Instance.Create(longSpeechBubblePrefab);
            speechBubble.SetRandomBunImage(BunType.both);
        }

        requireRedBin = randomRedBin;
        requireCream = randomCream;

        speechBubble.transform.SetParent(transform, false);
        speechBubble.UpdateText(requireRedBin, requireCream);
    }


    public virtual void GenerateRandomRequire()
    {
        int randomRedBin = UnityEngine.Random.Range(0, 5);
        int randomCream = UnityEngine.Random.Range(0, 5);

        if(randomRedBin + randomCream == 0)
        {
            if (UnityEngine.Random.value < 0.5f)
                randomRedBin = UnityEngine.Random.Range(1, 5);
            else
                randomCream = UnityEngine.Random.Range(1, 5);
        }

        SetRequire(randomRedBin,randomCream);   
    }


    public void UpdateRequire(BunType burn)
    {
        if (orderEnd)
            return;

        switch (burn)
        {
            case BunType.cream:
                requireCream--;
                break;
            case BunType.redBin:
                requireRedBin--;
                break;
        }

        speechBubble.UpdateText(requireRedBin, requireCream);

        bool isOrderCompleted = (requireCream <= 0 && requireRedBin <= 0);
        bool isOrderFailed = (requireCream < 0 || requireRedBin < 0);

        if (isOrderFailed || isOrderCompleted)
        {

            SetActiveSpeechBubble(false);
            orderEnd = true;
            orderFail = isOrderFailed;
            StartCoroutine(MoveToOut());
            gm.customerManger.RemoveCustomer(this);

            if (orderFail)
            {
                gm.timerbar.AddRemainingTime(-3f);
                Handheld.Vibrate();
                gm.ResetCombo();
            }
            else
            {
                gm.timerbar.AddRemainingTime(0.5f);
                gm.AddCombo();
                HandleCoinGenerate();
            }

            gm.customerManger.SpawnCustomer(19);
        }
    }
 

    protected virtual void HandleCoinGenerate()
    {
        Coin newCoin = PrefabFactory.Instance.Create(coin, transform.position);
        StartCoroutine(newCoin.PopOutEffect());
     
        gm.Addcoin(1);
    }

    public void SetTargetpos(Vector2 pos, Customer customer)
    {
        targetPosition = pos;
        StartCoroutine(MoveToLine(pos, 1));
    }

    public void SetActiveSpeechBubble(bool onOff)
    {
        speechBubble.gameObject.SetActive(onOff);
    }
    bool IsObjectOutsideCamera()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.x < -1f || viewportPosition.x > 1f ||
            viewportPosition.y < -1f || viewportPosition.y > 1f)
        {
            return true;
        }

        return false;
    }
    public void MoveCoroutineHandler()
    {
        randomMovement.SaveOriginPos();

        if (!isMoving && moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(randomMovement.RandomMoveAndReturn());
        }
        else if (isMoving && moveCoroutine != null)
        {
            // isMoving이 false일 때 코루틴 멈추기
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
