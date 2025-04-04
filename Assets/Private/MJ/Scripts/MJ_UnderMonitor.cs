using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MJ_UnderMonitor : MonoBehaviour
{
    private ITab currentTab;
    private Tabs currentTabEnum;
    
    private ITab commandTab;
    private ITab infoTab;
    
    public enum Tabs
    {
        Command,
        Info
    }
    
    private void Awake()
    {
        commandTab = transform.GetChild(2).GetComponent<ITab>();
        infoTab = transform.GetChild(3).GetComponent<ITab>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTab = commandTab;
        currentTabEnum = Tabs.Command;
    }

    // Update is called once per frame
    void Update()
    {
        currentTab?.UpdateTab();
    }

    public void ChangeMonitorTab(int indexNum)
    {
        Tabs newTabEnum = (Tabs)indexNum;
        if (currentTabEnum == newTabEnum)
        {
            Debug.Log("같은 탭으로 변경하려 했음.");
            return;
        }
        currentTabEnum = newTabEnum;
        ITab newTab = commandTab;
        switch (newTabEnum)
        {
            case Tabs.Command:
                newTab = commandTab;
                break;
            case Tabs.Info:
                newTab = infoTab;
                break;
        }
        currentTab?.StopTab();
        currentTab = newTab;
        currentTab?.StartTab();
    }
    
    
}
