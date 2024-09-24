using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Boid> boids = new List<Boid>();
    public List<Food> listFood = new List<Food>();
    public Hunter hunter;
    public float radiusDetect;
    public float radiusSeparation;

    [Range(2, 3)]
    public float weightCohesion;
    [Range(2, 3)]
    public float weightAlignment;
    [Range(2, 3)]
    public float weightSeparation;

    private void Awake()
    {
        instance = this;
    }
}