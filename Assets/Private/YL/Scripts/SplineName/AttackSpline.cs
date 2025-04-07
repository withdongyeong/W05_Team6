using UnityEngine;

public class AttackSpline : MonoBehaviour, ISplineRotation
{
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, 0f, -75f);
    }
}
