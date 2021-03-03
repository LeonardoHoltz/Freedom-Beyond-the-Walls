using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    Renderer m_Renderer;
    Outline outline;

    SelectedUnits unitsScripts;

    /*
    var outline = gameObject.AddComponent<Outline>();

    outline.OutlineMode = Outline.Mode.OutlineAll;
    outline.OutlineColor = Color.blue;
    outline.OutlineWidth = 5f;
    */

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Renderer = GetComponent<Renderer>();
        unitsScripts = GameObject.Find("EventSystem").GetComponent<SelectedUnits>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == gameObject.name)
                {
                    if (!unitsScripts.IsUnitInList(m_Rigidbody))
                    {
                        unitsScripts.AddUnitToList(m_Rigidbody);
                        // Highlight

                        outline = gameObject.AddComponent<Outline>();
                        outline.OutlineMode = Outline.Mode.OutlineAll;
                        outline.OutlineColor = Color.blue;
                        outline.OutlineWidth = 1f;
                    }
                    else
                    {
                        unitsScripts.RemoveUnitFromList(m_Rigidbody);
                        Destroy(GetComponent<Outline>());
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        if (!unitsScripts.IsUnitInList(m_Rigidbody))
                        {
                            unitsScripts.AddUnitToList(m_Rigidbody);

                            // Highlight
                            outline = gameObject.AddComponent<Outline>();
                            outline.OutlineMode = Outline.Mode.OutlineAll;
                            outline.OutlineColor = Color.blue;
                            outline.OutlineWidth = 1f;
                        }
                    }
                    else
                    {
                        if (unitsScripts.IsUnitInList(m_Rigidbody))
                        {
                            unitsScripts.RemoveUnitFromList(m_Rigidbody);
                            Destroy(GetComponent<Outline>());
                        }
                    }
                }
            }
        }
            /*
            unitsScripts.AddUnitToList(m_Rigidbody);
            CameraController.instance.followTransform = transform;
            */

    }
}
