using System;
using System.Threading;
using ConsoleLib.Console;
using CavesOfQuickMenu.Utilities;
using CavesOfQuickMenu.Concepts;
using System.Collections.Generic;
namespace XRL.UI
{
    [UIView(UIScreen.GENERAL, true, false, false, "Adventure", null, false, 0, false)]
    public class GeneralScreen : IWantsTextConsoleInit
    {
        private static TextConsole _textConsole;

        private static ScreenBuffer buffer;
        private static ScreenBuffer bufferOld;

        private static int baseX1, baseY1, baseX2, baseY2;

        private static Direction selectedDirection;
        private static Direction selectedDirectionPrev;
        private static QudScreenCode selectedScreenCode;
        private static InputDevice selectedByDevice;

        public static readonly Dictionary<Direction, QudScreenCode> DirectionToScreenCode = new Dictionary<Direction, QudScreenCode>()
        {
            { Direction.None, QudScreenCode.None },
            { Direction.M, QudScreenCode.Message },
            { Direction.N, QudScreenCode.Skills },
            { Direction.NE, QudScreenCode.Character },
            { Direction.E, QudScreenCode.Inventory },
            { Direction.SE, QudScreenCode.Equipment },
            { Direction.S, QudScreenCode.Factions },
            { Direction.SW, QudScreenCode.Quests },
            { Direction.W, QudScreenCode.Journal },
            { Direction.NW, QudScreenCode.Tinkering },
        };

        public void Init(TextConsole textConsole, ScreenBuffer _)
		{
			_textConsole = textConsole;

            (baseX1, baseY1, baseX2, baseY2) = LegacyCoord.GetBaseCoord();

            ResetSelected();
		}

        private static void ResetSelected()
        {
            selectedDirection = Direction.None;
            selectedDirectionPrev = Direction.None;
            selectedScreenCode = QudScreenCode.None;
            selectedByDevice = InputDevice.Keyboard;
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

        private static bool SelectDirection(Direction direction, bool isConfirm = true)
        {
            if (DirectionToScreenCode.ContainsKey(direction))
            {
                if (selectedDirection != selectedDirectionPrev && selectedDirectionPrev != Direction.None)
                {
                    DrawSelected(selectedDirectionPrev, false);
                }
                selectedDirectionPrev = selectedDirection;
                selectedDirection = direction;
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

        private static bool CheckingInput()
        {
            Keys input = Keyboard.getvk(false, false, false);
            if (input == Keys.MouseEvent && Keyboard.CurrentMouseEvent.Event != null)
            {
                string @event = Keyboard.CurrentMouseEvent.Event;

                // Keyboard & Controller
                string cmd = null;
                if (@event.StartsWith("Command:"))
                {
                    cmd = @event.Split(":")[1];
                }
                switch (cmd)
                {
                    case "CmdSystemMenu":
                    case QudCommand.OPEN_GENERAL:
                    case QudCommand.CLOSE:
                    case "Cancel":
                        return SelectScreen(QudScreenCode.None);
                    case "CmdWait":
                        return SelectDirection(Direction.M);
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
                    case "CmdHelp":
                        BookUI.ShowBook(QudBook.HELP);
                        break;
                }

                // Mouse
                if (Options.MouseInput)
                {
                    if (@event == "RightClick")
                    {
                        return SelectScreen(QudScreenCode.None);
                    }
                    int x = Keyboard.CurrentMouseEvent.x;
                    int y = Keyboard.CurrentMouseEvent.y;
                    bool isHover = false;
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        if (direction != Direction.None)
                        {
                            (int x1, int y1, int x2, int y2) = LegacyCoord.GetSelectedCoord(direction);
                            if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                            {
                                isHover = true;
                                if (@event == "LeftClick")
                                {
                                    selectedByDevice = InputDevice.Mouse;
                                    return SelectDirection(direction, true);
                                }
                                SelectDirection(direction);
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
            return true;
        }

        public static QudScreenCode Show()
        {
            GameManager.Instance.PushGameView(UIScreen.GENERAL);
            TextConsole.LoadScrapBuffers();
            buffer = TextConsole.ScrapBuffer;
            bufferOld = TextConsole.ScrapBuffer2;
            ResetSelected();
            Draw();
            while (CheckingInput());
            int delay = QudOption.NextScreenDelay;
            if (selectedScreenCode != QudScreenCode.None && selectedByDevice != InputDevice.Mouse && delay > 0)
            {
                Thread.Sleep(delay);
            }
            Erase();
            GameManager.Instance.PopGameView();
            return selectedScreenCode;
        }
    }
}
