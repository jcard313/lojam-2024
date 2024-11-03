using UnityEngine;

public class swordCollider : MonoBehaviour
{
    public Collider2D collied;

    void EnableCollider()
    {
        collied.enabled = true;
    }

    void DisableCollider()
    {
        collied.enabled = false;
    }
}
