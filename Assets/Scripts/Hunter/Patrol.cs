using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Patrol : IState
{
    private FSM _fsm;
    private Hunter _hunter;

    public Patrol(FSM fsm, Hunter hunter)
    {
        _fsm = fsm;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        Debug.Log("Entramos a Patrol");
        _hunter.actualWaypoint = _hunter.GetNearestWaypoint(_hunter.waypoints);
    }

    public void OnUpdate()
    {
        Debug.Log("Estamos en Patrol");
        var force = _hunter.Seek(_hunter.waypoints[_hunter.actualWaypoint].position);
        _hunter.AddForce(force);

        if (Vector3.Distance(_hunter.transform.position, _hunter.waypoints[_hunter.actualWaypoint].position) < 0.5f)
        {
            _hunter.actualWaypoint++;
            if (_hunter.actualWaypoint >= _hunter.waypoints.Length)
                _hunter.actualWaypoint = 0;
        }

        if (_hunter.CheckBoidNear(GameManager.instance.boids, GameManager.instance.radiusDetect) && _hunter.energy > 10f)
        {
            _fsm.ChangeState(FSM.HunterStates.Chase);
        }
    }

    public void OnExit()
    {
        Debug.Log("Salimos del Patrol");
    }
}
