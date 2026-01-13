using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] RawImage indicatorImage;
	Combat playerCombat;

	private void Awake()
	{
		TryGetComponent(out playerCombat);
	}

	private void Update()
	{
		if(playerCombat.TargetCombat != null)
		{
			ShowIndicator(playerCombat.TargetCombat.FirePoint.position);
		}
		else
		{
			HideIndicator();
		}
	}

	public void ShowIndicator(Vector3 targetPos)
	{
		indicatorImage.enabled = true;
		indicatorImage.transform.position = Camera.main.WorldToScreenPoint(targetPos);
	}
	public void HideIndicator()
	{
		indicatorImage.enabled = false;
	}
}
