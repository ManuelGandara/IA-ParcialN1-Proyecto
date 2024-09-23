using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Boid : MonoBehaviour
{

    [SerializeField] public float velocityMax = 1;
    [SerializeField] public float speedMax = 1;
    public Node nodeInitial;
    public Vector3 velocityActual;

    private void Start()
    {
        GameManager.instance.boids.Add(this);
        velocityActual = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * velocityMax;
        Debug.Log("Velocidad Inicial: " + velocityActual);
    }

    public void Update()
    {
        Flocking();
        Arriving();
        Debug.Log("Velocidad Actual Después de Flocking: " + velocityActual);

        transform.position += velocityActual * Time.deltaTime * 10f;
        Debug.Log("Nueva Posición: " + transform.position);
    }

    public void AddForce(Vector3 directional)
    {
        velocityActual = Vector3.ClampMagnitude(velocityActual + directional, velocityMax);
        Debug.Log("Fuerza Añadida: " + directional + " | Nueva Velocidad: " + velocityActual);
    }



    public bool CheckFoodNear(List<Food> listFood, float radius)
    {
        foreach (Food food in listFood)
        {
            if (Vector3.Distance(transform.position, food.transform.position) < radius)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckHerdNear(List<Boid> listBoids, float radius)
    {
        foreach (Boid boid in listBoids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) < radius)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckHunterNear(Hunter hunter, float radius)
    {
        return Vector3.Distance(transform.position, hunter.transform.position) < radius;
    }

    public void Flocking()
    {
        AddForce(Alignment(GameManager.instance.boids, GameManager.instance.radiusDetect) * GameManager.instance.weightAlignment);
        AddForce(Cohesion(GameManager.instance.boids, GameManager.instance.radiusDetect) * GameManager.instance.weightCohesion);
        AddForce(Separate(GameManager.instance.boids, GameManager.instance.radiusSeparation) * GameManager.instance.weightSeparation);
    }

    #region Flocking code
    Vector3 Separate(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var boid in boids)
        {
            var directional = transform.position - boid.transform.position;

            if (directional.magnitude > radius || boid == this)
                continue;

            desired += directional;
        }
        
        if (desired == Vector3.zero) 
            return desired;
        
        desired.Normalize();
        desired *= velocityMax;

        var steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);

        return steering;
    }

    Vector3 Alignment(List<Boid> boids, float radius)
    {
        var desired = transform.position;
        int count = 0;

        foreach (var boid in boids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) > radius || boid == this)
                continue;

            desired += boid.transform.position;
            count++;
        }

        if (count == 0)
            return Vector3.zero;

        desired /= count;
        desired -= transform.position;

        desired.Normalize();
        desired *= velocityMax;

        var steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);

        return steering;
    }


    Vector3 Cohesion(List<Boid> boids, float radius)
    {
        var desired = transform.position;
        int count = 1;

        foreach (Boid boid in boids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) > radius || boid == this)
            {
                continue;
            }
            desired += boid.transform.position;
            count++;
        }

        if (count == 1)
            return Vector3.zero;

        desired /= count;
        desired -= transform.position;

        desired.Normalize();
        desired *= velocityMax;

        var steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);

        return steering;
    }
    #endregion

    public void Arriving()
    {
        Vector3 nearestFoodPosition = GetNearestFood(GameManager.instance.listFood);
        Debug.Log("Comida más cercana: " + nearestFoodPosition);
        if (nearestFoodPosition != Vector3.zero)
        {
            AddForce(Arrive(nearestFoodPosition));
        }
    }


    public void Evading()
    {
        AddForce(Evade(GameManager.instance.hunter));
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= velocityMax;

        Vector3 steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);

        return steering;
    }

    Vector3 Arrive(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        float dist = desired.magnitude;

        if (dist > GameManager.instance.radiusDetect)
        {
            return Seek(target);
        }
        desired.Normalize();
        desired *= velocityMax * (dist / GameManager.instance.radiusDetect);

        Vector3 steering = desired - velocityActual;
        steering = Vector3.ClampMagnitude(steering, speedMax);

        return steering;
    }



    private Vector3 GetNearestFood(List<Food> listFood)
    {
        Vector3 foodNear = Vector3.positiveInfinity;
        float closestDistance = Mathf.Infinity;

        foreach (Food food in listFood)
        {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                foodNear = food.transform.position;
            }
        }

        return closestDistance < Mathf.Infinity ? foodNear : Vector3.zero;

    }

    public Vector3 Evade(Hunter hunter)
    {
        var posPre = hunter.transform.position + hunter.velocityActual;

        return Flee(posPre);
    }

    Vector3 Flee(Vector3 target)
    {
        return -Seek(target);
    }
    public void Die()
    {
        GameManager.instance.boids.Remove(this);
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance.radiusDetect);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance.radiusSeparation);
        }
    }
}
