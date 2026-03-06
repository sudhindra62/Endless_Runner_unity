using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
    public static class CrossPlatformInputManager
    {
        public enum ActiveInputMethod
        {
            Hardware,
            Touch
        }


        private static VirtualInput m_VirtualInput = new VirtualInput();

        public static void SwitchActiveInputMethod(ActiveInputMethod activeInput)
        {
            switch (activeInput)
            {
                case ActiveInputMethod.Hardware:
                    m_VirtualInput.SetVirtualMouse(false);
                    break;

                case ActiveInputMethod.Touch:
                    m_VirtualInput.SetVirtualMouse(true);
                    break;
            }
        }

        public static bool AxisExists(string name)
        {
            return m_VirtualInput.AxisExists(name);
        }

        public static bool ButtonExists(string name)
        {
            return m_VirtualInput.ButtonExists(name);
        }

        public static void RegisterVirtualAxis(VirtualAxis axis)
        {
            m_VirtualInput.RegisterVirtualAxis(axis);
        }


        public static void RegisterVirtualButton(VirtualButton button)
        {
            m_VirtualInput.RegisterVirtualButton(button);
        }


        public static void UnRegisterVirtualAxis(string name)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("name", "Need a valid string to unregister virtual axis");
            }
            m_VirtualInput.UnRegisterVirtualAxis(name);
        }


        public static void UnRegisterVirtualButton(string name)
        {
            m_VirtualInput.UnRegisterVirtualButton(name);
        }


        // returns a reference to a named virtual axis if it exists otherwise null
        public static VirtualAxis VirtualAxisReference(string name)
        {
            return m_VirtualInput.VirtualAxisReference(name);
        }


        // returns the platform appropriate axis value of a given axis name
        public static float GetAxis(string name)
        {
            return GetAxis(name, false);
        }


        public static float GetAxisRaw(string name)
        {
            return GetAxis(name, true);
        }


        // private function handles both raw and smoothed values
        private static float GetAxis(string name, bool raw)
        {
            return m_VirtualInput.GetAxis(name, raw);
        }


        // -- Button handling --
        public static bool GetButton(string name)
        {
            return m_VirtualInput.GetButton(name);
        }


        public static bool GetButtonDown(string name)
        {
            return m_VirtualInput.GetButtonDown(name);
        }


        public static bool GetButtonUp(string name)
        {
            return m_VirtualInput.GetButtonUp(name);
        }


        public static void SetButtonDown(string name)
        {
            m_VirtualInput.SetButtonDown(name);
        }


        public static void SetButtonUp(string name)
        {
            m_VirtualInput.SetButtonUp(name);
        }


        public static void SetAxisPositive(string name)
        {
            m_VirtualInput.SetAxisPositive(name);
        }


        public static void SetAxisNegative(string name)
        {
            m_VirtualInput.SetAxisNegative(name);
        }


        public static void SetAxisZero(string name)
        {
            m_VirtualInput.SetAxisZero(name);
        }


        public static void SetAxis(string name, float value)
        {
            m_VirtualInput.SetAxis(name, value);
        }


        public static Vector3 mousePosition
        {
            get { return m_VirtualInput.MousePosition(); }
        }

        public static void SetVirtualMousePositionX(float f)
        {
            m_VirtualInput.SetVirtualMousePositionX(f);
        }
        public static void SetVirtualMousePositionY(float f)
        {
            m_VirtualInput.SetVirtualMousePositionY(f);
        }
        public static void SetVirtualMousePositionZ(float f)
        {
            m_VirtualInput.SetVirtualMousePositionZ(f);
        }


        // virtual axis and button classes - applies to mobile input
        // Can be mapped to touch joysticks, tilt, etc
        public class VirtualAxis
        {
            public string name { get; private set; }
            private float m_Value;
            public bool matchWithInputManager { get; private set; }


            public VirtualAxis(string name)
                : this(name, true)
            {
            }


            public VirtualAxis(string name, bool matchToInputSettings)
            {
                this.name = name;
                matchWithInputManager = matchToInputSettings;
            }


            // removes an axis from the cross platform input manager
            public void Remove()
            {
                UnRegisterVirtualAxis(name);
            }


            // a controller gameobject (such as a joystick) should call this function
            public void Update(float value)
            {
                m_Value = value;
            }


            public float GetValue
            {
                get { return m_Value; }
            }


            public float GetValueRaw
            {
                get { return m_Value; }
            }
        }

        // a controller gameobject (such as a button) should call this function
        public class VirtualButton
        {
            public string name { get; private set; }
            public bool matchWithInputManager { get; private set; }

            private int m_LastPressedFrame = -5;
            private int m_ReleasedFrame = -5;
            private bool m_Pressed;


            public VirtualButton(string name)
                : this(name, true)
            {
            }


            public VirtualButton(string name, bool matchToInputSettings)
            {
                this.name = name;
                matchWithInputManager = matchToInputSettings;
            }


            // A controller gameobject should call this function when the button is pressed
            public void Pressed()
            {
                if (m_Pressed)
                {
                    return;
                }
                m_Pressed = true;
                m_LastPressedFrame = Time.frameCount;
            }


            // A controller gameobject should call this function when the button is released
            public void Released()
            {
                m_Pressed = false;
                m_ReleasedFrame = Time.frameCount;
            }


            // the cross platform input manager uses this function to remove the button
            public void Remove()
            {
                UnRegisterVirtualButton(name);
            }


            // these are the properties read by the cross platform input manager
            public bool GetButton
            {
                get { return m_Pressed; }
            }


            public bool GetButtonDown
            {
                get
                {
                    return m_LastPressedFrame - Time.frameCount == -1;
                }
            }


            public bool GetButtonUp
            {
                get
                {
                    return (m_ReleasedFrame == Time.frameCount - 1);
                }
            }
        }
    }
}
