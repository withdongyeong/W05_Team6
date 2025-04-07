using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.VirtualTexturing;

public class Enemy : MonoBehaviour
{
    public string enemyId = "enemy_1";

    private List<EnemyActionData> actions;
    private List<EnemyActionData> actionsBeforeShout; //포효하기전 공격들
    private bool isShouted = false; //포효했는가?
    private float currentHp;
    private bool isAlive = true;
    private Tester _tester;

    public enum EnemyActionState { Idle, Preparing, Countered, Executing }
    private EnemyActionState _state = EnemyActionState.Idle;
    private EnemyActionData _currentAction;
    private EnemyActionData _beforeAction;
    private float _prepareStartTime;
    private float _nextActionTime;
    private float _counteredTime;
    private Animator anim;
    
    [Header("Getter")]
    public EnemyActionState CurrentState => _state;

    void Start()
    {
        currentHp = GlobalSettings.Instance.EnemyMaxHp;
        actions = DataLoader.LoadEnemyActions().enemyActions.FindAll(a => a.enemyId == enemyId);
        actionsBeforeShout = actions.FindAll(a=>GlobalSettings.Instance.AttackBeforeShout.Exists(id => id == a.id));
        _tester = FindAnyObjectByType<Tester>();
        _nextActionTime = Time.time + GlobalSettings.Instance.EnemyActionInterval;
        anim = GetComponent<Animator>();
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
        //행동 정하는 if문.
        if (!isShouted)
        {
            if (currentHp > GlobalSettings.Instance.EnemyMaxHp / 2f)
                _currentAction = actionsBeforeShout[Random.Range(0, actionsBeforeShout.Count)];
            else
                _currentAction = actions.Find(a => a.id == "Shout");//체력 반절 이하면 포효
        }
        else
            do
            {
                _currentAction = actions[Random.Range(0, actions.Count)];//포효 빼야되는데 귀찮아요..
            } while (_currentAction.id == "Shout");
        _prepareStartTime = Time.time;
        _state = EnemyActionState.Preparing;

        if (_tester)
        {
            string text;
            if (!KoreanMapping.EnemySkill.TryGetValue(_currentAction.id, out text)) text = _currentAction.id;
            _tester.UpdateEnemyText($"적 {text}\n준비 중");
        }


        GameManager.Instance.Player.ChangePlayerEnergy(GlobalSettings.Instance.ChargeEnergyPerAction);
        // TODO: 준비 애니메이션 재생 위치
        anim.SetInteger("Prepare", _currentAction.index); // 이거 나중에 actionCount +1로 해야함.

    }

    void MonitorForCounter()
    {
        var playerAction = GameManager.Instance.GetCurrentPlayerAction();
        if (IsCounteredAfterPrepare(_currentAction, playerAction))
        {
            EnterCounteredState();
            return;
        }

        if (Time.time >= _prepareStartTime + _currentAction.castingTime)
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
        anim.SetInteger("Prepare", 0);
    }

    void UpdateCounteredState()
    {
        // 일정 시간 후 카운터 상태 해제
        if (Time.time - _counteredTime >= GlobalSettings.Instance.EnemyCounteredTime)
        {
            Debug.Log("[Enemy] Countered state ended → Returning to Idle");
            _state = EnemyActionState.Idle;
            _nextActionTime = Time.time + 1f; // 카운터 당한 후에는 바로 다시 액션 실행
        }
    }

    void ExecuteCurrentAction()
    {
        if (isAlive && _currentAction != null)
        {
            anim.SetTrigger("Attack");
            if (IsCounteredAfterExecute(_currentAction, GameManager.Instance.GetCurrentPlayerAction()))
            {
                EnterCounteredState();
                return;
            }  
            GameManager.Instance.ReceiveEnemyAction(_currentAction);
            if (_currentAction.id == "Shout")
                isShouted = true;
        }

        _currentAction = null;
        _state = EnemyActionState.Idle;
        anim.SetInteger("Prepare", 0);
        _nextActionTime = Time.time + GlobalSettings.Instance.EnemyActionInterval;
    }

    bool IsCounteredAfterPrepare(EnemyActionData enemyAction, GameManager.PendingAction player)
    {
        if (enemyAction == null || enemyAction.counteredBy == null || player == null)
            return false;

        if (player.occurTime < _prepareStartTime)
            return false;

        var action = player.action;
        if (player.action.type != "Attack")
            return false;
        else
            return enemyAction.counteredBy.Exists(c => (c.id == action.id));
    }

    //방어에도 카운터 당함.
    bool IsCounteredAfterExecute(EnemyActionData enemyAction, GameManager.PendingAction player)
    {
        if (enemyAction == null || enemyAction.counteredBy == null || player == null)
            return false;

        if (player.occurTime < _prepareStartTime)
            return false;

        var action = player.action;

        if (enemyAction.counteredBy.Exists(c => (c.id == action.id)))
            GameManager.Instance.Countered();
        return enemyAction.counteredBy.Exists(c => (c.id == action.id));
    }

    public bool TakeDamage(float amount)
    {
        currentHp -= amount;
        anim.SetTrigger("Damaged");
        if (_tester) _tester.UpdateResultText($"Enemy took {amount} damage. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            isAlive = false;
            if (_tester) _tester.UpdateResultText($"{enemyId} Defeated!");
            anim.SetTrigger("Dead");
            GameManager.Instance.GameEnd(isClear: true);

            return false;
        }
        return true;
    }
}
