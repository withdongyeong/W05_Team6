using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [Header("Instance Getter")]
    public static GlobalSettings Instance { get; private set; }

    [Header("Game Settings")]
    private float _comboCheckDuration = 4f;
    private float _attackCheckDuration = 1f;
    private float _playerHpMax = 100f;
    private float _playerEnergyMax = 10f;
    private float _playerEnergyRecoveryPerSec = 1f;
    private float _actionCancelRefundAmount = 5f;
    private float _playerDamage = 10f;
    private float _defenseBufferTime = 2f;
    
    [Header("Enemy Settings")]
    private float _enemyMaxHp = 100f;
    private float _enemyAttackDamage = 10f;
    private float _enemyActionInterval = 5f;
    private float _playerPrepareTime = 1f;
    private float _enemyPrepareTime = 20f;
    private float _enemyCounteredTime = 3.0f;

    
    [Header("Getter")]
    public float EnemyCounteredTime => _enemyCounteredTime;
    public float PlayerPrepareTime => _playerPrepareTime;
    public float EnemyPrepareTime => _enemyPrepareTime;
    public float DefenseBufferTime => _defenseBufferTime;
    public float PlayerDamage => _playerDamage;
    public float EnemyAttackDamage => _enemyAttackDamage;
    public float PlayerHpMax => _playerHpMax;
    public float ComboCheckDuration => _comboCheckDuration;
    public float AttackCheckDuration => _attackCheckDuration;
    public float PlayerEnergyMax => _playerEnergyMax;
    public float PlayerEnergyRecoveryPerSec => _playerEnergyRecoveryPerSec;
    public float ActionCancelRefundAmount => _actionCancelRefundAmount;
    public float EnemyMaxHp => _enemyMaxHp;
    public float EnemyActionInterval => _enemyActionInterval; 
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}