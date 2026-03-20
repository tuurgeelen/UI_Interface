using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectiveData
    {
        [Header("Objective Info")]
        public string objectiveID;

        [Header("Texts")]
        public string labelText = "OBJECTIVE";
        public string titleText = "New Objective";
        public bool useCounter = false;
        public string counterText = "";

        [Header("Colors")]
        public Color labelColor = Color.cyan;
        public Color titleColor = Color.white;
        public Color counterColor = Color.white;
    }

    [Header("Main UI")]
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private TextMeshProUGUI objectiveLabelText;
    [SerializeField] private TextMeshProUGUI objectiveTitleText;
    [SerializeField] private TextMeshProUGUI objectiveCounterText;

    [Header("Objectives")]
    [SerializeField] private List<ObjectiveData> objectives = new List<ObjectiveData>();

    [Header("Complete Popup")]
    [SerializeField] private GameObject completePopup;
    [SerializeField] private float popupDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completeSound;

    private int currentObjectiveIndex = 0;

    public static ObjectiveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (completePopup != null)
            completePopup.SetActive(false);

        ShowCurrentObjective();
    }

    private void Update()
    {
        UpdatePanelVisibility();
    }

    private void UpdatePanelVisibility()
    {
        if (objectivePanel == null)
            return;

        bool hasObjectivesLeft = currentObjectiveIndex < objectives.Count;
        bool shouldShow = hasObjectivesLeft && !PauseMenuManager.IsPaused;

        objectivePanel.SetActive(shouldShow);
    }

    private void ShowCurrentObjective()
    {
        if (currentObjectiveIndex >= objectives.Count)
        {
            ClearAllTexts();
            UpdatePanelVisibility();
            return;
        }

        ObjectiveData current = objectives[currentObjectiveIndex];

        if (objectiveLabelText != null)
        {
            objectiveLabelText.text = current.labelText;
            objectiveLabelText.color = current.labelColor;
        }

        if (objectiveTitleText != null)
        {
            objectiveTitleText.text = current.titleText;
            objectiveTitleText.color = current.titleColor;
        }

        if (objectiveCounterText != null)
        {
            if (current.useCounter)
            {
                objectiveCounterText.gameObject.SetActive(true);
                objectiveCounterText.text = current.counterText;
                objectiveCounterText.color = current.counterColor;
            }
            else
            {
                objectiveCounterText.text = "";
                objectiveCounterText.gameObject.SetActive(false);
            }
        }

        UpdatePanelVisibility();
    }

    private void ClearAllTexts()
    {
        if (objectiveLabelText != null)
            objectiveLabelText.text = "";

        if (objectiveTitleText != null)
            objectiveTitleText.text = "";

        if (objectiveCounterText != null)
        {
            objectiveCounterText.text = "";
            objectiveCounterText.gameObject.SetActive(false);
        }
    }

    public void CompleteObjective(string objectiveID)
    {
        if (currentObjectiveIndex >= objectives.Count)
            return;

        if (objectives[currentObjectiveIndex].objectiveID != objectiveID)
            return;

        currentObjectiveIndex++;

        ShowCompletePopup();
        ShowCurrentObjective();
    }

    public void SetCurrentCounterText(string newCounterText)
    {
        if (currentObjectiveIndex >= objectives.Count)
            return;

        objectives[currentObjectiveIndex].counterText = newCounterText;

        if (objectiveCounterText != null)
        {
            objectiveCounterText.gameObject.SetActive(true);
            objectiveCounterText.text = newCounterText;
            objectiveCounterText.color = objectives[currentObjectiveIndex].counterColor;
        }
    }

    public void SetCurrentCounterColor(Color newColor)
    {
        if (currentObjectiveIndex >= objectives.Count)
            return;

        objectives[currentObjectiveIndex].counterColor = newColor;

        if (objectiveCounterText != null)
        {
            objectiveCounterText.color = newColor;
        }
    }

    public void HideCounter()
    {
        if (objectiveCounterText != null)
        {
            objectiveCounterText.text = "";
            objectiveCounterText.gameObject.SetActive(false);
        }
    }

    public void ShowCounter()
    {
        if (currentObjectiveIndex >= objectives.Count || objectiveCounterText == null)
            return;

        objectiveCounterText.gameObject.SetActive(true);
        objectiveCounterText.text = objectives[currentObjectiveIndex].counterText;
        objectiveCounterText.color = objectives[currentObjectiveIndex].counterColor;
    }

    private void ShowCompletePopup()
    {
        if (completePopup != null)
        {
            StopAllCoroutines();
            completePopup.SetActive(true);
            StartCoroutine(HidePopupRoutine());
        }

        if (audioSource != null && completeSound != null)
        {
            audioSource.PlayOneShot(completeSound);
        }
    }

    private IEnumerator HidePopupRoutine()
    {
        yield return new WaitForSeconds(popupDuration);

        if (completePopup != null)
            completePopup.SetActive(false);
    }

    public string GetCurrentObjectiveID()
    {
        if (currentObjectiveIndex < objectives.Count)
            return objectives[currentObjectiveIndex].objectiveID;

        return string.Empty;
    }

    public int GetCurrentObjectiveIndex()
    {
        return currentObjectiveIndex;
    }

    public bool HasObjectivesRemaining()
    {
        return currentObjectiveIndex < objectives.Count;
    }

    public void RefreshUI()
{
    ShowCurrentObjective();
    UpdatePanelVisibility();
}
}