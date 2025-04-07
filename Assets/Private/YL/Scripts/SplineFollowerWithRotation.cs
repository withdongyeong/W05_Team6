using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class SplineFollowerWithRotation : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Coroutine moveRoutine;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void PlaySpline(string splineName)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveAlongSpline(splineName));
    }

    private IEnumerator MoveAlongSpline(string splineName)
    {
        GameObject splineObj = FindObjectOfType<SplineManager>().GetSpline(splineName);
        if (splineObj == null)
        {
            Debug.LogWarning($"[Follower] {splineName} 오브젝트 없음");
            yield break;
        }

        var splineContainer = splineObj.GetComponent<SplineContainer>();
        var splineRotation = splineObj.GetComponent<ISplineRotation>();

        if (splineContainer == null || splineRotation == null)
        {
            Debug.LogWarning($"[Follower] {splineName}에 필요한 컴포넌트 없음");
            yield break;
        }

        float moveDuration = 1f;
        float timer = 0f;

        while (timer < moveDuration)
        {
            float t = timer / moveDuration;

            transform.position = splineContainer.EvaluatePosition(t);
            transform.rotation = splineRotation.GetRotation();

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = splineContainer.EvaluatePosition(1f);
        transform.rotation = splineRotation.GetRotation();

        yield return new WaitForSeconds(1f);

        Vector3 endPosition = transform.position;
        Quaternion endRotation = transform.rotation;

        // 복귀
        float returnDuration = 2f;
        float returnTimer = 0f;

        while (returnTimer < returnDuration)
        {
            float t = returnTimer / returnDuration;

            transform.position = Vector3.Lerp(endPosition, startPosition, t);
            transform.rotation = Quaternion.Slerp(endRotation, startRotation, t);

            returnTimer += Time.deltaTime;
            yield return null;
        }

        // 최종 정렬
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
