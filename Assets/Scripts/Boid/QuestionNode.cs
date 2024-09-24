using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class QuestionNode : Node
    {
        public enum TypeQuestion
        {
            FoodNear,
            HerdNear,
            HunterNear
        }

        public Node trueNode;
        public Node falseNode;

        public TypeQuestion type;

        public override void Execute(Boid boid)
        {
            if (type == TypeQuestion.FoodNear)
            {
                Debug.Log("Verificando comida cerca...");
                bool foodNearby = boid.CheckFoodNear(GameManager.instance.listFood, GameManager.instance.radiusDetect);

                if (foodNearby)
                {
                    Debug.Log("Comida cerca, ejecutando trueNode.");
                    trueNode.Execute(boid);
                }
                else
                {
                    Debug.Log("No hay comida cerca, ejecutando falseNode.");
                    falseNode.Execute(boid);
                }
            }
            else if (type == TypeQuestion.HerdNear)
            {
                Debug.Log("Verificando manada cerca...");
                bool herdNearby = boid.CheckHerdNear(GameManager.instance.boids, GameManager.instance.radiusDetect);

                if (herdNearby)
                {
                    Debug.Log("Manada cerca, ejecutando trueNode.");
                    trueNode.Execute(boid);
                }
                else
                {
                    Debug.Log("No hay manada cerca, ejecutando falseNode.");
                    falseNode.Execute(boid);
                }
            }
            else if (type == TypeQuestion.HunterNear)
            {
                Debug.Log("Verificando cazador cerca...");
                bool hunterNearby = boid.CheckHunterNear(GameManager.instance.hunter, GameManager.instance.radiusDetect);

                if (hunterNearby)
                {
                    Debug.Log("Cazador cerca, ejecutando trueNode.");
                    trueNode.Execute(boid);
                }
                else
                {
                    Debug.Log("No hay cazador cerca, ejecutando falseNode.");
                    falseNode.Execute(boid);
                }
            }
        }
    }