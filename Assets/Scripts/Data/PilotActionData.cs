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
}

[System.Serializable]
public class PilotActionDataList
{
    public List<PilotActionData> pilotActions;
}