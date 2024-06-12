using System;
using System.Collections;
using Sonity;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapType type;
    public DirectionType directionType;
    private Vector3 direction;

    public GameObject[] trapObjects; // Массив объектов ловушек
    public float amount;
    public float speed;

    public SoundEvent activateSound;

    public bool isActive;
    public bool activateSequentially; // Булевая переменная для последовательной активации
    public bool autoActivate;
    
    public float destroyTime;
    public bool destroyAfterTime;
    public bool isInvisible; // Проверка на невидимость объекта
    
    public Vector3 teleportLocation; // Локация для телепортации

    private void Start()
    {
        SetDirection();
        
        if (autoActivate)
        {
            Activate();
        }
    }

    private void SetDirection()
    {
        switch (directionType)
        {
            case DirectionType.Left:
                direction = Vector3.left;
                break;
            case DirectionType.Right:
                direction = Vector3.right;
                break;
            case DirectionType.Up:
                direction = Vector3.up;
                break;
            case DirectionType.Down:
                direction = Vector3.down;
                break;
        }
    }

    public void Activate()
    {
        if (isActive)
        {
            return;
        }
        
        isActive = true;

        if (activateSequentially)
        {
            StartCoroutine(ActivateTrapSequentially());
        }
        else
        {
            StartCoroutine(ActivateAllTraps());
        }
    }

    private IEnumerator ActivateTrapSequentially()
    {
        foreach (var trapObject in trapObjects)
        {
            yield return StartCoroutine(ActivateSingleTrap(trapObject));
        }
    }

    private IEnumerator ActivateAllTraps()
    {
        foreach (var trapObject in trapObjects)
        {
            StartCoroutine(ActivateSingleTrap(trapObject));
        }

        yield return null;
    }

    private IEnumerator ActivateSingleTrap(GameObject trapObject)
    {
        if (activateSound != null)
        {
            SoundManager.Instance.Play(activateSound, transform);
        }
        Vector3 startPos = trapObject.transform.position;
        Vector3 endPos = startPos + direction * amount;
        float step = speed * Time.deltaTime;

        while (Vector3.Distance(trapObject.transform.position, endPos) > 0.01f)
        {
            trapObject.transform.position = Vector3.MoveTowards(trapObject.transform.position, endPos, step);
            yield return null;
        }

        if (destroyAfterTime)
        {
            Destroy(trapObject, destroyTime); // Уничтожение объекта после достижения конечной позиции через заданное время, если флаг установлен
        }
    }

    public void Teleport()
    {
        foreach (var trapObject in trapObjects)
        {
            trapObject.transform.position = teleportLocation;
        }
    }
    
    public void MoveChaotically(float range)
    {
        foreach (var trapObject in trapObjects)
        {
            Vector3 randomDirection = new Vector3(
                UnityEngine.Random.Range(-range, range),
                UnityEngine.Random.Range(-range, range),
                UnityEngine.Random.Range(-range, range)
            );
            trapObject.transform.position += randomDirection;
        }
    }
    
    public bool IsInvisible(GameObject trapObject)
    {
        Renderer renderer = trapObject.GetComponent<Renderer>();
        return renderer != null && !renderer.isVisible;
    }
    
    public void ActivateInvisibleTraps()
    {
        foreach (var trapObject in trapObjects)
        {
            if (IsInvisible(trapObject))
            {
                // Действия при активации невидимой ловушки
                ActivateSingleTrap(trapObject);
            }
        }
    }
}

public enum TrapType
{
    Spike, // шипы
    Hole, // яма
    Ceiling, // падающие ...
    End, // край карты
    Teleport, // телепорт в другое место
}

public enum DirectionType
{
    Left,
    Right,
    Up,
    Down
}
