using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    [Header("Shake Settings")]
    public float duration = 0.5f; // Продолжительность тряски
    public float amplitude = 1.0f; // Амплитуда тряски
    public AnimationCurve shakeCurve; // Кривая анимации для тряски

    private Transform camTransform;
    private Vector3 originalPos;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        camTransform = Camera.main.transform;
        originalPos = camTransform.localPosition;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            camTransform.localPosition = originalPos;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float strength = shakeCurve.Evaluate(elapsed / duration) * amplitude;
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            camTransform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        camTransform.localPosition = originalPos;
    }

    public void SetShakeSettings(float newDuration, float newAmplitude, AnimationCurve newCurve)
    {
        duration = newDuration;
        amplitude = newAmplitude;
        shakeCurve = newCurve;
    }
}