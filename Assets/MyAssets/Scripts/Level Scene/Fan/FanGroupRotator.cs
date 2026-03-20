using System.Collections.Generic;
using UnityEngine;

public class FanGroupRotator : MonoBehaviour
{
    [System.Serializable]
    public class FanData
    {
        public Transform fan;
        public Vector3 axis = Vector3.forward;
        public float speed = 150f;
        public bool rotate = true;

        [Header("Audio")]
        public AudioSource audioSource;
    }

    [SerializeField] private List<FanData> fans = new List<FanData>();

    [Header("Audio Settings")]
    [SerializeField] private Transform listener;
    [SerializeField] private LayerMask occlusionLayers;
    [SerializeField] private float occludedVolume = 0.2f;
    [SerializeField] private float normalVolume = 0.6f;

    private void Start()
    {
        if (listener == null && Camera.main != null)
            listener = Camera.main.transform;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < fans.Count; i++)
        {
            FanData fan = fans[i];

            if (fan.fan == null)
                continue;

            // ROTATION
            if (fan.rotate)
                fan.fan.Rotate(fan.axis, fan.speed * dt, Space.Self);

            // AUDIO
            if (fan.audioSource != null && listener != null)
            {
                Vector3 dir = listener.position - fan.fan.position;
                float distance = dir.magnitude;

                bool occluded = Physics.Raycast(
                    fan.fan.position,
                    dir.normalized,
                    distance,
                    occlusionLayers
                );

                fan.audioSource.volume = occluded ? occludedVolume : normalVolume;
            }
        }
    }
}