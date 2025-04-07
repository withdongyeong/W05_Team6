using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KoreanMapping
{
    Dictionary<string, string> PlayerSkill = new Dictionary<string, string>()
    {
        { "Jab", "잽"},
        { "Uppercut", "어퍼컷"},
        { "CounterAttack", "반격기"},
        { "Evasive", "회피"},
        { "Guard", "가드"},
        { "Kick", "킥"},
    };

    Dictionary<string, string> EnemySkill = new Dictionary<string, string>()
    {
        { "FastAttack", "오른팔 공격"},
        { "MidiumAttack", "왼팔 공격"},
        { "HeavyAttack", "박치기"},
        { "Shout", "포효하기"},
        { "LaserAttack", "레이저 공격"},
        { "Stab", "부리 찌르기"},
    };

    Dictionary<string, string> PilotAction = new Dictionary<string, string>()
    {
        { "Stretch", "뻗기"},
        { "Bending", "구부리기"},
        { "Forward", "앞으로"},
        { "Backward", "뒤로"},
    };
}
