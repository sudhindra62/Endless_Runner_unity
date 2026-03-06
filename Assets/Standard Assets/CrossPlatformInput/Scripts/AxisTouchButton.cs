using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // designed to work in a pair with another axis touch button
        // (typically with one having -1 and one having 1 axis values)
        public string axisName = "Horizontal"; // The name of the axis
        public float axisValue = 1; // The axis that the value has
        public float responseSpeed = 3; // The speed at which the axis touch button responds
        public float returnToCentreSpeed = 3; // The speed at which the button will return to its centre

        private CrossPlatformInputManager.VirtualAxis m_Axis; // A reference to the virtual axis as it is in the cross platform input
        private bool m_Down;

        void OnEnable()
        {
            if (!CrossPlatformInputManager.AxisExists(axisName))
            {
                // if the axis doesn't exist create a new one in our manager
                m_Axis = new CrossPlatformInputManager.VirtualAxis(axisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
            }
            else
            {
                m_Axis = CrossPlatformInputManager.VirtualAxisReference(axisName);
            }
            FindPairedButton();
        }

        void FindPairedButton()
        {
            // find the other button witch which this button should be paired
            // (the one with the opposite axis value)
            var otherAxisButtons = FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];

            if (otherAxisButtons != null)
            {
                for (int i = 0; i < otherAxisButtons.Length; i++)
                {
                    if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i].axisValue != axisValue)
                    {
                        m_PairedWith = otherAxisButtons[i];
                    }
                }
            }
        }

        void OnDisable()
        {
            // The object is disabled so remove it from the cross platform input manager
            m_Axis.Remove();
        }


        public void OnPointerDown(PointerEventData data)
        {
            m_Down = true;
        }


        public void OnPointerUp(PointerEventData data)
        {
            m_Down = false;
        }

        private void Update()
        {

            if (m_Down)
            {
                m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
            }
            else
            {
                m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, responseSpeed * Time.deltaTime));
            }

        }

        private AxisTouchButton m_PairedWith;
    }
}
