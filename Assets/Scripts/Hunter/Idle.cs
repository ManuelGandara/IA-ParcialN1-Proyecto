using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    private Hunter _hunter;
    private FSM _fsm;


    public Idle(FSM fsm, Hunter hunter)
    {
        _fsm = fsm;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        Debug.Log("Entramos en el estado de Idle");
    }

    public void OnUpdate()
    {
        Debug.Log("Ejecutando estado de Idle");

        RegenerateEnergy();
        CheckForNearbyBoids();
        CheckEnergyThreshold();
    }

    private void RegenerateEnergy()
    {
        _hunter.energy += 2f * Time.deltaTime;
    }

    private void CheckForNearbyBoids()
    {
        if (_hunter.CheckBoidNear(GameManager.instance.boids, GameManager.instance.radiusDetect) && _hunter.energy > 10f)
        {
            _fsm.ChangeState(FSM.HunterStates.Chase);
        }
    }

    private void CheckEnergyThreshold()
    {
        if (_hunter.energy >= _hunter.energyMax)
        {
            _fsm.ChangeState(FSM.HunterStates.Patrol);
        }
    }

    public void OnExit()
    {
        Debug.Log("Saliendo del estado de Idle");
    }
}

