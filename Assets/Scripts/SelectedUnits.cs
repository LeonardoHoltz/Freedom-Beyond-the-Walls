using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUnits : MonoBehaviour
{
    private List<Rigidbody> ListSelectedUnits = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ListSelectedUnits.Count);
    }

    public void AddUnitToList(Rigidbody m_Rigidbody)
    {
        ListSelectedUnits.Add(m_Rigidbody);
    }

    public void RemoveUnitFromList(Rigidbody m_Rigidbody)
    {
        ListSelectedUnits.Remove(m_Rigidbody);
    }

    public bool IsUnitInList(Rigidbody m_Rigidbody)
    {
        return ListSelectedUnits.Contains(m_Rigidbody);
    }
}
