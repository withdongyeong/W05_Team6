
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MJ_ButtonManager : MonoBehaviour
{
    //싱글톤
    public static MJ_ButtonManager instance { get; private set; }
    //조명은 아직 이거 하나만.
    public Light2D globalLight { get; private set; }
    //모니터.
    public GameObject mainMonitor { get; private set; }

    private void Awake()
    {
        if (MJ_ButtonManager.instance == null)
            instance = this;
        else
            Destroy(gameObject);
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        mainMonitor = GameObject.Find("Square");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartRobot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //로봇에 시동을 거는 함수.
    public void StartRobot()
    {
        StartCoroutine(StartRobotCoroutine());
    }

    IEnumerator StartRobotCoroutine()
    {
        float currentTime = 0;
        while(currentTime < 1f)
        {
            globalLight.intensity = Mathf.Lerp(0, 1, currentTime);
            mainMonitor.transform.position = new Vector2(0, Mathf.Lerp(-1, 0, currentTime));
            currentTime+= Time.deltaTime / 3f;
            yield return null;
        }
        Debug.Log("STAND BY");
    }
}
