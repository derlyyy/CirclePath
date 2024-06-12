using UnityEngine;

public class TrapCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCheck"))
        {
            PlayerManager.instance.Die();
            ScreenShake.instance.Shake();
        }
    }
}
