using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BattlesSystem handles the organisation of rounds, selecting the dancers to dance off from each side.
/// It then hands off to the fightManager to determine the outcome of 2 dancers dance off'ing.
/// 
/// TODO:
///     Needs to hand the request for a dance off battle round by selecting a dancer from each side and 
///         handing off to the fight manager, via GameEvents.RequestFight
///     Needs to handle GameEvents.OnFightComplete so that a new round can start
///     Needs to handle a team winning or loosing
///     This may be where characters are set as selected when they are in a dance off and when they leave the dance off
/// </summary>
public class BattleSystem : MonoBehaviour
{
    public DanceTeam TeamA,TeamB;

    public float battlePrepTime = 2;
    public float fightWinTime = 2;

    private void OnEnable()
    {
        GameEvents.OnRequestFighters += RoundRequested;
        GameEvents.OnFightComplete += FightOver;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestFighters -= RoundRequested;
        GameEvents.OnFightComplete -= FightOver;
    }

    void RoundRequested()
    {
        StartCoroutine(DoRound());
    }

    void FightOver(FightResultData data)
    {
        //check for winner
        if (data.outcome != 0)
        {
            data.winner.myTeam.EnableWinEffects();

            data.defeated.myTeam.RemoveFromActive(data.defeated);
        }

        //if none
        StartCoroutine(HandleFightOver());
    }
    
    IEnumerator DoRound()
    {
        //Tell fight manager to attack? Or just listen for outcomes?
        yield return new WaitForSeconds(battlePrepTime);

        if(TeamA.activeDancers.Count > 0 && TeamB.activeDancers.Count > 0)
        {
            Character a = TeamA.activeDancers[Random.Range(0, TeamA.activeDancers.Count)];
            Character b = TeamB.activeDancers[Random.Range(0, TeamB.activeDancers.Count)];
            GameEvents.RequestFight(new FightEventData(a, b));
        }
        else
        {
            var winner = TeamA.activeDancers.Count > 0 ? TeamA : TeamB;
            GameEvents.BattleFinished(winner);

            winner.EnableWinEffects();

            //log it battlelog also
            Debug.Log("Game Over");
        }
    }

    IEnumerator HandleFightOver()
    {
        yield return new WaitForSeconds(fightWinTime);
        TeamA.DisableWinEffects();
        TeamB.DisableWinEffects();
        GameEvents.RequestFighters();
    }
}
