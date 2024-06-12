using System.Collections.Generic;
using UnityEngine;

public class LegRenderer : MonoBehaviour
{
    [Header("Joints")]
    public Transform startJoint;
    public Transform midJoint;
    public Transform endJoint;

    [Header("Segments")]
    public int segmentCount = 10;

    [Header("Object Pool")]
    public ObjectPool pool; // Добавляем ссылку на пул объектов

    public List<GameObject> segments = new List<GameObject>();

    private void Awake()
    {
        pool.Initialize(segmentCount); // Инициализация пула объектов
        InitializeSegments();
    }

    private void InitializeSegments()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = pool.GetObject(); // Получение объекта из пула
            segments.Add(segment);
            if (i == segmentCount - 1)
                segment.transform.localScale = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        /*foreach (GameObject segment in segments)
        {
            pool.ReturnObject(segment); // Возвращаем объекты в пул
        }*/
    }

    private void Update()
    {
        UpdateSegmentPositions();
        UpdateSegmentRotations();
    }

    private void UpdateSegmentPositions()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            float t = i / (float)(segments.Count - 1);
            segments[i].transform.position = BezierCurve.QuadraticBezier(startJoint.position, midJoint.position, endJoint.position, t);
        }
    }

    private void UpdateSegmentRotations()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Vector3 direction = CalculateSegmentDirection(i);
            segments[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    private Vector3 CalculateSegmentDirection(int index)
    {
        if (index == 0)
        {
            return segments[1].transform.position - segments[0].transform.position;
        }
        if (index == segments.Count - 1)
        {
            return segments[index].transform.position - segments[index - 1].transform.position;
        }
        Vector3 prevSegmentDir = segments[index].transform.position - segments[index - 1].transform.position;
        Vector3 nextSegmentDir = segments[index + 1].transform.position - segments[index].transform.position;
        return (prevSegmentDir + nextSegmentDir) * 0.5f;
    }
}
