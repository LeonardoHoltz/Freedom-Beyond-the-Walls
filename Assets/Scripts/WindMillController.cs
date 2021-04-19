using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Resources;
using FBTW.Player;
using FBTW.HUD;
using FBTW.Units.Player;

public class WindMillController : MonoBehaviour
{
    private float timer = 0.0f;
    public float waitTime = 5.0f;
    public float cooldownTime = 0.0f;
    public float maxCooldownTime = 1.0f;

    public GameObject windmillObject;

    public List<GameObject> units;

    public Transform parent;
    public GameObject child;

    private float deltaX = 7.0f;
    private float deltaZ = 7.0f;

    Vector3 windmillPosition;

    public int unitFoodCost;
    public int unitGasCost;
    public int unitBladeCost;

    // Start is called before the first frame update
    void Start()
    {
        ResourceManagement.setFood(10);
        windmillPosition = windmillObject.GetComponent<Transform>().position;

        parent = GameObject.Find("Survey Corps").transform;
    }


    void Update()
    {
        timer += Time.deltaTime;
        cooldownTime += Time.deltaTime;

        // food
        if (timer >= waitTime)
        {
            timer = 0.0f;
            ResourceManagement.IncreaseFood(1);
            
        }

        // Connie and Sasha
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // check the cooldown
            if (cooldownTime >= maxCooldownTime)
            {
                // check if has food enough
                if (ResourceManagement.getFood() >= unitFoodCost)
                {
                    System.Random rnd = new System.Random();
                    child = Instantiate(units[rnd.Next(units.Count)], new Vector3(windmillPosition.x + 4, 0, windmillPosition.z + 4), Quaternion.identity);
                    child.transform.SetParent(parent);
                    ResourceManagement.DecreaseFood(unitFoodCost);

                    cooldownTime = 0.0f;

                    PlayerUnit pU = child.gameObject.GetComponent<PlayerUnit>();


                    while (Physics.CheckSphere(new Vector3(windmillPosition.x + deltaX, 1f, windmillPosition.z + deltaZ), 0.75f))
                    {
                        deltaX += Random.Range(-1f, 1f);
                        deltaZ += Random.Range(-1f, 1f);
                    };

                    pU.MoveUnit(new Vector3(windmillPosition.x + deltaX, 0, windmillPosition.z + deltaZ));
                    deltaX = 7.0f;
                    deltaZ = 7.0f;
                }
            }
            
        }

        // Set the current amount of food to display on HUD
        HUD.instance.SetResourceValues(ResourceManagement.getFood());
    }

}
