using UnityEngine;
using System.Collections.Generic;

public class SplineInputController : MonoBehaviour
{
    public SplineFollowerWithRotation followerBH; // RoboBH_0에 붙은 스크립트
    public SplineFollowerWithRotation followerGH; // RoboGH_0에 붙은 스크립트

    // 어떤 Spline을 누르면 어떤 오브젝트가 움직일지 정의
    private Dictionary<string, SplineFollowerWithRotation> splineToFollower;

    void Awake()
    {
        splineToFollower = new Dictionary<string, SplineFollowerWithRotation>
        {
            { "GardLeftSpline", followerBH },
            { "AttackSpline", followerBH },
            { "JapSpline", followerBH },

            { "GardRightSpline", followerGH },
            { "UppercutSpline", followerGH },
            { "KickSpline", followerGH }
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TriggerSpline("JapSpline");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            TriggerSpline("UppercutSpline");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            TriggerSpline("AttackSpline");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            TriggerSpline("GardLeftSpline");

        if (Input.GetKeyDown(KeyCode.Alpha5))
            TriggerSpline("GardRightSpline");

        if (Input.GetKeyDown(KeyCode.Alpha6))
            TriggerSpline("KickSpline");
    }

    void TriggerSpline(string splineName)
    {
        if (splineToFollower.TryGetValue(splineName, out var follower))
        {
            follower.PlaySpline(splineName);
        }
        else
        {
            Debug.LogWarning($"[SplineInputController] '{splineName}' 에 대한 대상 Follower가 정의되어 있지 않음");
        }
    }
}
