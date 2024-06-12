using UnityEngine;

public class LegRaycasters : MonoBehaviour
{
    [SerializeField] private LayerMask mask; // Маска слоев для raycast

    [SerializeField] private float force = 10f; // Сила применяемая к персонажу
    [SerializeField] private float drag = 5f; // Сила сопротивления при приземлении

    [SerializeField] private Transform[] legCastPositions; // Позиции ног для raycast

    [SerializeField] private AnimationCurve animationCurve; // Кривая для вычисления силы в зависимости от позиции

    private PlayerVelocity rig; // Скрипт скорости игрока
    private CharacterData data; // Данные персонажа

    public AnimationCurve wobbleCurve; // Кривая для колебаний при движении
    public AnimationCurve forceCurve; // Кривая для вычисления силы прыжка

    private IKLeg[] legs; // Ссылки на скрипты ног
    private float totalStepTime; // Общее время шага

    private void Awake()
    {
        legs = transform.root.GetComponentsInChildren<IKLeg>();
    }

    private void Start()
    {
        rig = GetComponentInParent<PlayerVelocity>();
        data = GetComponentInParent<CharacterData>();
    }

    private void FixedUpdate()
    {
        totalStepTime = 0f;
        foreach (IKLeg leg in legs)
        {
            if (!leg.footDown)
            {
                totalStepTime += leg.stepTime;
            }
        }
        CastRays();
    }

    private void CastRays()
    {
        foreach (Transform leg in legCastPositions)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(leg.position + Vector3.up * 0.5f, Vector2.down, 1f * transform.root.localScale.x, mask);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.transform.root != transform.root)
                {
                    HitGround(leg, hit);
                    break; // Прерываем цикл после обработки первого подходящего столкновения
                }
            }
        }
    }

    private void HitGround(Transform leg, RaycastHit2D hit)
    {
        if (data.sinceJump >= 0.2f && Vector3.Angle(Vector3.up, hit.normal) <= 70f)
        {
            data.TouchGround(hit.point, hit.normal, hit.rigidbody);
            Vector3 displacement = ((Vector3)hit.point - leg.position) / transform.root.localScale.x;
            if (data.input.direction.x != 0f)
            {
                displacement.y += wobbleCurve.Evaluate(totalStepTime) * transform.root.localScale.x;
                rig.rb.AddForce(Vector3.up * forceCurve.Evaluate(totalStepTime) * rig.rb.mass);
            }
            rig.rb.AddForce(animationCurve.Evaluate(Mathf.Abs(displacement.y)) * Vector3.up * rig.rb.mass * force);
            rig.rb.AddForce(animationCurve.Evaluate(Mathf.Abs(displacement.y)) * (0f - rig.rb.velocity.y) * Vector2.up * rig.rb.mass * drag);
        }
    }
}
