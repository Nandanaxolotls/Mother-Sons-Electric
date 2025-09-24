using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TwoPointLine : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public LineRenderer Line;
    // Start is called before the first frame update
    void Start()
    {
        Line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Line.positionCount = 2;
        Line.SetPosition(0, PointA.position);
        Line.SetPosition(1, PointB.position);
    }
}
