using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;

    [Header("Backgrounds")]
    [SerializeField] private GameObject startBackground;
    [SerializeField] private GameObject settingsBackground;

    private void Start()
    {
        ShowStartMenu();
    }

    private void Update()
    {
        // Als settings open is en je drukt ESC -> terug naar startmenu
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

        if (startBackground != null) startBackground.SetActive(true);
        if (settingsBackground != null) settingsBackground.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(true);

        if (startBackground != null) startBackground.SetActive(false);
        if (settingsBackground != null) settingsBackground.SetActive(true);
    }

    public void OnPlayPressed() { Debug.Log("Play pressed"); }
    public void OnNewGamePressed() { Debug.Log("New Game pressed"); }
    public void OnNewsPressed() { Debug.Log("News pressed"); }
    public void OnExitPressed() { Debug.Log("Exit pressed"); }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}