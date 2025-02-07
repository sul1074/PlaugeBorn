using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgrade : MonoBehaviour // 검사 무기 업그레이드
{   
    private Stat stat;
    private SwordSkillAttack swordSkillAttack;

    [SerializeField] public enum SwordType { // 업그레이드 검 3종류
        ChargeTimeReducton,
        CooldownReduction,
        StunIncrease
    }
    public SwordType swordType;
    [SerializeField] private int amount;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {   
            SwordEnforce();
            Destroy(gameObject);
        }
    }

    void SwordEnforce() { // 검사 강화
        switch (swordType) {
            case SwordType.ChargeTimeReducton:
            // swordSkillAttack.maxChargeTime -= 1f;
                break;
            case SwordType.CooldownReduction:
            // 스킬 쿨타임 감소
                break;
            case SwordType.StunIncrease:
            // swordSkillAttack.stunDuration += 1f;
                break;
        }
    }

    void Start()
    {
        stat = FindObjectOfType<Player>().GetComponent<Stat>();
        swordSkillAttack = FindObjectOfType<Player>().GetComponent<SwordSkillAttack>();
    }
}

