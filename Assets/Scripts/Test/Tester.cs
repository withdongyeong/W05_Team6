using UnityEngine;
using TMPro;

public class Tester : MonoBehaviour
{
    public static Tester Instance { get; private set; }

    private Player _player;
    private int? selectedPilot = null;
    private PilotActionDataList _pilotActions;


    public TextMeshProUGUI statusText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI enemyStateText;
    public TextMeshProUGUI pilotStatesText;
    
    private float _clearLogInterval = 3f;
    private float _lastLogTime = 0f;
    private string _logBuffer = "";
    
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _pilotActions = DataLoader.LoadPilotActions();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Update()
    {
        var enemy = FindAnyObjectByType<Enemy>();
        if (enemy != null && enemyStateText != null)
        {
            enemyStateText.text = $"Enemy State: {enemy.CurrentState}";
        }
        UpdatePilotStates();

        // 로그 자동 클리어
        if (Time.time - _lastLogTime > _clearLogInterval)
        {
            _logBuffer = "";
            resultText.text = "";
        }

        // 파일럿 선택
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedPilot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedPilot = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedPilot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedPilot = 4;

        // 행동 선택
        if (selectedPilot != null)
        {
            if (Input.GetKeyDown(KeyCode.Q)) TriggerAction("01");
            else if (Input.GetKeyDown(KeyCode.W)) TriggerAction("02");
            else if (Input.GetKeyDown(KeyCode.E)) TriggerAction("03");
            else if (Input.GetKeyDown(KeyCode.R)) TriggerAction("04");
        }

        // 행동 취소
        if (Input.GetKeyDown(KeyCode.Alpha5)) CancelAction(1);
        if (Input.GetKeyDown(KeyCode.Alpha6)) CancelAction(2);
        if (Input.GetKeyDown(KeyCode.Alpha7)) CancelAction(3);
        if (Input.GetKeyDown(KeyCode.Alpha8)) CancelAction(4);
    }
    void UpdatePilotStates()
    {
        var player = FindAnyObjectByType<Player>();
        if (player == null || pilotStatesText == null) return;

        string status = "";
        int count = player.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            var pilot = player.transform.GetChild(i).GetComponent<Pilot>();
            string state = pilot.IsPreparing ? "Preparing" : "Idle";
            status += $"Pilot {pilot.pilotId}: {state}\n";
        }

        pilotStatesText.text = status;
    }

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
        _lastLogTime = Time.time;

        _logBuffer += contents + "\n";
        resultText.text = _logBuffer;
    }
    public void UpdateStatusText(string contents)
    {
        statusText.text = contents;
    }


    public void TriggerAction(string actionId)
    {
        int pilotIndex = selectedPilot.Value - 1;
        if (pilotIndex < 0 || pilotIndex >= _player.transform.childCount) return;

        var pilot = _player.transform.GetChild(pilotIndex).GetComponent<Pilot>();

        var matchedAction = _pilotActions.pilotActions.Find(a =>
            a.pilot == selectedPilot.Value && a.id == actionId);

        if (matchedAction == null)
        {
            Debug.LogWarning($"[Tester] No matching action for Pilot{selectedPilot.Value} ID:{actionId}");
            return;
        }

        _player.IssueCommand(pilot, matchedAction);
        selectedPilot = null;
    }


    void CancelAction(int pilotNumber)
    {
        int index = pilotNumber - 1;
        if (index < 0 || index >= _player.transform.childCount) return;

        var pilot = _player.transform.GetChild(index).GetComponent<Pilot>();
        _player.CancelCommand(pilot);
    }
    
    public PilotActionDataList PilotActions => _pilotActions;

    public void ChangeSelectedPilot(int index)
    {
        selectedPilot = index;
    }
}
