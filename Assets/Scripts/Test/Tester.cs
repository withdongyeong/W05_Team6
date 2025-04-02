using UnityEngine;
using TMPro;

public class Tester : MonoBehaviour
{
    [Header("Instance Getter")]
    public static Tester Instance { get; private set; }
    
    private Player _player;
    private int? selectedPilot = null;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI resultText;

    public void UpdatePlayerText(string contents)
    {
        playerText.text = contents;
    }
    
    public void UpdateEnemyText(string contents)
    {
        enemyText.text = contents;
    }
    public void UpdateResultText(string contents)
    {
        resultText.text = contents;
    }
    
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        statusText.text = $"플레이어 공격 중 : {GameManager.Instance.IsPlayerAttacking.ToString()}";
        statusText.text += $"\n플레이어 방어 중 : {GameManager.Instance.IsPlayerDefending.ToString()}";
        statusText.text += $"\n적 공격 중 : {GameManager.Instance.IsEnemyAttacking.ToString()}";
        statusText.text += $"\n적 방어 중 : {GameManager.Instance.IsEnemyDefending.ToString()}";
        // Step 1: 파일럿 선택 (1~4)
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedPilot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedPilot = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedPilot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedPilot = 4;

        // Step 2: 행동 ID 선택 (Q~R)
        if (selectedPilot != null)
        {
            if (Input.GetKeyDown(KeyCode.Q)) TriggerAction("01");
            else if (Input.GetKeyDown(KeyCode.W)) TriggerAction("02");
            else if (Input.GetKeyDown(KeyCode.E)) TriggerAction("03");
            else if (Input.GetKeyDown(KeyCode.R)) TriggerAction("04");
        }

        // 취소 키: 5~8
        if (Input.GetKeyDown(KeyCode.Alpha5)) CancelAction(1);
        if (Input.GetKeyDown(KeyCode.Alpha6)) CancelAction(2);
        if (Input.GetKeyDown(KeyCode.Alpha7)) CancelAction(3);
        if (Input.GetKeyDown(KeyCode.Alpha8)) CancelAction(4);
    }

    void TriggerAction(string actionId)
    {
        int pilotIndex = selectedPilot.Value - 1;
        if (pilotIndex < 0 || pilotIndex >= _player.transform.childCount) return;

        var pilot = _player.transform.GetChild(pilotIndex).GetComponent<Pilot>();

        var action = new PilotActionData
        {
            pilot = selectedPilot.Value,
            id = actionId,
            type = "Attack",
            energyCost = 10
        };

        _player.IssueCommand(pilot, action);

        selectedPilot = null;
    }

    void CancelAction(int pilotNumber)
    {
        int index = pilotNumber - 1;
        if (index < 0 || index >= _player.transform.childCount) return;

        var pilot = _player.transform.GetChild(index).GetComponent<Pilot>();
        _player.CancelCommand(pilot);
    }
}
