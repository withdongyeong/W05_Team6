using UnityEngine;

public class KickSpline : MonoBehaviour, ISplineRotation
{
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, 180f, -66f);
    }
}
