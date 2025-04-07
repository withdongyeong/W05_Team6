using UnityEngine;

public class UppercutSpline : MonoBehaviour, ISplineRotation
{
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, 180f, -20f);
    }
}
