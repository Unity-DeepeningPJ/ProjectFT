using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateBackGround : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerStatePower;
    [SerializeField] private TextMeshProUGUI playerStateDefense;
    [SerializeField] private TextMeshProUGUI playerStatehealth;
    [SerializeField] private TextMeshProUGUI playerStateCritical;


    //playerstate을 받아와서 데이터를 넘겨받는다.
    private PlayerState playerState;

    private void Start()
    {
        playerState = GameManager.Instance.PlayerManager.player.PlayerState;

        playerState.OnStatsChanged += OnUpdateAllStats;
    }


    private void OnUpdateAllStats(PlayerState playerState)
    {
        PlayerStatePower.text = playerState.TotalPower.ToString();
        playerStateDefense.text = playerState.TotalDefense.ToString();
        playerStatehealth.text =playerState.TotalHealth.ToString();
        playerStateCritical.text =playerState.TotalCriticalChance.ToString();

    }

}
