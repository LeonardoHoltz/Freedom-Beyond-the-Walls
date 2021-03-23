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

    public GameObject windmillObject;

    public GameObject unit;

    public Transform parent;
    public GameObject child;

    public static RaycastHit hit;
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

        // food
        if(timer >= waitTime)
        {
            timer = 0.0f;
            ResourceManagement.IncreaseFood(1);
            
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(ResourceManagement.getFood() >= unitFoodCost)
            {
                
                child = Instantiate(unit, new Vector3(windmillPosition.x + 5, 0, windmillPosition.z + 5), Quaternion.identity);
                child.transform.SetParent(parent);
                ResourceManagement.DecreaseFood(unitFoodCost);

                PlayerUnit pU = child.gameObject.GetComponent<PlayerUnit>();

                // Create Ray
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(windmillPosition.x + deltaX, 0, windmillPosition.z + deltaZ));

                // Check if we hit something
                while (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "HumanUnit")
                {
                    deltaX += 2.0f;
                    deltaZ += 2.0f;
                    ray = Camera.main.ScreenPointToRay(new Vector3(windmillPosition.x + deltaX, 0, windmillPosition.z + deltaZ));
                };

                pU.MoveUnit(new Vector3(windmillPosition.x + deltaX, 0, windmillPosition.z + deltaZ));
                deltaX = 7.0f;
                deltaZ = 7.0f;
            }
            
        }
        // Set the current amount of food to display on HUD
        HUD.instance.SetResourceValues(ResourceManagement.getFood());
    }
}
