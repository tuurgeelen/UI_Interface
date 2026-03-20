using System.Collections.Generic;
using UnityEngine;

public class RobotHoverGroup : MonoBehaviour
{
    [System.Serializable]
    public class HoverRobot
    {
        public Transform robot;
        public float hoverAmount = 0.2f;
        public float hoverSpeed = 1.5f;
        public float phaseOffset = 0f;

        [HideInInspector] public Vector3 startPosition;
    }

    [SerializeField] private List<HoverRobot> robots = new List<HoverRobot>();

    private void Start()
    {
        for (int i = 0; i < robots.Count; i++)
        {
            if (robots[i].robot != null)
            {
                robots[i].startPosition = robots[i].robot.localPosition;
            }
        }
    }

    private void Update()
    {
        float time = Time.time;

        for (int i = 0; i < robots.Count; i++)
        {
            HoverRobot data = robots[i];

            if (data.robot == null)
                continue;

            Vector3 pos = data.startPosition;
            pos.y += Mathf.Sin((time * data.hoverSpeed) + data.phaseOffset) * data.hoverAmount;
            data.robot.localPosition = pos;
        }
    }
}