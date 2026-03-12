using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInventory : MonoBehaviour
{
    private HashSet<KeyID> collectedKeys = new HashSet<KeyID>();

    public void AddKey(KeyID keyID)
    {
        if (!collectedKeys.Contains(keyID))
        {
            collectedKeys.Add(keyID);
            Debug.Log("Key opgepakt: " + keyID);
        }
    }

    public bool HasKey(KeyID keyID)
    {
        return collectedKeys.Contains(keyID);
    }
}