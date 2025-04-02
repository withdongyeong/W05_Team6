using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;

public class Enemy : MonoBehaviour
{
    public string enemyId = "enemy_1";

    private List<EnemyActionData> actions;
    private float currentHp;
    private bool isAlive = true;
    private Tester _tester;

    void Start()
    {
        currentHp = GlobalSettings.Instance.EnemyMaxHp;
        actions = DataLoader.LoadEnemyActions().enemyActions.FindAll(a => a.enemyId == enemyId);
        StartCoroutine(ActionLoop());
        _tester = FindAnyObjectByType<Tester>();
    }

    IEnumerator ActionLoop()
    {
        while (isAlive)
        {
            // 1. 대기 후 행동 선택
            yield return new WaitForSeconds(GlobalSettings.Instance.EnemyActionInterval);
            var action = actions[Random.Range(0, actions.Count)];

            // 2. 준비 시작
            StartCoroutine(PrepareAndSendAction(action));
        }
    }

    IEnumerator PrepareAndSendAction(EnemyActionData action)
    {
        if (_tester) _tester.UpdateEnemyText($"Enemy preparing action: {action.id}");

        yield return new WaitForSeconds(GlobalSettings.Instance.EnemyPrepareTime);

        if (isAlive) // 여전히 살아있다면 실행
            GameManager.Instance.ReceiveEnemyAction(action);
    }

    public bool TakeDamage(float amount)
    {
        currentHp -= amount;
        if (_tester) _tester.UpdateResultText($"Enemy took {amount} damage. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            isAlive = false;
            Debug.Log($"{enemyId} Defeated!");
            // TODO 몬스터 죽음 처리
            return false;
        }
        return true;
    }
}