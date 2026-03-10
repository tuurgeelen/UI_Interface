using UnityEngine;
using UnityEngine.UI;

public class DisableMenuRaycasts : MonoBehaviour
{
    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>(true);

        foreach (Image img in images)
        {
            if (img.GetComponent<Button>() == null)
            {
                img.raycastTarget = false;
            }
        }
    }
}