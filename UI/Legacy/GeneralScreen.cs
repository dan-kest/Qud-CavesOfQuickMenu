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
        private static InputDevice activeDevice;

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
            activeDevice = InputDevice.Keyboard;
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

        private static bool CheckingInput()
        {
            Keys input = Keyboard.getvk(false, false, false);

            string cmd = LegacyKeyMapping.GetCommandFromKey(input);
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
                    BookUI.ShowBook(QudBook.HELP);
                    break;
            }

            if (input == Keys.MouseEvent && Keyboard.CurrentMouseEvent.Event != null)
            {
                string @event = Keyboard.CurrentMouseEvent.Event;

                if (@event == "Command:CmdWait")
                {
                    return SelectDirection(Direction.M);
                }

                if (Options.MouseInput)
                {
                    if (@event == QudKeyword.CLICK_RIGHT)
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
                                if (@event == QudKeyword.CLICK_LEFT)
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
                Thread.Sleep(10);
            };
            int delay = QudOption.NextScreenDelay;
            if (selectedScreenCode != QudScreenCode.None && activeDevice != InputDevice.Mouse && delay > 0)
            {
                Thread.Sleep(delay);
            }
            Erase();
            GameManager.Instance.PopGameView();
            return selectedScreenCode;
        }
    }
}
