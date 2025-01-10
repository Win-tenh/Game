using System.Collections.Generic;
using UnityEngine;

public class TmpBezierCurve : MonoBehaviour
{
    public static readonly List<Vector2> templateControlPoints = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(35, 15),
        new Vector2(47, 13),
        new Vector2(45, 5),
        new Vector2(48, 0),
        new Vector2(25, -5),
        new Vector2(15, -18),
        new Vector2(36, -20),
        new Vector2(64, -20),
        new Vector2(85, -18),
        new Vector2(75, -5),
        new Vector2(52, 0),
        new Vector2(55, 5),
        new Vector2(53, 13),
        new Vector2(65, 15),
        new Vector2(100, 0),

    };

    // keep referemce to point prefab
    // this represent each control point
    public GameObject PointPrefab;

    // use LineRenderer to show the straight line
    // connecting these control points as well as
    // the bezier curve based pn these control points.
    LineRenderer[] mLineRenderers = null;

    // keepj list of the points instantiated from the prefab to show control points
    List<GameObject> mPointGOs = new List<GameObject>();

    // store the properties of line
    public float LineWidth;
    public float LineWidthBezier;
    public Color LineColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    public Color BezierCurveColor = new Color(0.5f, 0.6f, 0.8f, 0.8f);

    private LineRenderer CreateLine()
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = LineColor;
        lr.endColor = LineColor;
        lr.startWidth = LineWidth;
        lr.endWidth = LineWidth;
        return lr;
    }

    private void Start()
    {
        // create actual lines
        mLineRenderers = new LineRenderer[2];
        mLineRenderers[0] = CreateLine();
        mLineRenderers[1] = CreateLine();

        // set name of these lines to distinguish
        mLineRenderers[0].gameObject.name = "LineRenderer_obj_0";
        mLineRenderers[1].gameObject.name = "LineRenderer_obj_1";

        // create instances of control points
        for (int i = 0; i < templateControlPoints.Count; i++)
        {
            GameObject obj = Instantiate(
                PointPrefab,
                templateControlPoints[i],
                Quaternion.identity
            );
            obj.name = "ControlPoint_" + i.ToString();
            mPointGOs.Add(obj);
        }
    }

    private void Update()
    {
        // draw the line every frame
        LineRenderer lineRenderer = mLineRenderers[0];
        LineRenderer curveRenderer = mLineRenderers[1];

        List<Vector2> pts = new List<Vector2>();
        for (int i = 0; i < mPointGOs.Count; i++)
            pts.Add(mPointGOs[i].transform.position);

        // set lineRenderer for showing the straight lines between
        // the control points
        lineRenderer.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; i++)
        {
            lineRenderer.SetPosition(i, pts[i]);
        }

        // the straight lines connecting the control points
        // proceed to draw the curve based on the bezier points
        List<Vector2> curve = BezierCurve.PointList2(pts, 0.01f);
        curveRenderer.startColor = BezierCurveColor;
        curveRenderer.endColor = BezierCurveColor;
        curveRenderer.positionCount = curve.Count;
        curveRenderer.startWidth = LineWidthBezier;
        curveRenderer.endWidth = LineWidthBezier;

        for (int i = 0; i < curve.Count; i++)
        {
            curveRenderer.SetPosition(i, curve[i]);
        }
    }
}
