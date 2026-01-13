using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeCanvas : MonoBehaviour
{
	public static UpgradeCanvas Instance { get; private set; }

	[SerializeField] Button[] optionButtons;
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		gameObject.SetActive(false);
	}
	public void Init(int optionsCount, string[] names, UnityAction[] actions)
	{
		if (optionsCount != actions.Length)
		{
			Debug.LogError("optionsCount와 actions 길이가 맞지 않습니다.");
			return;
		}

		for (int i = 0; i < optionButtons.Length; i++)
		{
			int ii = i; // 클로저 문제 해결
			if (i < optionsCount)
			{
				optionButtons[i].gameObject.SetActive(true);
				SetUpButton(ii, names[ii], actions[ii]);
			}
			else
				optionButtons[i].gameObject.SetActive(false);
		}
		gameObject.SetActive(true);
		Time.timeScale = 0f;
	}
	void SetUpButton(int buttonIndex, string name, UnityAction action)
	{
		if (buttonIndex >= 0 && buttonIndex < optionButtons.Length)
		{
			optionButtons[buttonIndex].onClick.RemoveAllListeners();
			optionButtons[buttonIndex].onClick.AddListener(action);
			optionButtons[buttonIndex].onClick.AddListener(Close);
			optionButtons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().SetText(name);
		}
	}
	void Close()
	{
		gameObject.SetActive(false);
		Time.timeScale = 1f;
	}
}
