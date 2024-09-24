using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float radius = 3f;  
    //public GameObject foodPrefab;

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

                //RespawnFood();
                break;
            }
        }
    }

    /*private void RespawnFood()
    {
        if (foodPrefab == null)
        {
            Debug.LogError("Prefab de comida no asignado.");
            return;
        }

        Vector3 newPosition = new Vector3(Random.Range(-50f, 51f), 0f, Random.Range(-50f, 51f));

        GameObject newFood = Instantiate(foodPrefab, newPosition, Quaternion.identity);

        Food foodScript = newFood.GetComponent<Food>();
        if (foodScript != null)
            {
                foodScript.enabled = true;

                GameManager.instance.listFood.Add(foodScript);
            }
        else
            {
                Debug.LogError("El prefab no tiene el script 'Food'.");
            }

        Debug.Log("Comida respawneada en posición: " + newPosition);
    }*/


    private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}

