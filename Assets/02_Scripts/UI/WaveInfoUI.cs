using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Image timerImage;
    float totalTime;
    float elapsedTime;

    public void SetUI(Sprite icon, int quantity, float totalTime)
    {
        image.sprite = icon;
        this.quantity.SetText("x" + quantity.ToString());
        this.totalTime = totalTime;
    }
    private void Update()
    {
        if (DayNightCycle.Instance.IsDay)
            return;

        elapsedTime += Time.deltaTime;
        timerImage.fillAmount = Mathf.Clamp01(1 - (elapsedTime / totalTime));
    }
}
