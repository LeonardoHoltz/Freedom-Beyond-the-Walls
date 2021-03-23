using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FBTW.Resources;


namespace FBTW.HUD
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance;
        private const int ICON_WIDTH = 32, ICON_HEIGHT = 32, TEXT_WIDTH = 128, TEXT_HEIGHT = 32, RESOURCE_BAR_HEIGHT = 40;
        public GUISkin m_resourceSkin;
        private int m_foodCount, m_gasCount, m_bladesCount, m_unitCount;
        public Texture2D m_foodTexture, m_unitTexture;
        void Start()
        {
            instance = this;
            m_foodCount = new int();
            m_unitCount = new int();
            //m_gasDisplay = new int();
            //m_bladesDisplay = new int();
        }

        // Update is called once per frame
        private void OnGUI()
        {
            DrawResourceBar();
        }
        private void DrawResourceBar()
        {
            GUI.skin = m_resourceSkin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
            GUI.Box(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");
            int topPos = 4, iconLeft = 4, textLeft = 40;
            DrawResourceFood(iconLeft, textLeft, topPos);
            iconLeft += TEXT_WIDTH;
            textLeft += TEXT_WIDTH;
            DrawUnitCount(iconLeft, textLeft, topPos);
            GUI.EndGroup();
        }
        public void SetResourceValues(int food/*, int gas, int blades*/)
        {
            this.m_foodCount = food;
            //this.m_gasDisplay = gas;
            //this.m_bladesDisplay = blades;
        }
        public void SetUnitCount(int units/*, int gas, int blades*/)
        {
            this.m_unitCount = units;
            //this.m_gasDisplay = gas;
            //this.m_bladesDisplay = blades;
        }
        private void DrawResourceFood(int iconLeft, int textLeft, int topPos)
        {
            string text = m_foodCount.ToString() /*+ "/" + resourceLimits[type].ToString()*/;
            GUI.DrawTexture(new Rect(iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), m_foodTexture);
            GUI.Label(new Rect(textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
        }
        private void DrawUnitCount(int iconLeft, int textLeft, int topPos)
        {
            string text = m_unitCount.ToString() /*+ "/" + resourceLimits[type].ToString()*/;
            GUI.DrawTexture(new Rect(iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), m_unitTexture);
            GUI.Label(new Rect(textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
        }
    }

}