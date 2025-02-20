using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class StatReward : MonoBehaviour
{
    private Stat stat;
    private Player player;
    [SerializeField] public enum RewardType { // 보상 타입
        HpIncrease,
        ATKEnforce,
        SpeedUp,
    }

    public RewardType rewardType;
    [SerializeField] private int amount; // 증가량
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ApplyStat();
            Destroy(gameObject);
        }
    }

    void ApplyStat() { // 스텟 강화 
        switch (rewardType) {
            case RewardType.HpIncrease:
                stat.playerHealth += amount;
                break;
            case RewardType.ATKEnforce:
                stat.playerATK += amount;
                break;
            case RewardType.SpeedUp:
                stat.playerSpeed += amount;
                break;
        }
    }

    void Awake() {
        stat = FindObjectOfType<Player>().GetComponent<Stat>();
    }
}
