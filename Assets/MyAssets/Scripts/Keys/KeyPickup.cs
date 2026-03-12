using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private KeyID keyID;

    public KeyID Key => keyID;

    public void Pickup(PlayerKeyInventory inventory)
    {
        inventory.AddKey(keyID);
        Destroy(gameObject);
    }
}