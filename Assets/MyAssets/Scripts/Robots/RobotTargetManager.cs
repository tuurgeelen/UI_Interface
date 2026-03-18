using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTargetManager : MonoBehaviour
{
    [System.Serializable]
    public class RobotData
    {
        public Transform robot;
        [HideInInspector] public Vector3 originalScale;
        [HideInInspector] public bool isDestroyed;
    }

    public static RobotTargetManager Instance { get; private set; }

    [Header("Objective")]
    [SerializeField] private string requiredObjectiveID = "robot_targets";

    [Header("Robots")]
    [SerializeField] private List<RobotData> robots = new List<RobotData>();

    [Header("Disappear Settings")]
    [SerializeField] private float disappearDuration = 0.25f;
    [SerializeField] private Vector3 shrinkTarget = Vector3.zero;

    private bool activated = false;
    private int robotsLeft;

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
        robotsLeft = robots.Count;

        for (int i = 0; i < robots.Count; i++)
        {
            if (robots[i].robot != null)
            {
                robots[i].originalScale = robots[i].robot.localScale;
                robots[i].isDestroyed = false;
                robots[i].robot.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!activated && ObjectiveManager.Instance != null)
        {
            if (ObjectiveManager.Instance.GetCurrentObjectiveID() == requiredObjectiveID)
            {
                ActivateRobots();
            }
        }
    }

    private void ActivateRobots()
    {
        activated = true;
        robotsLeft = robots.Count;

        for (int i = 0; i < robots.Count; i++)
        {
            if (robots[i].robot != null)
            {
                robots[i].isDestroyed = false;
                robots[i].robot.localScale = robots[i].originalScale;
                robots[i].robot.gameObject.SetActive(true);

                Collider[] cols = robots[i].robot.GetComponentsInChildren<Collider>(true);
                for (int c = 0; c < cols.Length; c++)
                {
                    cols[c].enabled = true;
                }
            }
        }

        UpdateObjectiveText();
    }

    public bool TryHitRobot(Transform hitTransform)
    {
        if (!activated || hitTransform == null)
            return false;

        for (int i = 0; i < robots.Count; i++)
        {
            if (robots[i].robot == null || robots[i].isDestroyed)
                continue;

            if (hitTransform == robots[i].robot || hitTransform.IsChildOf(robots[i].robot))
            {
                StartCoroutine(DestroyRobotRoutine(i));
                return true;
            }
        }

        return false;
    }

    private IEnumerator DestroyRobotRoutine(int index)
    {
        robots[index].isDestroyed = true;
        Transform robot = robots[index].robot;

        if (robot == null)
            yield break;

        Collider[] cols = robot.GetComponentsInChildren<Collider>(true);
        for (int c = 0; c < cols.Length; c++)
        {
            cols[c].enabled = false;
        }

        Vector3 startScale = robot.localScale;
        float time = 0f;

        while (time < disappearDuration)
        {
            time += Time.deltaTime;
            float t = time / disappearDuration;

            robot.localScale = Vector3.Lerp(startScale, shrinkTarget, t);
            yield return null;
        }

        robot.localScale = shrinkTarget;
        robot.gameObject.SetActive(false);

        robotsLeft--;
        robotsLeft = Mathf.Max(robotsLeft, 0);

        UpdateObjectiveText();

        if (robotsLeft <= 0 && ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteObjective(requiredObjectiveID);
        }
    }

    private void UpdateObjectiveText()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.SetCurrentCounterText(robotsLeft + " Robots Left");
        }
    }
}