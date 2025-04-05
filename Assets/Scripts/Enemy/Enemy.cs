using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public string enemyId = "enemy_1";

    private List<EnemyActionData> actions;
    private float currentHp;
    private bool isAlive = true;
    private Tester _tester;

    public enum EnemyActionState { Idle, Preparing, Countered, Executing }
    private EnemyActionState _state = EnemyActionState.Idle;
    private EnemyActionData _currentAction;
    private float _prepareStartTime;
    private float _nextActionTime;
    private float _counteredTime;
    
    [Header("Getter")]
    public EnemyActionState CurrentState => _state;

    void Start()
    {
        currentHp = GlobalSettings.Instance.EnemyMaxHp;
        actions = DataLoader.LoadEnemyActions().enemyActions.FindAll(a => a.enemyId == enemyId);
        _tester = FindAnyObjectByType<Tester>();
        _nextActionTime = Time.time + GlobalSettings.Instance.EnemyActionInterval;
    }

    void Update()
    {
        if (!isAlive) return;

        switch (_state)
        {
            case EnemyActionState.Idle:
                if (Time.time >= _nextActionTime)
                    StartPreparingAction();
                break;

            case EnemyActionState.Preparing:
                MonitorForCounter();
                break;

            case EnemyActionState.Countered:
                UpdateCounteredState();
                break;
        }
    }

    void StartPreparingAction()
    {
        _currentAction = actions[Random.Range(0, actions.Count)];
        _prepareStartTime = Time.time;
        _state = EnemyActionState.Preparing;

        if (_tester)
            _tester.UpdateEnemyText($"Enemy preparing action: {_currentAction.id}");

        GameManager.Instance.Player.ChangePlayerEnergy(GlobalSettings.Instance.ChargeEnergyPerAction);
        // TODO: 준비 애니메이션 재생 위치
    }

    void MonitorForCounter()
    {
        var playerAction = GameManager.Instance.GetCurrentPlayerAction();
        if (IsCounteredAfterPrepare(_currentAction, playerAction))
        {
            EnterCounteredState();
            return;
        }

        if (Time.time >= _prepareStartTime + GlobalSettings.Instance.EnemyPrepareTime)
        {
            _state = EnemyActionState.Executing;
            ExecuteCurrentAction();
        }
    }

    void EnterCounteredState()
    {
        _state = EnemyActionState.Countered;
        _counteredTime = Time.time;
        _currentAction = null;
        Debug.Log("[Enemy] Entered Countered state");
        if (_tester)
            _tester.UpdateResultText($"[Counter] Enemy action was countered!");

        // TODO: 파훼 애니메이션/이펙트 등
    }

    void UpdateCounteredState()
    {
        // 일정 시간 후 카운터 상태 해제
        if (Time.time - _counteredTime >= GlobalSettings.Instance.EnemyCounteredTime)
        {
            Debug.Log("[Enemy] Countered state ended → Returning to Idle");
            _state = EnemyActionState.Idle;
            _nextActionTime = Time.time + GlobalSettings.Instance.EnemyActionInterval;
        }
    }

    void ExecuteCurrentAction()
    {
        if (isAlive && _currentAction != null)
        {
            GameManager.Instance.ReceiveEnemyAction(_currentAction);
        }

        _currentAction = null;
        _state = EnemyActionState.Idle;
        _nextActionTime = Time.time + GlobalSettings.Instance.EnemyActionInterval;
    }

    bool IsCounteredAfterPrepare(EnemyActionData enemyAction, GameManager.PendingAction player)
    {
        if (enemyAction == null || enemyAction.counteredBy == null || player == null)
            return false;

        if (player.occurTime < _prepareStartTime)
            return false;

        var action = player.action;

        return enemyAction.counteredBy.Exists(c => (c.id == action.id));
    }

    public bool TakeDamage(float amount)
    {
        currentHp -= amount;
        if (_tester) _tester.UpdateResultText($"Enemy took {amount} damage. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            isAlive = false;
            if (_tester) _tester.UpdateResultText($"{enemyId} Defeated!");
            return false;
        }
        return true;
    }
}
