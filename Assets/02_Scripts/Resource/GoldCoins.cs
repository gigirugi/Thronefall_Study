using UnityEngine;

public class GoldCoins : MonoBehaviour
{
	[SerializeField] GameObject[] onObjects;
	[SerializeField] GameObject[] offObjects;

	int currentCount = 0;

	public void Init(int price)
	{
		gameObject.SetActive(true);

		for (int i = 0; i < onObjects.Length; i++)
		{
			offObjects[i].SetActive(i < price);

			onObjects[i].SetActive(false);
		}
	}

	public int AddAndGetCoin()
	{
		if (ResourceManager.Instance.GoldAmount <= 0)
			return currentCount;

		ResourceManager.Instance.AddOrSpendGold(-1);

		currentCount++;
		onObjects[currentCount - 1].SetActive(true);
		return currentCount;
	}
	public void ResetCount(HasEnoughMoneyType hasEnoughMoney)
	{
		if (currentCount > 0 && hasEnoughMoney == HasEnoughMoneyType.No)
		{
			ResourceManager.Instance.AddOrSpendGold(currentCount);
		}

		for (int i = 0; i < currentCount; i++)
		{
			onObjects[i].SetActive(false);
		}

		currentCount = 0;
	}
}