using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : PanelBase
{
    [Header("플레이어의 스탯")]
    [SerializeField] private TextMeshProUGUI attackPowerValue;
    [SerializeField] private TextMeshProUGUI speedValue;

    private Stat playerStat;

    public Stat PlayerStat
    {
        set { playerStat = value; }
    }

    private void Awake()
    {
        EventManager.OnStatsPressed += TogglePanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }
    protected override void OnShow()
    {
        UpdateStats();
    }
    protected override void OnHide()
    {
        
    }

    private void WriteAttackPowerValueText(float value)
    {
        attackPowerValue.text = value.ToString();
    }

    private void WriteSpeedValueText(float value)
    {
        speedValue.text = value.ToString();
    }

    public void UpdateStats()
    {
        if (playerStat == null)
        {
            Debug.LogError("플레이어 Stat 가져오지 못함.");

            return;
        }

        WriteAttackPowerValueText(playerStat.playerATK);
        WriteSpeedValueText(playerStat.playerSpeed);
    }
}
