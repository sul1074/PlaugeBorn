using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgrade : MonoBehaviour // 검사 무기 업그레이드
{   
    private Stat stat;
    private Player player;
    private SwordSkillAttack swordSkillAttack;

    [SerializeField] public enum SwordType { // 업그레이드 검 4종류
        ChargeTimeReducton,
        CooldownReduction,
        StunIncrease,
        maxSkillRange
    }
    public SwordType swordType;
    [SerializeField] private float amount;
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
            swordSkillAttack.maxChargeTime -= amount;
                break;
            case SwordType.CooldownReduction:
            swordSkillAttack.cooldownTime -= amount;
                break;
            case SwordType.StunIncrease:
            // swordSkillAttack.stunDuration += 1f;
                break;
            case SwordType.maxSkillRange:
            swordSkillAttack.maxSkillRange += amount;
                break;
        }
    }

    void Start()
    {
        stat = FindObjectOfType<Player>().GetComponent<Stat>();
        swordSkillAttack = FindObjectOfType<Player>().GetComponent<SwordSkillAttack>();
    }
}

