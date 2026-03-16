using UnityEngine;
using System.Collections.Generic;

public class LightFlickerGroup : MonoBehaviour
{
    public List<Light> lights = new List<Light>();

    public float minIntensity = 0.5f;
    public float maxIntensity = 5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    void Flicker()
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.intensity = Random.Range(minIntensity, maxIntensity);
            }
        }
    }
}