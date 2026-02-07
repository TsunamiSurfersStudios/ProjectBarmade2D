using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Order
{
    public string CustomerName { get; private set; }
    public string DrinkName { get; private set; }
    public GameObject UIElement { get; private set; }

    public Order(string customerName, string drinkName, GameObject uiElement)
    {
        CustomerName = customerName;
        DrinkName = drinkName;
        UIElement = uiElement;
    }
}

public class OrderController : MonoBehaviour
{
    [SerializeField] private GameObject orderFramePrefab;
    private Transform orderListContent;

    private List<Order> activeOrders = new List<Order>();
    private List<Order> completedOrders = new List<Order>();

    void Start()
    {
        orderListContent = transform.Find("OrdersListContainer/Viewport/Content");

        if (orderListContent == null)
        {
            Debug.LogError("Order list content not found in children of " + gameObject.name);
            return;
        }

        if (orderFramePrefab == null)
        {
            Transform existing = orderListContent.Find("Order");
            if (existing != null)
            {
                orderFramePrefab = existing.gameObject;
                orderFramePrefab.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        NPCOrdering.OnOrderCreated += AddOrder;
        NPCOrdering.OnOrderCompleted += CompleteOrder;
    }

    void OnDisable()
    {
        NPCOrdering.OnOrderCreated -= AddOrder;
        NPCOrdering.OnOrderCompleted -= CompleteOrder;
    }

    public void AddOrder(string customerName, string drinkName)
    {
        if (orderFramePrefab == null || orderListContent == null)
        {
            Debug.LogError("OrderController is not properly configured.");
            return;
        }

        GameObject newOrderUI = Instantiate(orderFramePrefab, orderListContent);
        newOrderUI.SetActive(true);

        TextMeshProUGUI customerNameText = newOrderUI.transform.Find("CustomerName")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI customerOrderText = newOrderUI.transform.Find("CustomerOrder")?.GetComponent<TextMeshProUGUI>();

        if (customerNameText != null)
            customerNameText.text = customerName;

        if (customerOrderText != null)
            customerOrderText.text = drinkName;

        Order order = new Order(customerName, drinkName, newOrderUI);
        activeOrders.Add(order);
    }

    public void CompleteOrder(string customerName)
    {
        Order order = activeOrders.Find(o => o.CustomerName == customerName);
        if (order == null)
        {
            Debug.LogWarning("No active order found for customer: " + customerName);
            return;
        }

        activeOrders.Remove(order);
        completedOrders.Add(order);

        if (order.UIElement != null)
            Destroy(order.UIElement);
    }

    public List<Order> GetActiveOrders() { return activeOrders; }
    public List<Order> GetCompletedOrders() { return completedOrders; }
    public List<Order> GetCompletedOrdersByCustomer(string customerName)
    {
        return completedOrders.FindAll(o => o.CustomerName == customerName);
    }
}
