using System;
using System.Threading;
using System.Collections.Generic;
using ConsoleLib.Console;
using CavesOfQuickMenu.Utilities;
using CavesOfQuickMenu.Concepts;
namespace XRL.UI
{
    [UIView(UIScreen.GENERAL, true, false, false, "Adventure", null, false, 0, false)]
    public class GeneralScreen : IWantsTextConsoleInit
    {
        private static TextConsole _textConsole;

        private static ScreenBuffer buffer, bufferOld;

        private static int baseX1, baseY1, baseX2, baseY2;

        private static Direction selectedDirection, selectedDirectionPrev;
        private static QudScreenCode selectedScreenCode;
        private static InputDevice activeDevice, activeDevicePrev;
        private static ControlManager.InputDeviceType inputDeviceTypePrev;
        private static float mouseXPrev, mouseYPrev;

        public static readonly Dictionary<Direction, QudScreenCode> DirectionToScreenCode = new()
        {
            { Direction.N, QudScreenCode.Skills },
            { Direction.NE, QudScreenCode.Character },
            { Direction.E, QudScreenCode.Inventory },
            { Direction.SE, QudScreenCode.Equipment },
            { Direction.S, QudScreenCode.Factions },
            { Direction.SW, QudScreenCode.Quests },
            { Direction.W, QudScreenCode.Journal },
            { Direction.NW, QudScreenCode.Tinkering },
            { Direction.M, QudScreenCode.Message },
            { Direction.None, QudScreenCode.None },
        };

        public void Init(TextConsole textConsole, ScreenBuffer _)
		{
			_textConsole = textConsole;

            (baseX1, baseY1, baseX2, baseY2) = LegacyCoord.GetBaseCoord();
		}

        private static void SetAttribute()
        {
            GameManager.ViewInfo viewInfo = GameManager.Instance.GetViewData(UIScreen.GENERAL);
            viewInfo.ForceFullscreen = Options.ModernUI && QudOption.IsForceFullscreen;
        }

        private static void ResetState()
        {
            selectedDirection = selectedDirectionPrev = Direction.None;
            selectedScreenCode = QudScreenCode.None;
            activeDevice = activeDevicePrev = InputDevice.Keyboard;
            inputDeviceTypePrev = ControlManager.InputDeviceType.Unknown;
            (mouseXPrev, mouseYPrev) = InputUtil.GetMousePosition();
        }

        private static void Draw()
        {
            if (buffer != null)
            {
                ConsoleUtil.SuppressScreenBufferImposters(true, baseX1, baseY1, baseX2, baseY2);
                buffer.Fill(baseX1, baseY1, baseX2, baseY2);
                int sliceCount = 1;
                for (int y = baseY1; y <= baseY2; y++)
                {
                    for (int x = baseX1; x <= baseX2; x++)
                    {
                        buffer.SetTileAt(x, y, TextureUtil.GetGeneralLegacy(sliceCount));
                        sliceCount++;
                    }
                }
                _textConsole.DrawBuffer(buffer);
            }
        }

        private static void DrawSelected(Direction direction, bool isSelecting = true)
        {
            if (buffer != null && direction != Direction.None)
            {
                (int x1, int y1, int x2, int y2) = LegacyCoord.GetSelectedCoord(direction);
                int sliceCount = 1;
                for (int y = y1; y <= y2; y++)
                {
                    for (int x = x1; x <= x2; x++)
                    {
                        if (isSelecting)
                        {
                            buffer.SetTileAt(x, y, TextureUtil.GetGeneralLegacySelected(direction, sliceCount));
                            sliceCount++;
                        }
                        else
                        {
                            int no = x - LegacyCoord.OFFSET_X + ((y - LegacyCoord.OFFSET_Y - 1) * LegacyCoord.WIDTH);
                            buffer.SetTileAt(x, y, TextureUtil.GetGeneralLegacy(no));
                        }
                    }
                }
                _textConsole.DrawBuffer(buffer);
            }
        }

        private static void Erase()
        {
            if (buffer != null && bufferOld != null)
            {
                _textConsole.DrawBuffer(bufferOld);
                ConsoleUtil.SuppressScreenBufferImposters(false, baseX1, baseY1, baseX2, baseY2);
            }
        }

        private static bool SelectScreen(QudScreenCode screenCode)
        {
            selectedScreenCode = screenCode;
            return false;
        }

        private static bool SelectDirection(Direction direction, bool isConfirm = true, bool isSelect = true)
        {
            if (DirectionToScreenCode.ContainsKey(direction))
            {
                if (selectedDirection != selectedDirectionPrev && selectedDirectionPrev != Direction.None)
                {
                    DrawSelected(selectedDirectionPrev, false);
                }
                if (isSelect)
                {
                    selectedDirectionPrev = selectedDirection;
                    selectedDirection = direction;
                }
                if (selectedDirection != selectedDirectionPrev && selectedDirection != Direction.None)
                {
                    DrawSelected(selectedDirection);
                }
                if (isConfirm)
                {
                    return SelectScreen(DirectionToScreenCode[direction]);
                }
            }
            return true;
        }

