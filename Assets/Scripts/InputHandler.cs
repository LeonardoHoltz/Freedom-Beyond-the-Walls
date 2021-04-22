using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using FBTW.Player;
using FBTW.Units.Player;
using FBTW.Units.Titans;

using System.Linq;
using System;
using FBTW.HUD;
using FBTW.Resources;


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

        private Transform lastUnitSelected, m_target = null;

        private Vector3 mousePos;

        private IEnumerator cr1, cr2;

        void Start()
        {
            instance = this;
        }

        private void LateUpdate()
        {
            for(int i = 0; i < listSelectedUnits.Count; i++)
            {
                if(listSelectedUnits[i] == null)
                {
                    listSelectedUnits.RemoveAt(i);
                    i--;
                }
            }
            if(!IsUnitListEmpty())
            {
                lastUnitSelected = listSelectedUnits[listSelectedUnits.Count - 1];
            }
        }

        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(1f, 1f, 1f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.gray);
            }
            if (showInspectWindow && lastUnitSelected != null)
            {
                if(lastUnitSelected.gameObject.name.Contains("connie"))
                {
                    HUD.HUD.instance.DrawInspectWindow(lastUnitSelected, HUD.HUD.UnitType.CONNIE);
                }
                else if (lastUnitSelected.gameObject.name.Contains("sasha"))
                {
                    HUD.HUD.instance.DrawInspectWindow(lastUnitSelected, HUD.HUD.UnitType.SASHA);
                }
                else if(lastUnitSelected.gameObject.name.Contains("armored_titan"))
                {
                    HUD.HUD.instance.DrawInspectWindow(lastUnitSelected, HUD.HUD.UnitType.TITAN);
                }


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
                        case "HumanUnit":
                            // put unit in a list of selected units
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                            break;
                        case "HorseUnit":
                            // put unit in a list of selected units
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                            break;
                        case "TitanUnit":
                            lastUnitSelected = hit.transform;
                            break;
                        default:
                            isDragging = true;

                            // if is not an unit then we can deselect all
                            DeselectUnits();
                            lastUnitSelected = null;
                            showInspectWindow = false;
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
                            // move next to the unit?
                            // probably use the same code when spawning an unit
                            break;
                        case "TitanUnit":
                            // attack on titan
                            m_target = hit.transform;
                            SetSelectedAttacking(true);
                            break;
                        default:
                            // move
                            SetSelectedAttacking(false);
                            MoveSelectedUnits(hit.point);
                            break;
                    }
                }
            }
            // Tester for XP functionality
            if (Input.GetKeyDown(KeyCode.J))
            {
                ResourceManagement.IncreaseXP(50);
            }


            // temporary take damage to test health bar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DamageSelectedUnits(1);
            }

            // Show portrait of last selected unit
            if (Input.GetKeyDown(KeyCode.I) && lastUnitSelected != null)
            {
                if (lastUnitSelected.gameObject.tag == "HumanUnit" || lastUnitSelected.gameObject.tag == "TitanUnit")
                {
                    showInspectWindow = !showInspectWindow;
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

        public void RemoveUnitFromList(Transform unit)
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
            m_Outline.OutlineWidth = 0.75f;
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
                    ringDistance.Add(ringDistance[ringDistance.Count - 1] + 1.6f);
                }
            }
            
            List<Vector3> targetPositionList = GetPositionListAround(destination, ringDistance, ringPosition);

            int targetPositionListIndex = 0;

            foreach (Transform unit in listSelectedUnits)
            {
                if (unit.gameObject.tag == "HumanUnit")
                {
                    PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                    pU.MoveUnit(targetPositionList[targetPositionListIndex]);
                }
                    
                if (unit.gameObject.tag == "HorseUnit")
                {
                    HorseUnit hU = unit.gameObject.GetComponent<HorseUnit>();
                    hU.MoveUnit(targetPositionList[targetPositionListIndex]);
                }
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

        private void SetSelectedAttacking(bool movingToAttack)
        {
            foreach (Transform unit in listSelectedUnits)
            {
                if (unit.gameObject.tag == "HumanUnit")
                {
                    // Set boolean for attacking and the target for each unit selected
                    PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                    pU.setMovingToAttack(movingToAttack);
                }
            }
        }

        public void BeginAttack(Transform unit)
        {
            if(m_target != null)
            {
                // Set boolean for attacking and the target for each unit selected
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                //pU.setAttacking(true);
                // If enemy in range attack
                if (EnemyInRange(m_target, unit))
                {
                    if(!pU.getAttacking())
                    {
                        PerformAttack(m_target, unit);
                    }

                }
                else
                {
                    // Move closer to enemy
                    pU.setAttacking(false);
                    pU.navAgent.speed = 3.5f;
                    ApproachEnemy(m_target, unit);
                }
            }
            else 
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                pU.setMovingToAttack(false);
                pU.navAgent.speed = 3.5f;
            }

        }

        private bool EnemyInRange(Transform target, Transform unit)
        {
            Vector3 targetLocation = target.position;
            Vector3 direction = targetLocation - unit.position;
            // Weapon range should be defined inside player unit later
            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
            if (direction.sqrMagnitude < pU.getAttackRange() * pU.getAttackRange())
            {
                return true;
            }
            return false;
        }

        private void ApproachEnemy(Transform target, Transform unit)
        {
            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
            Vector3 attackPosition = FindNearestAttackPosition(target, unit);
            pU.MoveUnit(attackPosition);
        }

        private Vector3 FindNearestAttackPosition(Transform target, Transform unit)
        {
            // Return closest point of enemy position
            Vector3 targetLocation = target.position;
            Vector3 direction = targetLocation - unit.position;
            float targetDistance = direction.magnitude;
            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
            float distanceToTravel = targetDistance - (0.9f * pU.getAttackRange());
            return Vector3.Lerp(unit.position, targetLocation, distanceToTravel / targetDistance);
        }
        
        private void PerformAttack(Transform target, Transform unit)
        {
            if (target != null)
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                // If enemy got out of range approach
                if (!EnemyInRange(target, unit))
                {
                    ApproachEnemy(target, unit);
                }
                // If not looking at enemy rotate
                else if (!FacingEnemy(target, unit))
                {
                    // Rotate to face enemy
                    RotateToEnemy(target, unit);

                }
                // Attack
                else
                {
                    // Implement attack here
                    Attack(target, unit);
                }
            }
            else
            {
                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                LineController lC = unit.gameObject.GetComponent<LineController>();
                pU.setMovingToAttack(false);
                lC.points[1] = unit.Find("HookPoint");
                lC.lr.enabled = false;
            }

        }
        private bool FacingEnemy(Transform target, Transform unit)
        {
            // Check if facing the enemy
            Vector3 targetLocation = target.position;
            Vector3 direction = targetLocation - unit.position;
            // Set y to 0 we want direction in x and z axis
            direction[1] = 0;
            // Check if unit is facing enemy
            if (direction.normalized == unit.forward.normalized)
            {
                return true;
            }   
            else
            {
                return false;
            }

        }
        private void RotateToEnemy(Transform target, Transform unit)
        {
            // Find a good turn speed i don't know
            int turnSpeed = 5;
            Quaternion aimRotation = Quaternion.LookRotation(target.position - unit.position);
            unit.rotation = Quaternion.RotateTowards(unit.rotation, aimRotation, turnSpeed);
        }
        private void Attack(Transform target, Transform unit)
        {
            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
            LineController lC = unit.gameObject.GetComponent<LineController>();

            // Implement hook in the target here
            lC.points[1] = target;
            lC.lr.enabled = true;

            pU.setAttacking(true);
            pU.navAgent.speed = 10.0f;

            // Return closest point of enemy position
            Vector3 targetLocation = target.position;
            Vector3 direction = targetLocation - unit.position;

            Vector3 dashPosition = targetLocation + 2*direction;
            pU.MoveUnit(dashPosition);
            cr1 = DisableLineRenderer(lC);
            cr2 = ApplyDamage(target, unit);
            StartCoroutine(cr1);
            StartCoroutine(cr2);
        }

        private IEnumerator DisableLineRenderer(LineController lC)
        {
            yield return new WaitForSeconds(1.2f);
            lC.lr.enabled = false;
        }

        private IEnumerator ApplyDamage(Transform target, Transform unit)
        {
            yield return new WaitForSeconds(1.0f);
            TitanUnit tU = target.gameObject.GetComponent<TitanUnit>();
            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
            // for now the damage is just the agility, we gonna fix this to an equation, i think
            tU.TakeDamage(pU.m_agility);
        }

    }
}


