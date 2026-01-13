using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Combat))]
public abstract class BuildingBase : InteractableBase
{
    protected Combat combat;
    protected bool isDestroyed = false;

    [SerializeField] BuildingBase[] upgrades;
    int currentUpgradeIndex = 0;
    [SerializeField] float showTime = 0.5f;

    protected virtual void OnEnable()
    {
        if (!combat)
            combat = GetComponent<Combat>();

        combat.OnDie += Combat_OnDie;

        DayNightCycle.Instance.OnTimeChanged += DayNightCycle_OnTimeChanged; 
    }
    protected virtual void OnDisable()
    {
        combat.OnDie -= Combat_OnDie;

        DayNightCycle.Instance.OnTimeChanged -= DayNightCycle_OnTimeChanged;
    }
    void Init()
    {
        GetComponent<Collider>().enabled = true;
        combat.enabled = true;
        gameObject.SetActive(true);
    }
    protected virtual void Combat_OnDie()
    {
        isDestroyed = true;
        combat.enabled = false;
    }
    protected virtual void DayNightCycle_OnTimeChanged(bool isDay)
    {
        if (isDay)
        {
            isDestroyed = false;
            combat.enabled = true;
        }
    }
    public override void Interact()
    {
        base.Interact();

        InitUpgradeCanvas();
    }
    private void InitUpgradeCanvas()
    {
        string[] upgradeNames = new string[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgradeNames[i] = upgrades[i].name;
        }

        UnityAction[] actions = new UnityAction[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            int index = i;
            actions[i] = () => Upgrade(index);
        }

        UpgradeCanvas.Instance.Init(upgrades.Length, upgradeNames, actions);
    }
    protected virtual void Upgrade(int index)
    {
        print($"Upgrading to {upgrades[index].name}");

        if (index < 0 || index >= upgrades.Length)
            return;

        OnFocusLost();

        upgrades[index].gameObject.SetActive(true);
        upgrades[index].GetComponent<Renderer>().material = GetComponent<Renderer>().material;
        upgrades[index].Init();

        gameObject.SetActive(false);
    }
    public override void OnFocusGain()
    {
        base.OnFocusGain();
        ShowUpgradeOptions();
    }
    public override void OnFocusLost()
    {
        base.OnFocusLost();
        HideUpgradeOption();
    }
    void ShowUpgradeOptions()
    {
        if (upgrades.Length == 0)
            return;

        currentUpgradeIndex = (currentUpgradeIndex + 1) % upgrades.Length;
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].gameObject.SetActive(i == currentUpgradeIndex);
        }

        Invoke(nameof(ShowUpgradeOptions), showTime);
    }
    void HideUpgradeOption()
    {
        if (upgrades.Length == 0)
            return;

        CancelInvoke(nameof(ShowUpgradeOptions));
        upgrades[currentUpgradeIndex].gameObject.SetActive(false);
    }
}
