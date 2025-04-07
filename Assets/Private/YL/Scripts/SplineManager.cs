using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineManager : MonoBehaviour
{
    private Dictionary<string, GameObject> splineDict;

    [SerializeField] private List<string> registeredSplines = new List<string>();

    void Awake()
    {
        splineDict = new Dictionary<string, GameObject>();
        registeredSplines.Clear();

        RegisterSpline<JapSpline>();
        RegisterSpline<UppercutSpline>();
        RegisterSpline<AttackSpline>();
        RegisterSpline<GardLeftSpline>();
        RegisterSpline<GardRightSpline>();
        RegisterSpline<KickSpline>();
    }

    private void RegisterSpline<T>() where T : MonoBehaviour
    {
        var component = FindAnyObjectByType<T>();
        if (component != null)
        {
            string name = typeof(T).Name;
            if (!splineDict.ContainsKey(name))
            {
                splineDict.Add(name, component.gameObject);
                registeredSplines.Add(name);
                Debug.Log($"[SplineManager] 등록됨: {name}");
            }
        }
        else
        {
            Debug.LogWarning($"[SplineManager] {typeof(T).Name} 타입을 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    public GameObject GetSpline(string name)
    {
        if (splineDict.TryGetValue(name, out var obj))
            return obj;

        Debug.LogWarning($"Spline '{name}' 을 찾을 수 없습니다.");
        return null;
    }

    public List<string> GetRegisteredSplineNames()
    {
        return new List<string>(splineDict.Keys);
    }
}
