namespace CavesOfQuickMenu.Concepts
{
    public enum QudScreenCode
    {
        Skills    = 0,
        Character = 1,
        Inventory = 2,
        Equipment = 3,
        Factions  = 4,
        Quests    = 5,
        Journal   = 6,
        Tinkering = 7,
        None      = -1000,
        Message   = 1000,
        Effects   = 1001,
        Abilities = 1002,
    }

    public enum Direction
    {
        N    = 0,
        NE   = 1,
        E    = 2,
        SE   = 3,
        S    = 4,
        SW   = 5,
        W    = 6,
        NW   = 7,
        M    = 1000,
        None = -1000,
    }

    public enum InputDevice
    {
        Keyboard,
        Mouse,
    }
}
