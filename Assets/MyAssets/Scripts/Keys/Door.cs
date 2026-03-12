using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private KeyID requiredKey;
    [SerializeField] private bool isLocked = true;

    public bool TryOpen(PlayerKeyInventory inventory)
    {
        if (!isLocked)
        {
            OpenDoor();
            return true;
        }

        if (inventory.HasKey(requiredKey))
        {
            isLocked = false;
            OpenDoor();
            return true;
        }

        Debug.Log("Deur is op slot. Je hebt de juiste key niet.");
        return false;
    }

    private void OpenDoor()
    {
        Debug.Log($"{gameObject.name} gaat open!");
        
        // Hier later je animatie of open-logica
        // Bijvoorbeeld animator trigger:
        // GetComponent<Animator>().SetTrigger("Open");
    }
}