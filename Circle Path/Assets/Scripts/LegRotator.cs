using UnityEngine;

public class LegRotator : MonoBehaviour
{
    private PlayerVelocity rig;

    private void Start()
    {
        rig = GetComponentInParent<PlayerVelocity>();
    }

    private void Update()
    {
        if ((bool)rig)
        {
            if (rig.rb.velocity.x < 0f)
            {
                transform.localEulerAngles = Vector3.Lerp(base.transform.localEulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * 15f * Mathf.Clamp(Mathf.Abs(rig.rb.velocity.x), 0f, 1f));
            }
            else
            {
                transform.localEulerAngles = Vector3.Lerp(base.transform.localEulerAngles, new Vector3(0f, 180f, 0f), Time.deltaTime * 15f * Mathf.Clamp(Mathf.Abs(rig.rb.velocity.x), 0f, 1f));
            }
        }
    }
}