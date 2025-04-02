using System.Collections.Generic;

[System.Serializable]
public class PilotActionData
{
    public int pilot;
    public string id;
    public string type; // "Attack", "Defense"
    public float energyCost;
}

[System.Serializable]
public class PilotActionDataList
{
    public List<PilotActionData> pilotActions;
}