        private static void CheckInputDevice(bool hasMouseEvent, string @event)
        {
            ControlManager.InputDeviceType inputDeviceType = ControlManager.activeControllerType;
            InputDevice activeDeviceTemp = activeDevice;

            (float mouseX, float mouseY) = InputUtil.GetMousePosition();
            if (activeDevice != InputDevice.Mouse)
            {
                if (mouseXPrev != mouseX || mouseYPrev != mouseY || InputUtil.IsMouseAny())
                {
                    activeDevice = InputDevice.Mouse;
                }
            }
            mouseXPrev = mouseX;
            mouseYPrev = mouseY;

            Direction stickDirection = InputUtil.GetStickDirection(QudKeyword.STICK_DIR);
            bool hasStickMovement = stickDirection != Direction.None && stickDirection != Direction.M;
            if (inputDeviceType != inputDeviceTypePrev || hasStickMovement || hasMouseEvent)
            {
                bool isKeyboard = inputDeviceType == ControlManager.InputDeviceType.Keyboard;
                bool isGamepad = inputDeviceType == ControlManager.InputDeviceType.Gamepad;
                if (isKeyboard || isGamepad)
                {
                    activeDevice = InputUtil.InputToInput[inputDeviceType];
                }
                inputDeviceTypePrev = inputDeviceType;
            }
            activeDevicePrev = activeDeviceTemp;
        }

        private static bool CheckingInput()
        {
            Keys input = Keyboard.getvk(false, false, false);
            bool hasMouseEvent = input == Keys.MouseEvent && Keyboard.CurrentMouseEvent.Event != null;
            string @event = hasMouseEvent ? Keyboard.CurrentMouseEvent.Event : "";

            CheckInputDevice(hasMouseEvent, @event);

            // Gamepad
            if (activeDevice == InputDevice.Gamepad)
            {
                Direction stickDirection = InputUtil.GetStickDirection(QudKeyword.STICK_DIR);
                if (stickDirection != Direction.None)
                {
                    if (stickDirection == Direction.M && Keyboard.RawCode == Keys.NumPad5)
                    {
                        return SelectDirection(stickDirection);
                    }
                    SelectDirection(stickDirection, false);
                }
            }
            else if (activeDevicePrev == InputDevice.Gamepad)
            {
                SelectDirection(Direction.None, false);
            }

            if (hasMouseEvent)
            {
                // Keyboard & Gamepad
                string cmd = @event.StartsWith("Command:") ? @event.Split(":")[1] : null;
                switch (cmd)
                {
                    case "CmdMoveN":
                        return SelectDirection(Direction.N);
                    case "CmdMoveNE":
                        return SelectDirection(Direction.NE);
                    case "CmdMoveE":
                        return SelectDirection(Direction.E);
                    case "CmdMoveSE":
                        return SelectDirection(Direction.SE);
                    case "CmdMoveS":
                        return SelectDirection(Direction.S);
                    case "CmdMoveSW":
                        return SelectDirection(Direction.SW);
                    case "CmdMoveW":
                        return SelectDirection(Direction.W);
                    case "CmdMoveNW":
                        return SelectDirection(Direction.NW);
                    case "CmdWait":
                        return SelectDirection(Direction.M);
                    case "CmdSystemMenu":
                    case QudCommand.OPEN_GENERAL:
                    case QudCommand.CLOSE:
                    case "Cancel":
                        return SelectScreen(QudScreenCode.None);
                    case "CmdHelp":
                        BookUI.ShowBookByID(QudBook.HELP);
                        break;
                }
            }

            // Mouse
            if (activeDevice == InputDevice.Mouse && Options.MouseInput)
            {
                if (InputUtil.IsMouseRight())
                {
                    return SelectScreen(QudScreenCode.None);
                }
                if (activeDevicePrev != InputDevice.Mouse)
                {
                    SelectDirection(Direction.None, false);
                }
                (int x, int y) = InputUtil.GetTilePositionOnMouse();
                if (x > -1 && y > -1)
                {
                    bool isHover = false;
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (direction != Direction.None)
                        {
                            (int x1, int y1, int x2, int y2) = LegacyCoord.GetSelectedCoord(direction);
                            if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                            {
                                isHover = true;
                                if (InputUtil.IsMouseLeft())
                                {
                                    activeDevice = InputDevice.Mouse;
                                    return SelectDirection(direction);
                                }
                                SelectDirection(direction, false);
                                break;
                            }
                        }
                    }
                    if (!isHover)
                    {
                        SelectDirection(Direction.None, false);
                    }
                }
            }

            if (activeDevice == InputDevice.Keyboard && activeDevicePrev != InputDevice.Keyboard)
            {
                SelectDirection(Direction.None, false, false);
            }

            return true;
        }

        public static QudScreenCode Show()
        {
            SetAttribute();
            GameManager.Instance.PushGameView(UIScreen.GENERAL);
            TextConsole.LoadScrapBuffers();
            buffer = TextConsole.ScrapBuffer;
            bufferOld = TextConsole.ScrapBuffer2;
            ResetState();
            Draw();
            while (CheckingInput())
            {
                Thread.Sleep(QudOption.InputInterval);
            }
            while (InputUtil.IsAnyInput())
            {
                Thread.Sleep(QudOption.InputInterval);
            }
            Erase();
            GameManager.Instance.PopGameView();
            return selectedScreenCode;
        }
    }
}
