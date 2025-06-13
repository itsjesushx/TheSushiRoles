namespace TheSushiRoles.Roles
{
    public static class Trickster 
    {
        public static PlayerControl Player;

        public static Color Color = Palette.ImpostorRed;

        public static float lightsOutTimer = 0f;
        public static void ClearAndReload() 
        {
            Player = null;
            lightsOutTimer = 0f;
            JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
        }
    }
}