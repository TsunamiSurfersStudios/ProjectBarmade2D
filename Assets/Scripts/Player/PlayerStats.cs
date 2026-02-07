using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float money = 0;
    [SerializeField] TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void RemoveMoney(float amount)
    {
        money -= amount;
        UpdateMoneyText();
    }

    public float GetMoney()
    {
        return money;
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "$" + money.ToString("0.00");
    }
}
