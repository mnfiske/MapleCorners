using UnityEngine;
using UnityEngine.UI;

public class MoneyAttribute : MonoBehaviour
{
    public float playerMoney;
    //Serialize will make it visible in the Unity inspector, but it's still private to this script
    [SerializeField] private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        // this will update the UI text box that displays energy and set it to the player's energy (converted from float to string)
        moneyText.text = playerMoney.ToString("0.00");
    }
}
