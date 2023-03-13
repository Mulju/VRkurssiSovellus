using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : MonoBehaviour
{
    [SerializeField]
    private Transform endPoint1, endPoint2;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        CreateString(null);
    }

    // ? antaa parametrin olla myös null
    public void CreateString(Vector3? midPoint)
    {
        // Jos midPointtia ei ole, taulukon koko on 2. Jos midpointissa on jotain muuta, koko on 3
        Vector3[] points = new Vector3[midPoint == null ? 2 : 3];
        points[0] = endPoint1.localPosition;
        if(midPoint != null)
        {
            // Konvertoidaan parametrina tullut keskipiste lokaaliin kordinaatistoon
            points[1] = transform.InverseTransformPoint(midPoint.Value);
        }
        // ^1 viittaa viimeiseen alkioon listassa. ^2 viittaisi toiseksi viimeiseen jne..
        points[^1] = endPoint2.localPosition;
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
