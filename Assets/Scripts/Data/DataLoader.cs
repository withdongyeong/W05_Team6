using UnityEngine;

public static class DataLoader
{
    private static string _pathPilotAction = "Data/pilot_actions";
    private static string _pathEnemyAction = "Data/enemy_actions";
    private static string _pathActionCombo = "Data/action_combo_table";
    public static PilotActionDataList LoadPilotActions()
    {
        var json = Resources.Load<TextAsset>(_pathPilotAction);
        return JsonUtility.FromJson<PilotActionDataList>(json.text);
    }

    public static EnemyActionDataList LoadEnemyActions()
    {
        var json = Resources.Load<TextAsset>(_pathEnemyAction);
        return JsonUtility.FromJson<EnemyActionDataList>(json.text);
    }

    public static ActionComboDataList LoadActionCombos()
    {
        var json = Resources.Load<TextAsset>(_pathActionCombo);
        return JsonUtility.FromJson<ActionComboDataList>(json.text);
    }
}