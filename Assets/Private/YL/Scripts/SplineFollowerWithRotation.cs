using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowerWithRotation : MonoBehaviour
{
    public SplineContainer splineContainer;
    public GameObject splineObjectWithRotation; // AttackSpline 오브젝트
    public float duration = 1f; // 움직이는 데 걸리는 시간

    private float timer;
    private ISplineRotation splineRotation;

    void Start()
    {
        if (splineObjectWithRotation != null)
        {
            splineRotation = splineObjectWithRotation.GetComponent<ISplineRotation>();
        }
    }

    void Update()
    {
        if (splineContainer == null || splineRotation == null) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration); // 0 ~ 1

        // Spline의 위치 계산
        Vector3 position = splineContainer.EvaluatePosition(t);
        transform.position = position;

        // 회전 적용
        transform.rotation = splineRotation.GetRotation();
    }
}
