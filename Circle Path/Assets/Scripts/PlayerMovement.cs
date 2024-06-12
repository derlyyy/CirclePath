using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float force = 10f; // Сила применяемая к игроку
    [SerializeField] private float airControl = 0.3f; // Управление в воздухе
    [SerializeField] private float extraDrag = 0.5f; // Дополнительное сопротивление при движении
    [SerializeField] private float extraAngularDrag = 0.5f; // Дополнительное угловое сопротивление

    private float multiplier = 1f;
    
    private CharacterData data;
    private CharacterStatModifier stats;

    private void Start()
    {
        data = GetComponent<CharacterData>();
        stats = GetComponent<CharacterStatModifier>();
    }

    private void FixedUpdate()
    {
        if (!data.isPlaying)
        {
            return;
        }

        if (data.canMove)
        {
            Move(data.input.direction);
        }

        ApplyPhysicsModifiers();
    }

    private void Move(Vector2 direction)
    {
        UpdateMultiplier();

        if (!data.isStunned)
        {
            direction.y = Mathf.Clamp(direction.y, -1f, 0f) * 2f;
            float movementForce = (1f - stats.slow) * stats.movementSpeed * force * data.playerVel.rb.mass * 0.01f * multiplier;
            data.playerVel.rb.AddForce(direction * movementForce, ForceMode2D.Force);
        }
    }

    private void ApplyPhysicsModifiers()
    {
        data.playerVel.rb.velocity *= (1f - 0.01f * 0.1f * extraDrag * multiplier);
        data.playerVel.rb.angularVelocity *= (1f - 0.01f * 0.1f * extraAngularDrag * multiplier);
    }

    private void UpdateMultiplier()
    {
        multiplier = data.isGrounded ? 1f : airControl;
    }
}