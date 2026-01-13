using UnityEngine;
using UnityEngine.UI;

public class HPCanvas : MonoBehaviour
{
    [SerializeField] Slider slider;
    float showTime = 2f;
    float timer = 0f;

    Quaternion initialRotation;
    private void Start()
    {
        initialRotation = slider.transform.rotation;
        HideSlider();
    }
    private void Update()
    {
        if (slider.gameObject.activeSelf)
        {
            slider.transform.rotation = initialRotation;

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                HideSlider();
            }
        }
    }
    public void ShowSlider(float value)
    {
        slider.gameObject.SetActive(true);
        slider.value = value;
        timer = showTime;
    }
    public void HideSlider()
    {
        slider.gameObject.SetActive(false);
        timer = showTime;
    }
}
