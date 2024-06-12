using UnityEngine;

public class PlayerVelocity : MonoBehaviour
{
    private CharacterData data;
    
    public Rigidbody2D rb;
    
    public Vector2 position
    {
        get => transform.position;
        set => rb.MovePosition(value);
    }

    private void Awake()
    {
        data = GetComponent<CharacterData>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!data.isPlaying)
        {
            rb.velocity = Vector2.zero; // Останавливаем движение, если персонаж не активен
        }
    }
}