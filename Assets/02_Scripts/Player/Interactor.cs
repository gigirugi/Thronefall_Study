using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] InputActionReference interactInputRef;
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] LayerMask interactableLayer;
    Collider[] results = new Collider[10];
    [Header("Debug")]
    [SerializeField] int playerLevel = 0;

    void Update()
    {
        int length = Physics.OverlapSphereNonAlloc(transform.position, interactRange, results, interactableLayer);
        InteractableBase nearest = null;
        float bestSqr = float.MaxValue;
        for (int i = 0; i < length; i++)
        {
            if (results[i].TryGetComponent(out InteractableBase interactableBase))
            {
                float sqr = (results[i].ClosestPoint(transform.position) - transform.position).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    if (interactableBase.CanInteract(GameManager.Instance.PlayerLevel))
                        nearest = interactableBase;
                }
            }
        }

        if (nearest != InteractableBase.CurrentInteractable)
        {
            if (InteractableBase.CurrentInteractable)
                InteractableBase.CurrentInteractable.OnFocusLost();

            InteractableBase.CurrentInteractable = nearest;

            if (InteractableBase.CurrentInteractable)
                InteractableBase.CurrentInteractable.OnFocusGain();
        }
        if (interactInputRef.action.IsPressed())
        {
            if (InteractableBase.CurrentInteractable)
                InteractableBase.CurrentInteractable.BeginInteract();
            else
                DayNightCycle.Instance.TryChangeTime();
        }
        else
        {
            if (InteractableBase.CurrentInteractable)
                InteractableBase.CurrentInteractable.EndInteract();
            else
                DayNightCycle.Instance.ResetTimer();
        }   
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
