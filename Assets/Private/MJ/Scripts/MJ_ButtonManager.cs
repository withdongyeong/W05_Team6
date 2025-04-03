
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
    public GameObject monitor { get; private set; }

    public bool isStarted { get; private set; }

    public bool mainMonitorActive { get; private set; }

    private void Awake()
    {
        if (MJ_ButtonManager.instance == null)
            instance = this;
        else
            Destroy(gameObject);
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        monitor = GameObject.Find("Square");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //로봇에 시동을 거는 함수.
    public void StartRobot()
    {
        if(!isStarted)
            StartCoroutine(StartRobotCo());
    }

    IEnumerator StartRobotCo()
    {
        isStarted = true;
        float currentTime = 0;
        while(currentTime < 1f)
        {
            globalLight.intensity = Mathf.Lerp(0, 1, currentTime);
            currentTime+= Time.deltaTime / 3f;
            yield return null;
        }
        Debug.Log("STAND BY");
    }

    //메인 모니터 올리는 함수
    public void ActivateMainMonitor()
    {
        if (isStarted && !mainMonitorActive)
            StartCoroutine(ActivateMainMonitorCo());
        else if (isStarted && mainMonitorActive)
            StartCoroutine(DeactivateMainMonitorCo());

    }

    IEnumerator ActivateMainMonitorCo()
    {  
        float currentTime = 0;
        while (currentTime < 1f)
        {
            monitor.transform.position = new Vector2(0.004f, Mathf.Lerp(-2.685f, 4.7f, currentTime));
            currentTime += Time.deltaTime *5f;
            yield return null;
        }
        mainMonitorActive = true;
        Debug.Log("MainMonitorActive");
    }
    IEnumerator DeactivateMainMonitorCo()
    {
        float currentTime = 0;
        while (currentTime < 1f)
        {
            monitor.transform.position = new Vector2(0.004f, Mathf.Lerp(4.7f, -2.685f, currentTime));
            currentTime += Time.deltaTime * 5f;
            yield return null;
        }
        mainMonitorActive = false;
        Debug.Log("MainMonitorDeactive");
    }
}
