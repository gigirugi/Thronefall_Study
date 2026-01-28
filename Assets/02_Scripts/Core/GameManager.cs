using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int PlayerLevel { get; private set; } = 0;
    public void IncreasePlayerLevel() => PlayerLevel++;

    public Action OnGameOver;
    [SerializeField] Material grayscaleMaterial;
    [SerializeField] Button restartButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void OnEnable()
    {
        OnGameOver += GameManager_GameOver;
        restartButton.onClick.AddListener(RestartGame);
    }
    private void OnDisable()
    {
        OnGameOver -= GameManager_GameOver;
        restartButton.onClick.RemoveListener(RestartGame);
    }
    private void GameManager_GameOver()
    {
        StartCoroutine(CoFadeToGray());
    }
    IEnumerator CoFadeToGray()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            grayscaleMaterial.SetFloat("_Strength", Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        grayscaleMaterial.SetFloat("_Strength", 0);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnApplicationQuit()
    {
        grayscaleMaterial.SetFloat("_Strength", 0);
    }
}
