using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnGenerator : MonoBehaviour
{
    private int numberOfTrees = 300;
    private int numberOfRocks = 200;

    private float mapSize; // TODO: Get size of map in the future, hardcoded right now
        
    public List<GameObject> treesPrefabs;
    public List<GameObject> rocksPrefabs;

    public Transform parent;
    public GameObject treeChild, rockChild;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rndTree = new System.Random();
        System.Random rndRock = new System.Random();
        parent = GameObject.Find("Scenario").transform;

        mapSize = 5 * GameObject.Find("Plane").transform.localScale.x;

        float xSpawn, zSpawn;
            
        for(int i = 0; i < numberOfTrees; i++)
        {
            do
            {
                xSpawn = Random.Range(mapSize * -1,  mapSize);
                zSpawn = Random.Range(mapSize * -1,  mapSize);

            } while(Physics.CheckSphere(new Vector3(xSpawn, 1f, zSpawn), 0.75f));

            treeChild = Instantiate(treesPrefabs[rndTree.Next(treesPrefabs.Count)], new Vector3(xSpawn, 0, zSpawn), Quaternion.identity);

            treeChild.transform.SetParent(parent);
        }

        for(int i = 0; i < numberOfRocks; i++)
        {
            do
            {
                xSpawn = Random.Range(mapSize * -1,  mapSize);
                zSpawn = Random.Range(mapSize * -1,  mapSize);

            } while(Physics.CheckSphere(new Vector3(xSpawn, 1f, zSpawn), 0.75f));

            rockChild = Instantiate(rocksPrefabs[rndRock.Next(rocksPrefabs.Count)], new Vector3(xSpawn, 0, zSpawn), Quaternion.identity);

            rockChild.transform.SetParent(parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
