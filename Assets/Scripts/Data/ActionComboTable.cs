using System.Collections.Generic;

[System.Serializable]
public class ComboInput
{
    public int pilot;
    public string id;
}

[System.Serializable]
public class ComboData
{
    public List<ComboInput> inputs;  // 콤보 입력들을 리스트로
    public string result;            // 결과 행동 ID
}

[System.Serializable]
public class ActionComboDataList
{
    public List<ComboData> actionCombos;
}