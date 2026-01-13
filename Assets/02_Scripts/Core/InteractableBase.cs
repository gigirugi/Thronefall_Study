using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    public static InteractableBase CurrentInteractable;

    [SerializeField] int interactableLevel;
    [SerializeField] float interactionTime = 2f;
    float timer;

    [Header("Price Settings")]
    [SerializeField] int price = 3;
    [SerializeField] protected GoldCoins goldCoins;

    public bool CanInteract(int playerLevel)
    {
        return playerLevel >= interactableLevel && DayNightCycle.Instance.IsDay;
    }
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void OnFocusGain()
    {
        goldCoins.Init(price);
    }
    public virtual void OnFocusLost()
    {
        goldCoins.ResetCount(HasEnoughMoneyType.No);
        goldCoins.gameObject.SetActive(false);
    }

    public virtual void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;

        if (CurrentInteractable == this)
            CurrentInteractable = null;
    }

    public void BeginInteract()
    {
        timer += Time.deltaTime;

        float timePerPrice = interactionTime / price;
        if (timer >= timePerPrice)
        {
            int coinIndex = goldCoins.AddAndGetCoin();
            timer = 0f;

            if (coinIndex >= price)
            {
                goldCoins.ResetCount(HasEnoughMoneyType.Yes);
                goldCoins.gameObject.SetActive(false);
                Interact();
            }
        }
    }

    public void EndInteract()
    {
        timer = 0f;
        goldCoins.ResetCount(HasEnoughMoneyType.No);
    }
}
