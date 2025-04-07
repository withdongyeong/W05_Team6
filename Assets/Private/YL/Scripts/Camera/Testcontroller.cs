using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMotionController : MonoBehaviour
{
    private Camera cam;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialFov;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialFov = cam.fieldOfView;
    }

    public void StartBounce() => StartCoroutine(Bounce());
    public void StartFall() => StartCoroutine(Fall());
    public void StartZoomInOut() => StartCoroutine(ZoomInOut());
    public void StartZoomIn() => StartCoroutine(ZoomIn());
    public void StartTilt() => StartCoroutine(Tilt());

    private IEnumerator Bounce()
    {
        float duration = 1f;
        float half = duration / 2f;
        float quarter = duration / 4f;

        // 순차적으로: Y 0 → 1.1 → 0, Z회전 0 → 1 → -1 → 0
        yield return AnimatePositionAndZRotation(initialPosition, initialPosition + Vector3.up * 0.5f, 0.8f, quarter); // Z회전 +1
        yield return AnimatePositionAndZRotation(initialPosition + Vector3.up * 0.5f, initialPosition, -0.2f, quarter); // Z회전 -1
        yield return AnimateZRotation(-0.2f, 0f, half); // Z회전 복귀f
    }

    private IEnumerator Fall()
    {
        float duration = 0.5f;
        Vector3 targetPos = initialPosition + Vector3.down * 0.5f;

        yield return StartCoroutine(AnimatePositionAndFov(initialPosition, targetPos, initialFov, 58f, duration));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(AnimatePositionAndFov(targetPos, initialPosition, 58f, initialFov, duration));
    }

    private IEnumerator ZoomInOut()
    {
        float duration = 0.3f;

        // Fov: 55 → 60 → 50 → (2초 후) → 55
        yield return ChangeFOV(cam.fieldOfView, 60f, duration);
        yield return ChangeFOV(60f, 50f, duration);
        yield return new WaitForSeconds(2f);
        yield return ChangeFOV(50f, initialFov, duration);
    }

    private IEnumerator ZoomIn()
    {
        float duration = 0.3f;

        // Fov: 55 → 60 → (2초 후) → 55
        yield return ChangeFOV(cam.fieldOfView, 60f, duration);
        yield return new WaitForSeconds(2f);
        yield return ChangeFOV(60f, initialFov, duration);
    }

    private IEnumerator Tilt()
    {
        float duration = 0.3f;
        Quaternion tilted = Quaternion.Euler(2.5f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        yield return RotateTo(transform.localRotation, tilted, duration);
        yield return new WaitForSeconds(2f);
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
