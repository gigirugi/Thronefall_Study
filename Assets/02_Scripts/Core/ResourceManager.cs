using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public int GoldAmount { get; private set; }

    [Header("Settings")]
    [SerializeField] int startingGold = 5;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI goldText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Init(startingGold);
    }
    public void AddOrSpendGold(int amount)
    {
        GoldAmount += amount;

        goldText.SetText(GoldAmount.ToString());
    }

    public void Init(int startingGold)
    {
        GoldAmount = 0;
        AddOrSpendGold(startingGold);
    }
}

public enum HasEnoughMoneyType
{
    Yes,
    No
}
