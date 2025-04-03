
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MJ_ButtonManager : MonoBehaviour
{
    //�̱���
    public static MJ_ButtonManager instance { get; private set; }
    //������ ���� �̰� �ϳ���.
    public Light2D globalLight { get; private set; }
    //�����.
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

    //�κ��� �õ��� �Ŵ� �Լ�.
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
