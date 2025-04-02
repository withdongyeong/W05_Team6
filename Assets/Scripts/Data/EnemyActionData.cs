using System.Collections.Generic;

[System.Serializable]
public class CounterInfo
{
    public int pilot;
    public string id;
}

[System.Serializable]
public class EnemyActionData
{
    public string enemyId;
    public string id;
    public string type;
    public List<CounterInfo> counteredBy;
}

[System.Serializable]
public class EnemyActionDataList
{
    public List<EnemyActionData> enemyActions;
}