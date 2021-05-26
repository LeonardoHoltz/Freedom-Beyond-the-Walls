using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Resources;
using FBTW.Player;
using FBTW.HUD;
using FBTW.Units.Player;
using FBTW.Pause;


public class StableController : MonoBehaviour
{
    private float timer = 0.0f;
    public float waitTime = 5.0f;
    public float cooldownTime = 0.0f;
    public float maxCooldownTime = 1.0f;

    public GameObject stableObject;

    public GameObject horse;

    public Transform parent;
    public GameObject child;

    private float deltaX = 7.0f;
    private float deltaZ = 7.0f;

    Vector3 stablePosition;

    public int horseCost;

    void Start()
    {
        ResourceManagement.setHorse(1);
        stablePosition = stableObject.GetComponent<Transform>().position;

        parent = GameObject.Find("Survey Corps").transform;
    }


    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            timer += Time.deltaTime;
            cooldownTime += Time.deltaTime;

            // food
            if (timer >= waitTime)
            {
                timer = 0.0f;
                ResourceManagement.IncreaseHorse(1);
            }

            // Horse
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // If horse units are unlocked
                if (PlayerManager.instance.CanSpawnHorseUnit())
                {
                    // check the cooldown
                    if (cooldownTime >= maxCooldownTime)
                    {
                        // check if has food enough
                        if (ResourceManagement.getHorse() >= horseCost)
                        {

                            child = Instantiate(horse, new Vector3(stablePosition.x + 1, 0, stablePosition.z + 1), Quaternion.identity);
                            child.transform.SetParent(parent);
                            ResourceManagement.DecreaseFood(horseCost);

                            cooldownTime = 0.0f;

                            HorseUnit hU = child.gameObject.GetComponent<HorseUnit>();

                            while (Physics.CheckSphere(new Vector3(stablePosition.x + deltaX, 1.5f, stablePosition.z + deltaZ), 1.4f))
                            {
                                deltaX += Random.Range(-1f, 1f);
                                deltaZ += Random.Range(-1f, 1f);
                            };

                            hU.MoveUnit(new Vector3(stablePosition.x + deltaX, 0, stablePosition.z + deltaZ));
                            deltaX = 7.0f;
                            deltaZ = 7.0f;
                        }
                    }
                }

            }

            // Set the current amount of food to display on HUD
            //HUD.instance.SetResourceValues(ResourceManagement.getHorse());
        }
    }

}
