using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject loadingScreenPanel;

    [Header("Backgrounds")]
    [SerializeField] private GameObject startBackground;
    [SerializeField] private GameObject settingsBackground;

    [Header("Loading")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private string sceneToLoad = "GameScene";
    [SerializeField] private float minLoadingTime = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        ShowStartMenu();

        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(false);

        if (loadingBar != null)
            loadingBar.value = 0f;
    }

    private void Update()
    {
        if (settingsMenuPanel != null && settingsMenuPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowStartMenu();
            }
        }
    }

    public void ShowStartMenu()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (loadingScreenPanel != null) loadingScreenPanel.SetActive(false);

        if (startBackground != null) startBackground.SetActive(true);
        if (settingsBackground != null) settingsBackground.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(true);
        if (loadingScreenPanel != null) loadingScreenPanel.SetActive(false);

        if (startBackground != null) startBackground.SetActive(false);
        if (settingsBackground != null) settingsBackground.SetActive(true);
    }

    public void OnPlayPressed()
    {
        DOTween.KillAll();
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);

        if (startBackground != null) startBackground.SetActive(false);
        if (settingsBackground != null) settingsBackground.SetActive(false);

        if (loadingScreenPanel != null) loadingScreenPanel.SetActive(true);

        if (loadingBar != null)
            loadingBar.value = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float displayProgress = Mathf.Min(progress, timer / minLoadingTime);

            if (loadingBar != null)
                loadingBar.value = Mathf.Lerp(loadingBar.value, displayProgress, 5f * Time.deltaTime);

            if (progress >= 1f && timer >= minLoadingTime)
            {
                if (loadingBar != null)
                    loadingBar.value = 1f;

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}