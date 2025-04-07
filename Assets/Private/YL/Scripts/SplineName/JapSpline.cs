using UnityEngine;

public class JapSpline : MonoBehaviour, ISplineRotation
{
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, 0f, -52f);
    }
}
