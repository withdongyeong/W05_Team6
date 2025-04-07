using System.Collections.Generic;

[System.Serializable]
public class PilotActionData
{
    public int pilot;
    public string id;
    public string type; // "Attack", "Defense"
    public float energyCost;
    public int damage; // 데미지. 어택일 때만 존재.
    public float duration; //지속시간. 디펜스일 때만 존재.
    public int cameraIndex;//이 액션이 실행될때, 실행시키고 싶은 카메라 액션의 index를 넣어주면 됨. 카메라 액션의 index는 gamemanager 확인.
}

[System.Serializable]
public class PilotActionDataList
{
    public List<PilotActionData> pilotActions;
}