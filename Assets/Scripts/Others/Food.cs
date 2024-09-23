using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float radius = 3f;

    private void Start()
    {
        GameManager.instance.listFood.Add(this);

    }

    private void Update()
    {
        foreach (Boid boid in GameManager.instance.boids)
        {
            if (Vector3.Distance(boid.transform.position, transform.position) < radius)
            {
                GameManager.instance.listFood.Remove(this);
                Destroy(this.gameObject);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}

