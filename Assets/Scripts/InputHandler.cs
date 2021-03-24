using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Player;
using FBTW.Units.Player;

using System.Linq;
using System;
using FBTW.HUD;

namespace FBTW.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        // Singleton: There will be only this instance in the game
        public static InputHandler instance;

        public static RaycastHit hit; // what is hitted by the ray

        private static List<Transform> listSelectedUnits = new List<Transform>();

        private bool isDragging = false;

        private bool showInspectWindow = false;

        private Transform lastUnitSelected;

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
            if (showInspectWindow)
            {
                HUD.HUD.instance.DrawInspectWindow();
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
                            lastUnitSelected = unit;
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
                            MoveSelectedUnits(hit.point);
                            break;
                    }
                }
            }

            // temporary take damage to test health bar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DamageSelectedUnits(1);
            }
            // Show portrait of last selected unit
            if (Input.GetKeyDown(KeyCode.I) && !IsUnitListEmpty())
            {
                showInspectWindow = !showInspectWindow;
            }
            else if (Input.GetKeyDown(KeyCode.I) && IsUnitListEmpty())
            {
                showInspectWindow = false;
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
                    lastUnitSelected = unit;
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
                    lastUnitSelected = unit;
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

        private void MoveSelectedUnits(Vector3 destination)
        {
            
            List<float> ringDistance = new List<float>();
            List<int> ringPosition = new List<int>();

            ringDistance.Add(1.5f);
            ringPosition.Add(5);

            for(int i = 0; i < listSelectedUnits.Count; i++)
            {
                if (i == ringPosition.Sum() + 1)
                {
                    ringPosition.Add((int)Math.Floor(2 * 3.14f * ringPosition.Count));
                    ringDistance.Add(ringDistance[ringDistance.Count - 1] + 1.5f);
                }
            }
            
            List<Vector3> targetPositionList = GetPositionListAround(destination, ringDistance, ringPosition);

            int targetPositionListIndex = 0;

            foreach (Transform unit in listSelectedUnits)
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                pU.MoveUnit(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }

        private List<Vector3> GetPositionListAround(Vector3 startPosition, List<float> ringDistanceArray, List<int> ringPositionCountArray)
        {
            List<Vector3> positionList = new List<Vector3>();
            positionList.Add(startPosition);
            for (int i = 0; i < ringDistanceArray.Count; i++)
            {
                positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
            }

            return positionList;
        }

        private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
        {
            List<Vector3> positionList = new List<Vector3>();
            for (int i = 0; i < positionCount; i++)
            {
                float angle = i * (360 / positionCount);
                Vector3 dir = ApplyRotationToVector(new Vector3(1, 0, 0), angle);
                Vector3 position = startPosition + dir * distance;
                positionList.Add(position);
            }
            return positionList;
        }

        private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, angle, 0) * vec;
        }

        private void DamageSelectedUnits(int damage)
        {
            foreach (Transform unit in listSelectedUnits)
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                pU.TakeDamage(damage);
            }
        }

    }
}


