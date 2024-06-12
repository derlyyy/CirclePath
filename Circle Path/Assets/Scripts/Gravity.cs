using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float gravityForce = 9.81f;  // Стандартная сила гравитации
    [SerializeField] private float exponent = 1f;  // Значение, влияющее на изменение гравитации со временем

    private PlayerVelocity rig;
    private CharacterData data;

    private void Start()
    {
        data = GetComponent<CharacterData>();
        rig = GetComponent<PlayerVelocity>();
    }

    private void FixedUpdate()
    {
        float timeSinceLastContact = Mathf.Min(data.sinceGrounded, data.sinceWallGrab);
        float gravityMultiplier = timeSinceLastContact > 0 ? Mathf.Pow(timeSinceLastContact, exponent) : timeSinceLastContact;

        rig.rb.AddForce(Vector3.down * gravityMultiplier * gravityForce * rig.rb.mass, ForceMode2D.Force);
    }
}