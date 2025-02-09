using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    // create a lookup table for out factorials
    // so up to a value of 18
    private static float[] Factorial = new float[]
    {
        1.0f,
        1.0f,
        2.0f,
        6.0f,
        24.0f,
        120.0f,
        720.0f,
        5040.0f,
        40320.0f,
        362880.0f,
        3628800.0f,
        39916800.0f,
        479001600.0f,
        6227020800.0f,
        87178291200.0f,
        1307674368000.0f,
        20922789888000.0f,
        355687428096000.0f,
        6402373705728000.0f,
    };

    private static float Binomial(int n, int i)
    {
        float ni;
        float a1 = Factorial[n];
        float a2 = Factorial[i];
        float a3 = Factorial[n-i];
        ni = a1 / (a2 * a3);
        return ni;
    }

    // bernstein basis points
    private static float BernStein(int n, int i, float t)
    {
        float t_i = Mathf.Pow(t, i);
        float t_n_minus_i = Mathf.Pow((1-t), (n-i));
        float basis = Binomial(n, i) * t_i * t_n_minus_i;
        return basis;
    }

    // implement bezier point func
    // return the interpolated point
    public static Vector3 Point3(float t, List<Vector3> controlPoints)
    {
        // this function to return a bezier point
        // based on the bezier curve equation. 0<t<1
        // both inclusive
        // just return a dummy point

        int N = controlPoints.Count - 1; // degree of curve
        if (N > 18)
        {
            Debug.Log("Max is 18");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        if (t <= 0) return controlPoints[0];
        if (t >= 1) return controlPoints[controlPoints.Count -1];

        Vector3 p = new Vector3();
        for (int i = 0; i< controlPoints.Count; ++i)
        {
            Vector3 bn = BernStein(N, i, t) * controlPoints[i];
            p += bn;
        }
        return p;
    }

    // calculate all the point for entire curve
    // t = 0 > 1 with an increment of 0.01  
    public static List<Vector3> PointList3(List<Vector3> controlPoints, float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 18)
        {
            Debug.Log("Max is 18");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        List<Vector3> points = new List<Vector3>();
        for (float t = 0.0f; t <= 1.0f +  interval - 0.0001f; t += interval)
        {
            Vector3 p = new Vector3();
            for (int i=0; i < controlPoints.Count; ++i)
            {
                Vector3 bn = BernStein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }
        return points;
    }

    // the same for vector2 (for 2 dimenstional points)
    public static Vector3 Point2(float t, List<Vector2> controlPoints)
    {
        // this function to return a bezier point
        // based on the bezier curve equation. 0<t<1
        // both inclusive
        // just return a dummy point

        int N = controlPoints.Count - 1; // degree of curve
        if (N > 18)
        {
            Debug.Log("Max is 18");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        if (t <= 0) return controlPoints[0];
        if (t >= 1) return controlPoints[controlPoints.Count - 1];

        Vector2 p = new Vector2();
        for (int i = 0; i < controlPoints.Count; ++i)
        {
            Vector2 bn = BernStein(N, i, t) * controlPoints[i];
            p += bn;
        }
        return p;
    }

    // calculate all the point for entire curve
    // t = 0 > 1 with an increment of 0.01  
    public static List<Vector2> PointList2(List<Vector2> controlPoints, float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 18)
        {
            Debug.Log("Max is 18");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = BernStein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }
        return points;
    }
}