using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ActionComboDataList _comboData;
    private bool _isPlayerAttacking;
    private bool _isPlayerDefending;
    private bool _isEnemyAttacking;
    private bool _isEnemyDefending;
    private EnemyActionData _currentEnemyAction;
    private Enemy _enemy;
    private Player _player;
    private Tester _tester;

    private class TimedAction
    {
        public int pilot;
        public PilotActionData action;
        public float timestamp;
    }

    private List<TimedAction> _actionQueue = new();
    private HashSet<int> _activePilotIds = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _tester = FindAnyObjectByType<Tester>();
        _comboData = DataLoader.LoadActionCombos();
        _enemy = FindAnyObjectByType<Enemy>();
        _player = FindAnyObjectByType<Player>();
        StartCoroutine(ComboProcessingLoop());
    }

    public void ReceivePlayerAction(int pilotId, PilotActionData action)
    {
        if (_activePilotIds.Contains(pilotId))
        {
            Debug.LogWarning($"Pilot {pilotId} action already active. Ignoring duplicate execution.");
            return;
        }

        _activePilotIds.Add(pilotId);
        _actionQueue.Add(new TimedAction
        {
            pilot = pilotId,
            action = action,
            timestamp = Time.time
        });
    }

    IEnumerator ComboProcessingLoop()
    {
        while (true)
        {
            TryProcessActionQueue();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TryProcessActionQueue()
    {
        int i = 0;

        while (i < _actionQueue.Count)
        {
            var current = _actionQueue[i];

            bool matched = false;
            for (int j = i + 1; j < _actionQueue.Count; j++)
            {
                var next = _actionQueue[j];
                if (next.timestamp - current.timestamp <= GlobalSettings.Instance.ComboCheckDuration)
                {
                    var combo = CheckComboAction(current.pilot, current.action.id, next.pilot, next.action.id);
                    if (combo != null)
                    {
                        ExecutePlayerAction(combo);
                        _activePilotIds.Remove(current.pilot);
                        _activePilotIds.Remove(next.pilot);
                        _actionQueue.RemoveAt(j);
                        _actionQueue.RemoveAt(i);
                        matched = true;
                        break; // 다시 처음부터
                    }
                }
            }

            if (!matched)
            {
                if (Time.time - current.timestamp > GlobalSettings.Instance.ComboCheckDuration)
                {
                    ExecutePlayerAction(current.action);
                    _activePilotIds.Remove(current.pilot);
                    _actionQueue.RemoveAt(i);
                    continue; // 그대로 다음
                }
                i++; // 다음 항목으로
            }
        }
    }


    PilotActionData CheckComboAction(int pilotA, string idA, int pilotB, string idB)
    {
        foreach (var combo in _comboData.actionCombos)
        {
            bool match =
                (combo.inputA.pilot == pilotA && combo.inputA.id == idA &&
                 combo.inputB.pilot == pilotB && combo.inputB.id == idB)
                ||
                (combo.inputB.pilot == pilotA && combo.inputB.id == idA &&
                 combo.inputA.pilot == pilotB && combo.inputA.id == idB);

            if (match)
            {
                return new PilotActionData { pilot = 5, id = combo.result, type = "Combo" };
            }
        }
        return null;
    }

    void ExecutePlayerAction(PilotActionData action)
    {
        if (_tester) _tester.UpdatePlayerText($"Player Action Executed: Pilot{action.pilot}_{action.id} [{action.type}]");
        StartCoroutine(HandlePlayerAction(action));
    }

    IEnumerator HandlePlayerAction(PilotActionData action)
    {
        if (action.type == "Attack" || action.type == "Combo") _isPlayerAttacking = true;
        if (action.type == "Defense") _isPlayerDefending = true;

        yield return new WaitForSeconds(GlobalSettings.Instance.AttackCheckDuration);

        // TODO 플레이어 공격 및 방어 애니메이션 재생
        
        if (action.type == "Attack" || action.type == "Combo")
        {
            bool enemyBlocked = _isEnemyDefending &&
                                _currentEnemyAction != null &&
                                _currentEnemyAction.counteredBy.Exists(c => c.pilot == action.pilot && c.id == action.id);

            if (enemyBlocked)
            {
                Debug.Log("Enemy blocked the attack.");
            }
            else
            {
                Debug.Log("Enemy took damage!");
                if (!_enemy.TakeDamage(GlobalSettings.Instance.PlayerDamage))
                {
                    // TODO 플레이어 승리
                    Debug.Log("Enemy defeated.");
                }
            }
        }

        if (action.type == "Defense" && _isEnemyAttacking)
        {
            Debug.Log("Player blocked the attack.");
        }

        yield return new WaitForSeconds(GlobalSettings.Instance.DefenseBufferTime);

        if (action.type == "Attack" || action.type == "Combo") _isPlayerAttacking = false;
        if (action.type == "Defense") _isPlayerDefending = false;
    }

    public void ReceiveEnemyAction(EnemyActionData enemyAction)
    {
        _currentEnemyAction = enemyAction;
        StartCoroutine(EnemyActionResolution(enemyAction));
    }

    IEnumerator EnemyActionResolution(EnemyActionData enemyAction)
    {
        _isEnemyAttacking = enemyAction.type == "Attack";
        _isEnemyDefending = enemyAction.type == "Defense";

        yield return new WaitForSeconds(GlobalSettings.Instance.AttackCheckDuration);
        
        if (_tester) _tester.UpdateEnemyText($"Enemy Action Executed: {enemyAction.id}");

        if (enemyAction.type == "Attack")
        {
            if (!_isPlayerDefending)
            {
                if (!_player.TakeDamage(GlobalSettings.Instance.EnemyAttackDamage))
                {
                    Debug.Log("Player defeated.");
                    // TODO: 게임 패배 처리
                }
            }
            else
            {
                if (_tester) _tester.UpdatePlayerText("Enemy attack was blocked.");
            }
        }

        if (enemyAction.type == "Defense" && _isPlayerAttacking)
        {
            if (_tester) _tester.UpdatePlayerText("Enemy blocked the attack.");
        }

        yield return new WaitForSeconds(GlobalSettings.Instance.DefenseBufferTime);

        _isEnemyAttacking = false;
        _isEnemyDefending = false;
    }
    
    // Getter
    public bool IsPlayerAttacking => _isPlayerAttacking;
    public bool IsPlayerDefending => _isPlayerDefending;
    public bool IsEnemyAttacking => _isEnemyAttacking;
    public bool IsEnemyDefending => _isEnemyDefending;
}
