using UnityEngine;

public static class BezierCurve
{
    // Вычисляет точку на кубической кривой Безье
    public static Vector3 CubicBezier(Vector3 start, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 end, float t)
    {
        // Формула кубической кривой Безье
        return (1f - t) * QuadraticBezier(start, controlPoint1, controlPoint2, t) + t * QuadraticBezier(controlPoint1, controlPoint2, end, t);
    }

    // Вычисляет точку на квадратичной кривой Безье
    public static Vector3 QuadraticBezier(Vector3 start, Vector3 controlPoint, Vector3 end, float t)
    {
        // Формула квадратичной кривой Безье
        return (1f - t) * LinearBezier(start, controlPoint, t) + t * LinearBezier(controlPoint, end, t);
    }

    // Вычисляет точку на линейной кривой Безье
    public static Vector3 LinearBezier(Vector3 start, Vector3 end, float t)
    {
        // Формула линейной кривой Безье
        return (1f - t) * start + t * end;
    }
}