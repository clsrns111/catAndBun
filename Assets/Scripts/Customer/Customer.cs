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

    public bool orderEnd = false;
    bool orderFail = false;
    public bool isMoving = false;

    float outSpeed = 10f;

    public Vector3 targetPosition;  // 목표 위치

    [SerializeField] public Sprite sprites;
    [SerializeField] public  RandomMovement randomMovement;
    [SerializeField] public Coin coin;
    Animator animator;
    public int bestCoinRecord = 0;

    private Coroutine moveCoroutine; // 코루틴을 저장할 변수
    int rewardCoin;
    public float moveTime = 0f;

    private void Start()
    {
        gm = GameManager.instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        randomMovement = GetComponent<RandomMovement>();
        animator = GetComponent<Animator>();

        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, UnityEngine.Random.Range(0f, 1f));
    }

    public virtual void ApplyRandomSprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cats/Normal/Anim");

        int randomIndex = UnityEngine.Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    IEnumerator MoveToOut()
    {
        isMoving = true;
        MoveCoroutineHandler();

        StartCoroutine(HardDestroy());

        while (!IsObjectOutsideCamera())
        {
            transform.position += Time.deltaTime * outSpeed * Vector3.left;
            
            if(orderFail)
                transform.Rotate(0, 0, 360f * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }

    public IEnumerator MoveToLine(Vector2 target, float time)
    {
        moveTime = time;

        if (gameObject == null)
        {
            yield break; // 처음부터 gameObject가 없다면 바로 종료
        }

        isMoving = true;
        MoveCoroutineHandler();

        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < time )
        {
            if (gameObject == null || !gameObject.activeInHierarchy)
            {
                Destroy(gameObject);    
                yield break;
            }

            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(startPosition, target, t);

            elapsedTime += Time.deltaTime;
            moveTime -= Time.deltaTime;

            yield return null;
        }

        if (gameObject != null) // 마지막 확인
        {
            transform.position = target;
            isMoving = false;

            MoveCoroutineHandler();
        }
    }

    public IEnumerator HardDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
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
        rewardCoin = requireCream + requireRedBin;

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
            StartCoroutine(MoveToOut());
            gm.customerManger.RemoveCustomer(this);
            orderEnd = true;
            orderFail = isOrderFailed;

            if (orderFail)
            {
                gm.HandleRequestFail();
            }
            else
            {
                gm.HandleRequestSuccess();

                for (int i = 0; i<rewardCoin; i++)
                {
                    HandleCoinGenerate();
                }

                if(rewardCoin < 4)
                {
                    SoundManager.Instance.PlayEffectSound(0, "coin_s");
                } else if(rewardCoin < 6)
                {
                    SoundManager.Instance.PlayEffectSound(1, "coin_m");
                }
                else
                {
                    SoundManager.Instance.PlayEffectSound(2, "coin_L");
                }
            }
        }
    }
 

    protected virtual void HandleCoinGenerate()
    {
        Coin newCoin = PrefabFactory.Instance.Create(coin, transform.position);
        StartCoroutine(newCoin.PopOutEffect());

        gm.Addcoin(1);
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
