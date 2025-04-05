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
    public string result;            // 결과 행동 ID // 스킬이름
    public string type; //어택인지, 디펜스인지.
    public int damage; // 데미지. 어택일 때만 존재.
    public float duration; //지속시간. 디펜스일 때만 존재.

}

[System.Serializable]
public class ActionComboDataList
{
    public List<ComboData> actionCombos;
}