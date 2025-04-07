using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ActionComboDataList _comboData;
    private Enemy _enemy;
    public Player Player { get { return _player; } }
    private Player _player;
    private Tester _tester;

    private class TimedAction
    {
        public int pilot;
        public PilotActionData action;
        public float timestamp;
    }

    public class PendingAction
    {
        public int pilot;
        public PilotActionData action;
        public float occurTime;
        public float endTime;
    }

    private List<TimedAction> _actionQueue = new();
    private HashSet<int> _activePilotIds = new();

    private PendingAction _playerAction;
    private EnemyActionData _enemyAction;
    private float _enemyOccurTime;
    private float _enemyEndTime;

    public List<Pilot> Pilots;

    public Action pilotActionOver;

    private Action startBounceJap;
    private Action startFall;
    private Action startZoomInOut;
    private Action startZoomIn;
    private Action startTilt;
    private Action startBouneKick;

    private List<Action> cameraActions;
    public CameraMotionController _mainCamera { get; private set; }

    void Awake()
    {
        if (GameManager.Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _tester = FindAnyObjectByType<Tester>();
        _comboData = DataLoader.LoadActionCombos();
        _enemy = FindAnyObjectByType<Enemy>();
        _player = FindAnyObjectByType<Player>();
        _mainCamera = FindAnyObjectByType<CameraMotionController>();
    }
    void Start()
    {
        StartCoroutine(ComboProcessingLoop());
        SubscribeCameraFunc();//밑의 cameraactions에 들어갈 이벤트들 구독
        cameraActions = new()
        {
            startBounceJap,
            startFall,
            startZoomInOut,
            startZoomIn,
            startTilt,
            startBouneKick
        };
    }

    public void ReceivePlayerAction(int pilotId, PilotActionData action)
    {
        if (_activePilotIds.Contains(pilotId)) return;

        _activePilotIds.Add(pilotId);
        _actionQueue.Add(new TimedAction
        {
            pilot = pilotId,
            action = action,
            timestamp = Time.time
        });
    }
    public PendingAction GetCurrentPlayerAction()
    {
        return _playerAction;
    }

    IEnumerator ComboProcessingLoop()
    {
        while (true)
        {
            TryProcessActionQueue();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    void Update()
    {
        if (_tester != null)
        {
            string status = "";

            float now = Time.time;

            bool isPlayerDefending = _playerAction != null &&
                                     _playerAction.action.type == "Defense" &&
                                     now <= _playerAction.endTime;

            bool isEnemyDefending = _enemyAction != null &&
                                    _enemyAction.type == "Defense" &&
                                    now <= _enemyEndTime;

            if (isPlayerDefending) status += "플레이어 방어 중\n";
            if (isEnemyDefending) status += "적 방어 중\n";
            if (status == "") status = "방어 중인 캐릭터 없음";

            _tester.UpdateStatusText(status);
        }
    }


    void TryProcessActionQueue()
    {
        float now = Time.time;

        // 윈도우 내에 있는 액션만 추림
        var windowedActions = _actionQueue.FindAll(a => now - a.timestamp <= GlobalSettings.Instance.ComboCheckDuration);

        foreach (var combo in _comboData.actionCombos)
        {
            var match = TryMatchCombo(windowedActions, combo);
            if (match != null)
            {
                StartCoroutine(CastingCombo(new PendingAction
                {
                    pilot = 5,
                    action = new PilotActionData
                    {
                        pilot = 5,
                        id = combo.result,
                        type = combo.type,
                        damage = combo.type == "Attack" || combo.result == "CounterAttack" ? combo.damage : 0,
                        duration = combo.type == "Defense" ? combo.duration : 0,
                        cameraIndex = combo.cameraIndex
                    }
                },
                combo.castingTime));
                //ReceiveResolvedPlayerAction(new PendingAction
                //{
                //    pilot = 5,
                //    action = new PilotActionData { pilot = 5, id = combo.result, type = combo.type,
                //    damage = combo.type == "Attack" || combo.result == "CounterAttack" ? combo.damage : 0,
                //    duration = combo.type == "Defense" ? combo.duration : 0}
                //});

                foreach (var used in match)
                {
                    _activePilotIds.Remove(used.pilot);
                    _actionQueue.Remove(used);
                }
                return;
            }
        }

        // 콤보 불가능한 액션은 단일 실행
        for (int i = 0; i < _actionQueue.Count;)
        {
            var current = _actionQueue[i];
            if (now - current.timestamp > GlobalSettings.Instance.ComboCheckDuration)
            {
                _activePilotIds.Remove(current.pilot);
                pilotActionOver();
                _actionQueue.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }


    List<TimedAction> TryMatchCombo(List<TimedAction> availableActions, ComboData combo)
    {
        var matched = new List<TimedAction>();

        foreach (var input in combo.inputs)
        {
            var found = availableActions.Find(a => a.pilot == input.pilot && a.action.id == input.id && !matched.Contains(a));
            if (found != null)
            {
                matched.Add(found);
            }
            else
            {
                return null; // 하나라도 안 맞으면 불일치
            }
        }

        return matched;
    }

    IEnumerator CastingCombo(PendingAction combo, float time)
    {
        Debug.Log(time + "만큼 기다리기 시작!");
        yield return new WaitForSeconds(time);
        ReceiveResolvedPlayerAction(combo);
        pilotActionOver();
    }

    void ReceiveResolvedPlayerAction(PendingAction resolved)
    {
        resolved.occurTime = Time.time;
        resolved.endTime = resolved.action.type == "Defense"
            ? Time.time + resolved.action.duration
            : Time.time;
        
        _playerAction = resolved;

        if (_tester)
            _tester.UpdatePlayerText($"Player Action Executed: Pilot{resolved.pilot}_{resolved.action.id} [{resolved.action.type}]");
        UIManager.Instance.UpdateStatusUI(resolved.action.id);

        ResolvePlayerAction(resolved);
    }

    public void ReceiveEnemyAction(EnemyActionData action)
    {
        _enemyAction = action;
        _enemyOccurTime = Time.time;
        _enemyEndTime = action.type == "Defense"
            ? Time.time + GlobalSettings.Instance.DefenseBufferTime
            : Time.time;

        if (_tester)
            _tester.UpdateEnemyText($"Enemy Action Executed: {action.id}");

        ResolveEnemyAction(action);
    }

    void ResolvePlayerAction(PendingAction action)
    {
        //액션에 맞는 카메라 함수 호출
        cameraActions[action.action.cameraIndex]();
        if (action.action.type == "Attack")
        {
            bool enemyBlocked = _enemyAction != null &&
                                _enemyAction.type == "Defense" &&
                                _enemyOccurTime <= action.occurTime &&
                                action.occurTime <= _enemyEndTime;

            if (enemyBlocked)
            {
                _tester.UpdateResultText($"[Player] Pilot{action.pilot}_{action.action.id} 공격 → [Enemy] 방어 성공");
            }
            else
            {
                bool isAlive = _enemy.TakeDamage(action.action.damage);
                _tester.UpdateResultText($"[Player] Pilot{action.pilot}_{action.action.id} 공격 → [Enemy] 피해 {(isAlive ? "입음" : "사망")}");
            }
        }
    }




    void ResolveEnemyAction(EnemyActionData action)
    {
        if (action.type == "Attack")
        {
            bool playerBlocked = _playerAction != null &&
                                 _playerAction.action.type == "Defense" &&
                                 _playerAction.occurTime <= _enemyOccurTime &&
                                 _enemyOccurTime <= _playerAction.endTime;

            if (playerBlocked)
            {
                _tester.UpdateResultText($"[Enemy] {action.id} 공격 → [Player] 방어 성공");
            }
            else
            {
                bool isAlive = _player.TakeDamage(GlobalSettings.Instance.EnemyAttackDamage);
                _tester.UpdateResultText($"[Enemy] {action.id} 공격 → [Player] 피해 {(isAlive ? "입음" : "사망")}");
            }
        }
    }

    public void Countered()
    {
        if(_playerAction != null && _playerAction.action.type == "Defense")
        {
            Debug.Log("카운터 발생 및 부가 효과 발동");
            if (_playerAction.action.id == "CounterAttack")
                _enemy.TakeDamage(_playerAction.action.damage);
            else if (_playerAction.action.id == "Evasive")
                _player.ChangePlayerEnergy(3);

        }
    }

    //액션이랑 카메라 함수들 매칭.
    private void SubscribeCameraFunc()
    {
        startBounceJap += _mainCamera.StartBounceJap;
        startFall += _mainCamera.StartFall;
        startTilt += _mainCamera.StartTilt;
        startZoomIn += _mainCamera.StartZoomIn;
        startZoomInOut += _mainCamera.StartZoomInOut;
        startBouneKick += _mainCamera.StartBounceKick;
    }

    public void GameEnd(bool isClear)
    {
        UIManager.Instance.SetGameEndPanel(isClear);
    }
}
