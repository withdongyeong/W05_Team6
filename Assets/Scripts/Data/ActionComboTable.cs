using System.Collections.Generic;

[System.Serializable]
public class ComboInput
{
    public int pilot;
    public string id;
}

[System.Serializable]
public class ActionCombo
{
    public ComboInput inputA;
    public ComboInput inputB;
    public string result; // 결과 행동명 (예: "PowerSmash")
}

[System.Serializable]
public class ActionComboDataList
{
    public List<ActionCombo> actionCombos;
}