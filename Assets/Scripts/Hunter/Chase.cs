using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    private Hunter _hunter;
    private FSM _fsm;

    public Chase(FSM fsm, Hunter hunter)
    {
        _fsm = fsm;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        Debug.Log("Entramos al estado de Chase");
        InitializeChaseState();
    }

    private void InitializeChaseState()
    {
        _hunter.energy = Mathf.Max(_hunter.energy, 0);
    }

    public void OnUpdate()
    {
        Debug.Log("Ejecutando estado de Chase");

        var nearestBoid = GetNearestBoid();

        ApplyPursuitForce(nearestBoid);

        HandleEnergyAndBoidInteraction(nearestBoid);
    }

    private Boid GetNearestBoid()
    {
        return _hunter.GetNearestBoid(GameManager.instance.boids, GameManager.instance.radiusDetect);
    }

    private void ApplyPursuitForce(Boid nearestBoid)
    {
        if (nearestBoid != null)
        {
            _hunter.AddForce(_hunter.Pursuit(nearestBoid));
        }
    }

    private void HandleEnergyAndBoidInteraction(Boid nearestBoid)
    {
        _hunter.energy -= 2f * Time.deltaTime;

        if (_hunter.energy <= 0)
        {
            _fsm.ChangeState(FSM.HunterStates.Idle);
        }

        if (nearestBoid != null && Vector3.Distance(_hunter.transform.position, nearestBoid.transform.position) < GameManager.instance.radiusSeparation / 2)
        {
            nearestBoid.Die();
        }
    }

    public void OnExit()
    {
        Debug.Log("Saliendo del estado de Chase");
        ResetHunterEnergy();
    }

    private void ResetHunterEnergy()
    {
        _hunter.energy = 0;
    }
}