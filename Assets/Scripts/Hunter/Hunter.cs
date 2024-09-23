using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class Hunter : MonoBehaviour
{
    public Vector3 velocityActual;
    public float velocityMax = 5f, speedMax = 3f, energyMax = 20f, energy;

    public Transform[] waypoints;
    public int actualWaypoint;

    private FSM _fsm;

    private void Start()
    {
        energy = energyMax;
        _fsm = new FSM();
        _fsm.CreateState(FSM.HunterStates.Idle, new Idle(_fsm, this));
        _fsm.CreateState(FSM.HunterStates.Patrol, new Patrol(_fsm, this));
        _fsm.CreateState(FSM.HunterStates.Chase, new Chase(_fsm, this));
        _fsm.ChangeState(FSM.HunterStates.Patrol);

        Debug.Log("Hunter Start: " + transform.position);
    }

    protected void Update()
    {
        _fsm.ArtificialUpdate();

        transform.position += velocityActual * Time.deltaTime;

        if (velocityActual != Vector3.zero)
        {
            transform.forward = velocityActual;
        }

        Debug.Log("Velocity: " + velocityActual);
        Debug.Log("Position: " + transform.position);
    }

    public bool CheckBoidNear(List<Boid> boidList, float radius)
    {
        foreach (Boid boid in boidList)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) < radius)
                return true;
        }
        return false;
    }

    public Boid GetNearestBoid(List<Boid> boidList, float radius)
    {
        Boid nearest = null;
        foreach (var boid in boidList)
        {
            if (nearest == null || Vector3.Distance(transform.position, boid.transform.position) < Vector3.Distance(transform.position, nearest.transform.position))
            {
                nearest = boid;
            }
        }
        return nearest;
    }

    public int GetNearestWaypoint(Transform[] waypoints)
    {
        Vector3 nearest = Vector3.positiveInfinity;
        int index = 0;
        int actualIndex = 0;

        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(waypoint.position, transform.position) < Vector3.Distance(nearest, transform.position))
            {
                nearest = waypoint.transform.position;
                index = actualIndex;
            }
            actualIndex++;
        }

        return index;
    }

    public Vector3 Pursuit(Boid target)
    {
        var posPre = target.transform.position + target.velocityActual;
        return Seek(posPre);
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= velocityMax;  // Velocidad máxima permitida

        Vector3 steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);  // Limita la aceleración

        Debug.Log("Seeking towards: " + target + " with desired velocity: " + steering);

        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        velocityActual = Vector3.ClampMagnitude(velocityActual + dir, velocityMax);

        Debug.Log("Adding force: " + dir + " resulting in velocity: " + velocityActual);
    }

private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance.radiusDetect);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance.radiusSeparation);
        }
    }
}


