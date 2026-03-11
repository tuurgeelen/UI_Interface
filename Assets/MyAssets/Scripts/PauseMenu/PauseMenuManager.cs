using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                PauseGame();
            }
            else
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    ShowMainPausePanel();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        IsPaused = true;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (mainPausePanel != null)
            mainPausePanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        IsPaused = false;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (mainPausePanel != null)
            mainPausePanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowSettingsPanel()
    {
        if (mainPausePanel != null)
            mainPausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void ShowMainPausePanel()
    {
        if (mainPausePanel != null)
            mainPausePanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("UI");
    }
}