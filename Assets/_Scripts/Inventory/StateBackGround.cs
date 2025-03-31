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

        //플레이어 스텟 초기화 작업 >원래는 여기서 하면 안될거같은데 다른분 코드 건들면 안되니까 여기서 실행 
        playerState.UpdateEquipStats(0, 0, 0, 0);
    }


    private void OnUpdateAllStats(PlayerState playerState)
    {
        PlayerStatePower.text = $"공격력: {playerState.TotalPower.ToString()}"; 
        playerStateDefense.text = $"방어력: {playerState.TotalDefense.ToString()}"; 
        playerStatehealth.text = $"체력: {playerState.TotalHealth.ToString()}";
        playerStateCritical.text = $"체력: {playerState.TotalCriticalChance.ToString()}"; 

    }

}
