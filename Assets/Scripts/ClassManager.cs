namespace Assets.Scripts
{
    public static class ClassManager
    {
        public static float Volume = 1;

        public static int WaitingToLaunch = 0;

        public static MainScript MainScript;
        public static InventoryScript InventoryScript;
        public static CharacterMovementScript CharacterMovementScript;
        public static MiningScript MiningScript;
        public static DrawingScript DrawingScript;
        public static MapReadService MapReadService;
        public static TabScript InventoryPanel;
        public static SoundScript SoundScript;

        public static void Clear()
        {
            WaitingToLaunch = 0;
            MainScript = null;
            InventoryScript = null;
            CharacterMovementScript = null;
            MiningScript = null;
            DrawingScript = null;
            MapReadService = null;
            InventoryPanel = null;
            SoundScript = null;
        }
    }
}
