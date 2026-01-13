using UnityEngine;

public class BuildingSpot : InteractableBase
{
    [SerializeField] BuildingBase building;
    [SerializeField] GameObject buildingGhost;
    public override void OnFocusGain()
    {
        base.OnFocusGain();
        buildingGhost.SetActive(true);
    }
    public override void OnFocusLost()
    {
        base.OnFocusLost();
        buildingGhost.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        PlaceBuilding();
    }
    public void PlaceBuilding()
    {
        buildingGhost.SetActive(false);
        building.gameObject.SetActive(true);
        enabled = false;

        GetComponent<Renderer>().enabled = false;
    }
}
