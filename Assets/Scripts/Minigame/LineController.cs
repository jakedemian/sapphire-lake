using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {
    public RectTransform fishHookPoint;
    public RectTransform rodHookPoint;
    public float lineThickness;

    private RectTransform rt;
    private Vector2 differenceVector => fishHookPoint.transform.position - rodHookPoint.transform.position;
    private float targetAngle => Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;

    private void Awake() {
        rt = GetComponent<RectTransform>();
    }

    private void Update() {
        transform.position = (Vector2)fishHookPoint.transform.position - (differenceVector / 2f);
        float distance = Vector2.Distance(fishHookPoint.transform.position, rodHookPoint.transform.position) / CanvasScalerInfo.i.currentScaleFactor;
        rt.sizeDelta = new Vector2(lineThickness, distance);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle + 90);
    }
}
