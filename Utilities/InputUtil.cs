using System;
using System.Collections.Generic;
using UnityEngine;
using XRL.UI;
using CavesOfQuickMenu.Concepts;

namespace CavesOfQuickMenu.Utilities
{
    public static class InputUtil
    {
        public static readonly BiDictionary<InputDevice, ControlManager.InputDeviceType> InputToInput = new()
        {
            Data = new Dictionary<InputDevice, ControlManager.InputDeviceType>()
            {
                { InputDevice.Gamepad, ControlManager.InputDeviceType.Gamepad },
                { InputDevice.Keyboard, ControlManager.InputDeviceType.Keyboard },
                { InputDevice.Mouse, ControlManager.InputDeviceType.Mouse },
            }
        };

        public static Direction AxisToDirection(float x, float y)
        {
            const double inputSectorAngle = 360.0f / 8;
            const double inputSectorAngleOffset = inputSectorAngle / 2.0f;

            double angleRadian = Math.Atan2(x, y);
            if (angleRadian < 0.0f)
            {
                angleRadian += Math.PI * 2.0f;
            }
            double angleDegree = 180.0f * angleRadian / Math.PI;
            double inputDegree = angleDegree + inputSectorAngleOffset;
            return (Direction) (((int) Math.Floor(inputDegree / inputSectorAngle)) % 8);
        }

        public static bool IsStickInDeadzone(float axisX, float axisY)
        {
            float threshold = QudOption.DeadzoneThreshold;
            bool hasAxisX = axisX <= threshold && axisX >= -threshold;
            bool hasAxisY = axisY <= threshold && axisY >= -threshold;
            return hasAxisX && hasAxisY;
        }

        public static (float, float) GetMousePosition()
        {
            Vector3 vector = Input.mousePosition;
            return (vector.x, vector.y);
        }

        public static (int, int) GetTilePositionOnMouse()
        {
            RaycastHit[] array = Physics.RaycastAll(GameManager.MainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition));
            foreach (RaycastHit raycastHit in array)
            {
                TileBehavior component = raycastHit.collider.gameObject.GetComponent<TileBehavior>();
                if (component != null)
                {
                    return (component.x, component.y);
                }
            }
            return (-1, -1);
        }

        public static bool IsMouseLeft() { return Input.GetMouseButton(0); }
        public static bool IsMouseRight() { return Input.GetMouseButton(1); }
        public static bool IsMouseMiddle() { return Input.GetMouseButton(2); }
        public static bool IsMouseAny() { return IsMouseLeft() || IsMouseRight() || IsMouseMiddle(); }

        public static (float, float) GetStickPosition(string stickType)
        {
            Vector2 vector = CommandBindingManager.CommandBindings[stickType].ReadValue<Vector2>();
            return (vector.x, vector.y);
        }

        public static Direction GetStickDirection(string stickType)
        {
            if (CommandBindingManager.CommandBindings.ContainsKey(QudKeyword.STICK_DIR))
            {
                (float axisX, float axisY) = GetStickPosition(stickType);
                if (IsStickInDeadzone(axisX, axisY))
                {
                    return Direction.M;
                }
                return AxisToDirection(axisX, axisY);
            }
            return Direction.None;
        }

        public static bool IsAnyInput()
        {
            return Input.anyKey;
        }
    }
}
