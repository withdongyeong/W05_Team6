using UnityEngine;

public class GardRightSpline : MonoBehaviour, ISplineRotation
{
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, 0f, 0f);
    }
}
