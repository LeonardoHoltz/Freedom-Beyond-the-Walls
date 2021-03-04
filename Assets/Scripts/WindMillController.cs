using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Resources;
using FBTW.Player;

public class WindMillController : MonoBehaviour
{
    private float timer = 0.0f;
    private float waitTime = 5.0f;

    public GameObject windmillObject;

    public GameObject unit;

    public Transform parent;
    public GameObject child;

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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= waitTime)
        {
            timer = 0.0f;
            ResourceManagement.IncreaseFood(1);
            Debug.Log("Food: " + ResourceManagement.getFood());
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(ResourceManagement.getFood() >= unitFoodCost)
            {
                
                child = Instantiate(unit, new Vector3(windmillPosition.x + 5, 0, windmillPosition.z + 5), Quaternion.identity);
                child.transform.SetParent(parent);
                ResourceManagement.DecreaseFood(unitFoodCost);
                Debug.Log("Soldier created using 2 foods, foods remaining: " + ResourceManagement.getFood());
            }
            
        }
    }
}
