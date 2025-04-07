using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class CameraMotionController : MonoBehaviour
{
    private Camera cam;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialFov;

    private SplineInputController splineController; 


    private void Awake()
    {
        cam = GetComponent<Camera>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialFov = cam.fieldOfView;

        splineController = FindAnyObjectByType<SplineInputController>();
    }

    public void StartBounceJap() => StartCoroutine(BounceJap());
    public void StartBounceKick() => StartCoroutine(BounceKick());
    public void StartFall() => StartCoroutine(Fall());
    public void StartZoomInOut() => StartCoroutine(ZoomInOut());
    public void StartZoomIn() => StartCoroutine(ZoomIn());
    public void StartTilt() => StartCoroutine(Tilt());

    private IEnumerator BounceJap()
    {
        float duration = 0.5f;
        float halfDuration = 0.2f;

        Vector3 upPos = initialPosition + Vector3.up * 0.3f;
        Vector3 rightPos = initialPosition + Vector3.right * 0.1f;
        Vector3 targetPos = upPos + Vector3.right * 1f;

        // Y축 + Z회전 애니메이션 시작 (튕김)
        StartCoroutine(AnimatePositionAndZRotation(initialPosition, upPos, 0.3f, halfDuration));
        yield return new WaitForSeconds(halfDuration);
        StartCoroutine(AnimatePositionAndZRotation(upPos, initialPosition, -0.2f, halfDuration));
        StartCoroutine(ChangeFOV(cam.fieldOfView, 54f, duration));
        yield return new WaitForSeconds(halfDuration);
        StartCoroutine(AnimateZRotation(-0.2f, 0f, duration));


        Quaternion rotatedY = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 0.2f, transform.localEulerAngles.z);
        Quaternion originalRot = transform.localRotation;
        // X축으로 동시에 이동 + JapSpline 즉시 실행
        StartCoroutine(RotateTo(originalRot, rotatedY, duration));

        splineController.SendMessage("TriggerSpline", "JapSpline");

        // 3. 동시에 복귀: 위치 + FOV
        yield return new WaitForSeconds(1.2f);

        Coroutine rotBack = StartCoroutine(RotateTo(rotatedY, originalRot, duration));
        Coroutine fovBack = StartCoroutine(ChangeFOV(54f, initialFov, duration));

        // 둘 다 끝날 때까지 기다림
        yield return rotBack;
        yield return fovBack;
    }


    private IEnumerator BounceKick()
    {
        float duration = 0.5f;
        float halfDuration = 0.2f;

        Vector3 upPos = initialPosition + Vector3.up * 0.2f;
        Vector3 rightPos = initialPosition + Vector3.right * 0.1f;
        Vector3 targetPos = upPos + Vector3.right * 1f;

        // Y축 + Z회전 애니메이션 시작 (튕김)
        StartCoroutine(AnimatePositionAndZRotation(initialPosition, upPos, 0.2f, halfDuration));
        yield return new WaitForSeconds(halfDuration);
        StartCoroutine(AnimatePositionAndZRotation(upPos, initialPosition, -0.2f, halfDuration));
        StartCoroutine(ChangeFOV(cam.fieldOfView, 54f, duration));
        yield return new WaitForSeconds(halfDuration);
        StartCoroutine(AnimateZRotation(-0.2f, 0f, duration));


        Quaternion rotatedY = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 0.2f, transform.localEulerAngles.z);
        Quaternion originalRot = transform.localRotation;
        // X축으로 동시에 이동 + JapSpline 즉시 실행
        

        splineController.SendMessage("TriggerSpline", "KickSpline");
        yield return new WaitForSeconds(halfDuration);
        StartCoroutine(RotateTo(originalRot, rotatedY, duration));
        // 3. 동시에 복귀: 위치 + FOV
        yield return new WaitForSeconds(1f);

        Coroutine rotBack = StartCoroutine(RotateTo(rotatedY, originalRot, duration));
        Coroutine fovBack = StartCoroutine(ChangeFOV(54f, initialFov, duration));

        // 둘 다 끝날 때까지 기다림
        yield return rotBack;
        yield return fovBack;

    }

    private IEnumerator Fall()
    {
        float duration = 0.5f;
        Vector3 downPos = initialPosition + Vector3.down * 0.5f;
        Vector3 upPos = initialPosition + Vector3.up * 0.3f;

        // 1단계: 아래로 떨어지기
        yield return AnimatePositionAndFov(initialPosition, downPos, initialFov, 58f, duration);

        // 2단계: 동시에 UppercutSpline 실행 + 위로 상승
        Coroutine moveUpRoutine = StartCoroutine(MovePosition(downPos, upPos, 0.3f));
        splineController.SendMessage("TriggerSpline", "UppercutSpline");

        yield return moveUpRoutine;
        yield return new WaitForSeconds(1.2f);
        // 3단계: 원래 위치로 복귀
        yield return AnimatePositionAndFov(upPos, initialPosition, 58f, initialFov, duration);
    }

    private IEnumerator ZoomInOut()
    {
        float duration = 0.3f;

        // 1단계: Zoom In
        yield return ChangeFOV(cam.fieldOfView, 60f, duration);
        yield return new WaitForSeconds(0.1f);
        // 2단계: Zoom Out과 동시에 AttackSpline 실행
        Coroutine zoomOutRoutine = StartCoroutine(ChangeFOV(60f, 50f, duration));
        splineController.SendMessage("TriggerSpline", "AttackSpline");

        yield return zoomOutRoutine;

        // 대기 후 복귀
        yield return new WaitForSeconds(1.2f);
        yield return ChangeFOV(50f, initialFov, duration);
    }

    private IEnumerator ZoomIn()
    {
        float duration = 0.3f;

        // Fov: 55 → 60 → (2초 후) → 55
        yield return ChangeFOV(cam.fieldOfView, 60f, duration);
        yield return new WaitForSeconds(1f);
        yield return ChangeFOV(60f, initialFov, duration);
    }

    private IEnumerator Tilt()
    {
        Quaternion tilted = Quaternion.Euler(2.5f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        float duration = 0.3f;

        // 기울이기 시작
        Coroutine tiltRoutine = StartCoroutine(RotateTo(transform.localRotation, tilted, duration));

        // 동시에 spline 실행
        splineController.SendMessage("TriggerSpline", "GardLeftSpline");
        splineController.SendMessage("TriggerSpline", "GardRightSpline");

        // Spline이 2초간 재생된다고 가정 (혹은 WaitUntil 사용 가능)
        yield return new WaitForSeconds(1.3f);

        // 기울이기 완료 대기
        yield return tiltRoutine;

        // 복귀
        yield return RotateTo(tilted, initialRotation, duration);
    }

    // 🟢 보조 메서드들

    private IEnumerator MovePosition(Vector3 from, Vector3 to, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = to;
    }

    private IEnumerator ChangeFOV(float from, float to, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            cam.fieldOfView = Mathf.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        cam.fieldOfView = to;
    }

    private IEnumerator RotateTo(Quaternion from, Quaternion to, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = to;
    }

    private IEnumerator AnimatePositionAndFov(Vector3 fromPos, Vector3 toPos, float fromFov, float toFov, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(fromPos, toPos, time / duration);
            cam.fieldOfView = Mathf.Lerp(fromFov, toFov, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = toPos;
        cam.fieldOfView = toFov;
    }

    private IEnumerator AnimatePositionAndZRotation(Vector3 fromPos, Vector3 toPos, float zRotDeg, float duration)
    {
        float time = 0;
        Quaternion fromRot = transform.localRotation;
        Quaternion toRot = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotDeg);

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(fromPos, toPos, time / duration);
            transform.localRotation = Quaternion.Lerp(fromRot, toRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = toPos;
        transform.localRotation = toRot;
    }

    private IEnumerator AnimateZRotation(float fromZ, float toZ, float duration)
    {
        float time = 0;
        Quaternion fromRot = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, fromZ);
        Quaternion toRot = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, toZ);

        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(fromRot, toRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = toRot;
    }
}
