using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CustomerManger : MonoBehaviour
{
   [SerializeField] Customer[] customerPrefab;
   [SerializeField] RichCustomer richCustomer;
   [SerializeField] public List<Customer> CustomerList;
   [SerializeField] GameObject saleLine;

    public float spawnOffset = 5f; //
    public float spawnInterval = 1f;  //
    float accSpeed = 0.3f;
    float MinAccSpeed = 0.3f;
    float MaxAccSpeed = 0.05f;

    GameManager gm;
    List<WaitSpot> waitSpots = new List<WaitSpot>();
    private void Start()
    {
        CustomerList = new List<Customer>();
        gm = GameManager.instance;
        waitSpots = gm.waitSpots;

        for(int i = 0; i < waitSpots.Count; i++)
        {
            Customer newCustomer = SpawnCustomer(i);
            AllCustomerShowSpeechBubble(newCustomer, false);

            SetOrderLayer(i);
        }
        UpdateOrderCustomer();
    }

    public void SetOrderLayer(int i)
    {
        SpriteRenderer spriteRenderer = CustomerList[i].GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = -i;
    }

    public IEnumerator AllCustomerMove()
    {
        yield return new WaitForSeconds(0f);

        for (int i = 0; i < CustomerList.Count; i++)
        {
            StartCoroutine(CustomerList[i].MoveToLine(waitSpots[i].transform.position, Mathf.Clamp(accSpeed / gm.comboCount, MaxAccSpeed, MinAccSpeed)));
            SetOrderLayer(i);

            CustomerList[i].randomMovement.SaveOriginPos(waitSpots[i].transform.position);
        }
    }

    public Customer SpawnCustomer(int i)
    {
        if (CustomerList.Count >= 20)
            return null;

        Customer newCustomer;

        if (Random.value <= 0.07f)
        {
             newCustomer = PrefabFactory.Instance.Create(richCustomer, waitSpots[i].transform.position);
        } else
        {
            Customer customer = customerPrefab[Random.Range(0,customerPrefab.Length - 1)];

             newCustomer = PrefabFactory.Instance.Create(customer, waitSpots[i].transform.position);
        }

        CustomerList.Add(newCustomer);
        newCustomer.randomMovement.SaveOriginPos(waitSpots[i].transform.position);
        newCustomer.MoveCoroutineHandler();
        newCustomer.GenerateRandomRequire();

        return newCustomer;
    }

    public void ToggleCustomerSpeechBubble(Customer customer, bool on)
    {
        customer.GetComponentInChildren<SpeechBubble>(true).gameObject.SetActive(on);   
    }

    public void AllCustomerShowSpeechBubble(Customer customer, bool active)
    {
        ToggleCustomerSpeechBubble(customer, active);
    }

    public void RemoveCustomer(Customer customer)
    {
        CustomerList.Remove(customer);
        StartCoroutine(AllCustomerMove());
        UpdateOrderCustomer();
    }

    public void UpdateOrderCustomer()
    {
        Customer targetCustomer = CustomerList[0];
        gm.playerController.SetTargetCustomer(targetCustomer);
    }
}
