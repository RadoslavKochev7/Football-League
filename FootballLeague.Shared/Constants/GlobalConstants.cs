namespace FootballLeague.Shared.Constants
{
    public static class GlobalConstants
    {
        public const string DefaultConnectionString = "DefaultConnection";

        // Team Constants
        public const int TeamNameMaxLength = 100;

        // Match Constants
        public const int ScoredGoalsMinValue = 0;
        public const int ScoredGoalsMaxValue = 100;

        // Gameplay Constants
        public const int WinPoints = 3;
        public const int DrawPoints = 1;
        public const int LossPoints = 0;
    }
}
