using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManger : MonoBehaviour
{
   [SerializeField] Customer customer;
   [SerializeField] RichCustomer richCustomer;
   [SerializeField] List<Customer> CustomerList;
   [SerializeField] GameObject saleLine;

    public float spawnOffset = 5f; //
    public float spawnInterval = 1f;  //
    float accSpeed = 0.5f;
    float maxAccSpeed = 0.5f;

    GameManager gm;
    List<WaitSpot> waitSpots = new List<WaitSpot>();
    private void Start()
    {
        CustomerList = new List<Customer>();
        gm = GameManager.instance;
        waitSpots = gm.waitSpots;

        for(int i = 0; i < waitSpots.Count; i++)
        {
            SpawnCustomer(i);
            SetOrderLayer(i);
        }

        StartCoroutine(UpdateOrderCustomer());
    }

    public void SetOrderLayer(int i)
    {
        SpriteRenderer spriteRenderer = CustomerList[i].GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -i;
    }

    public void AllCustomerMove()
    {
        for(int i = 0; i< CustomerList.Count;i++) 
        {
            StartCoroutine(CustomerList[i].MoveToLine(waitSpots[i].transform.position, Mathf.Clamp(accSpeed / gm.comboCount,0.05f,0.5f)));

            SetOrderLayer(i);
        }

        StartCoroutine(UpdateOrderCustomer());
    }

    public void SpawnCustomer(int i)
    {
        if (CustomerList.Count >= 20)
            return;

        Customer newCustomer;

        if (Random.value <= 0.07f)
        {
             newCustomer = PrefabFactory.Instance.Create(richCustomer, waitSpots[i].transform.position);
        } else
        {
             newCustomer = PrefabFactory.Instance.Create(customer, waitSpots[i].transform.position);
        }

        CustomerList.Add(newCustomer);
        newCustomer.GenerateRandomRequire();
        newCustomer.MoveCoroutineHandler();
    }

    public void RemoveCustomer(Customer customer)
    {
        CustomerList.Remove(customer);
        AllCustomerMove();
    }

    public IEnumerator UpdateOrderCustomer()
    {
        yield return new WaitForSeconds(0.1f);

        Customer targetCustomer = CustomerList[0];

        gm.playerController.SetTargetCustomer(targetCustomer);
    }
}
