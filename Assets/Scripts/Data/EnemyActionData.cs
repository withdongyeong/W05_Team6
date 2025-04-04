using System.Collections.Generic;

[System.Serializable]
public class CounterInfo
{
    public int pilot;     // 단일 행동이면 실제 pilot 번호, 콤보면 무시
    public string id;     // 행동 ID (단일이든 콤보든)
    public string type;   // "Normal" or "Combo"
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