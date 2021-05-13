using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    GameObject sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = GameObject.Find("CameraRig");
        this.transform.SetParent(sceneCamera.transform);
        this.transform.position = sceneCamera.transform.position; ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
