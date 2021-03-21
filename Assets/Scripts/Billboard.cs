using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private new Transform camera;
    private GameObject go;

    private void Start()
    {
        go = GameObject.FindGameObjectWithTag("MainCamera");
        camera = go.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
