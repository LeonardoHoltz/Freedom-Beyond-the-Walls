using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FBTW.SpawnGenerator
{
    public class MapSpawnGenerator : MonoBehaviour
    {
        public static MapSpawnGenerator instance;

        private int numberOfTrees = 300;
        private int numberOfRocks = 200;

        private float mapSize, mapSizeForCity; // TODO: Get size of map in the future, hardcoded right now

        public List<GameObject> treesPrefabs;
        public List<GameObject> rocksPrefabs;
        public GameObject shiganshina, titan;

        public int numberTitans = 5;

        public float xCitySpawn, zCitySpawn;

        public Transform parent;
        public GameObject treeChild, rockChild;

        // Start is called before the first frame update
        void Start()
        {
            System.Random rndTree = new System.Random();
            System.Random rndRock = new System.Random();
            parent = GameObject.Find("Scenario").transform;

            mapSize = 5 * GameObject.Find("Plane").transform.localScale.x;
            mapSizeForCity = 3.5f * GameObject.Find("Plane").transform.localScale.x;

            float xSpawn, zSpawn;

            // Shiganshina Spawn:

            xCitySpawn = Random.Range(mapSizeForCity * -1, mapSizeForCity);
            zCitySpawn = Random.Range(mapSizeForCity * -1, mapSizeForCity);

            Instantiate(shiganshina, new Vector3(xCitySpawn, 0, zCitySpawn), Quaternion.identity).transform.SetParent(parent);

            // Titans Spawn:

            bool insideCity = false;

            for (int i = 0; i < numberTitans; i++)
            {
                do
                {
                    xSpawn = Random.Range(mapSize * -1, mapSize);
                    zSpawn = Random.Range(mapSize * -1, mapSize);
                    if ((xSpawn > xCitySpawn - 50f && xSpawn < xCitySpawn + 50f) && (zSpawn > zCitySpawn - 50f && zSpawn < zCitySpawn + 50f))
                    {
                        insideCity = true;
                    }

                } while (Physics.CheckSphere(new Vector3(xSpawn, 1f, zSpawn), 0.75f) || insideCity);
                insideCity = false;
                Instantiate(titan, new Vector3(xSpawn, 0, zSpawn), Quaternion.identity).transform.SetParent(GameObject.Find("Titans").transform);
            }

            for (int i = 0; i < numberOfTrees; i++)
            {
                do
                {
                    xSpawn = Random.Range(mapSize * -1, mapSize);
                    zSpawn = Random.Range(mapSize * -1, mapSize);

                } while (Physics.CheckSphere(new Vector3(xSpawn, 1f, zSpawn), 0.75f));

                treeChild = Instantiate(treesPrefabs[rndTree.Next(treesPrefabs.Count)], new Vector3(xSpawn, 0, zSpawn), Quaternion.identity);
                treeChild.layer = 8;
                treeChild.transform.SetParent(parent);
            }

            for (int i = 0; i < numberOfRocks; i++)
            {
                do
                {
                    xSpawn = Random.Range(mapSize * -1, mapSize);
                    zSpawn = Random.Range(mapSize * -1, mapSize);

                } while (Physics.CheckSphere(new Vector3(xSpawn, 1f, zSpawn), 0.75f));

                rockChild = Instantiate(rocksPrefabs[rndRock.Next(rocksPrefabs.Count)], new Vector3(xSpawn, 0, zSpawn), Quaternion.identity);

                rockChild.transform.SetParent(parent);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}