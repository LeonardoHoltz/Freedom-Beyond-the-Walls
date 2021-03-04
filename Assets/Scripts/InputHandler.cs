using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Player;
using FBTW.Units.Player;

namespace FBTW.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        // Singleton: There will be only this instance in the game
        public static InputHandler instance;

        public static RaycastHit hit; // what is hitted by the ray

        private static List<Transform> listSelectedUnits = new List<Transform>();

        private bool isDragging = false;

        private Vector3 mousePos;

        void Start()
        {
            instance = this;
        }

        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(1f, 1f, 1f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.gray);
            }
        }

        public void HandleUnitMovement()
        {
            // Click on Units
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition; // For dragging

                // Create Ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Check if we hit something
                if (Physics.Raycast(ray, out hit))
                {
                    // if we do, then do something with that data
                    switch (hit.transform.gameObject.tag)
                    {
                        case "HumanUnit": // Units tag
                            // put unit in a list of selected units
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                            break;
                        default:
                            isDragging = true;

                            // if is not an unit then we can deselect all
                            DeselectUnits();
                            break;
                    }
                }
            }

            // Dragging box selection
            if (Input.GetMouseButtonUp(0))
            {
                foreach (Transform child in Player.PlayerManager.instance.playerUnits)
                {
                    foreach (Transform unit in child)
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                            SelectUnit(unit, true);
                        }
                    }
                }
                isDragging = false;
            }

            // Movement with Mouse1 click
            if (Input.GetMouseButtonDown(1) && !IsUnitListEmpty())
            {
                mousePos = Input.mousePosition; // For dragging

                // Create Ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Check if we hit something
                if (Physics.Raycast(ray, out hit))
                {
                    // if we do, then do something with that data
                    switch (hit.transform.gameObject.tag)
                    {
                        case "HumanUnit":
                            // move to the unit
                            break;
                        case "TitanUnit":
                            // attack on titan
                            break;
                        default:
                            // move
                            MoveSelectedUnits();
                            break;
                    }
                }
            }
        }

        private void SelectUnit(Transform unit, bool isMultiSelection)
        {
            // Left Shift is being pressed
            if (isMultiSelection)
            {
                if (!IsUnitInList(unit))
                {
                    AddUnitToList(unit);
                    HighlightUnit(unit);
                }
                else
                {
                    RemoveUnitFromList(unit);
                    RemoveHighlight(unit);
                }
            }
            else // Just the click
            {
                // Deselect all other selected units
                if(IsUnitInList(unit))
                {
                    DeselectAllOtherUnits(unit);
                }
                else
                {
                    if(!IsUnitListEmpty())
                    {
                        DeselectUnits();
                    }
                    AddUnitToList(unit);
                    HighlightUnit(unit);
                }
                
            }
            
        }

        private void DeselectUnits()
        {
            for(int i = 0; i < listSelectedUnits.Count; i++)
            {
                RemoveHighlight(listSelectedUnits[i]);
            }
            listSelectedUnits.Clear();
        }

        private void DeselectAllOtherUnits(Transform unit) // Removes units but not the one in the parameters
        {
            for (int i = 0; i < listSelectedUnits.Count; i++)
            {
                if (listSelectedUnits[i] != unit)
                {
                    RemoveHighlight(listSelectedUnits[i]);
                }
            }
            listSelectedUnits.Clear();
            AddUnitToList(unit); // compesate the clear
        }

        private void AddUnitToList(Transform unit)
        {
            listSelectedUnits.Add(unit);
        }

        private void RemoveUnitFromList(Transform unit)
        {
            listSelectedUnits.Remove(unit);
        }

        private bool IsUnitInList(Transform unit)
        {
            return listSelectedUnits.Contains(unit);
        }

        private bool IsUnitListEmpty()
        {
            return listSelectedUnits.Count == 0;
        }

        private void HighlightUnit(Transform unit)
        {
            Outline m_Outline; // outline for highlight an unit
            m_Outline = unit.gameObject.AddComponent<Outline>();
            m_Outline.OutlineMode = Outline.Mode.OutlineAll;
            m_Outline.OutlineColor = Color.blue;
            m_Outline.OutlineWidth = 1f;
        }

        private void RemoveHighlight(Transform unit)
        {
            Destroy(unit.gameObject.GetComponent<Outline>());
        }

        private bool isWithinSelectionBounds(Transform tf)
        {
            if (!isDragging)
            {
                return false;
            }

            Camera cam = Camera.main;
            Bounds viewportBounds = MultiSelect.GetViewPointBounds(cam, mousePos, Input.mousePosition);
            return viewportBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }

        private void MoveSelectedUnits()
        {
            foreach (Transform unit in listSelectedUnits)
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                pU.MoveUnit(hit.point);
            }
        }

    }
    public static class hasComponent
    {
        public static bool HasComponent<T>(this GameObject flag) where T : Component
        {
            return flag.GetComponent<T>() != null;
        }
    }
}


