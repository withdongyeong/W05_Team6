using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer; // Inspector에서 할당할 SplineContainer
    public float speed = 2f;                // 이동 속도
    public bool isMoving = false;           // Inspector에서 제어 가능하도록 public으로 설정
    private float t = 0f;                   // 진행률 (0 ~ 1)
    private Vector3 initialPosition;        // 초기 위치 저장

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Attack();
        // else 블록을 제거해서 Inspector에서 설정한 값이 강제로 덮어씌워지지 않도록 함
    }

    public void StartMoving()
    {
        if (splineContainer != null && splineContainer.Spline != null)
        {
            t = 0f;
            isMoving = true;
        }
        else
        {
            Debug.LogError("SplineContainer가 할당되지 않았거나 스플라인이 정의되지 않았습니다!");
        }
    }

    private Vector3 GetPositionOnSpline(float t)
    {
        Spline spline = splineContainer.Spline;
        float length = spline.GetLength();
        Vector3 localPosition = spline.EvaluatePosition(t);
        return splineContainer.transform.TransformPoint(localPosition);


    }


    void Attack()
    {
        if (isMoving)
        {
            t += Time.deltaTime * speed;

            if (t >= 1f)
            {
                t = 1f;
                isMoving = false; // 이동 완료 시 false로 설정
                transform.position = initialPosition; // 초기 위치로 복귀
            }
            else
            {
                Vector3 newPosition = GetPositionOnSpline(t);
                transform.position = newPosition;
            }
        }
    }
}