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

        // Team Endpoints
        public const string TeamsTag = "Teams";
        public const string GetTeamByIdSummary = "Get team by Id";
        public const string CreateTeamSummary = "Create new team";
        public const string UpdateTeamSummary = "Update existing team";
        public const string DeleteTeamSummary = "Delete an existing team";
        public const string AlreadyExistingTeamValidation = "A team with the same name already exists.";
        public const string DeleteConflictDetail = "Cannot delete a team that has played matches. You need to first delete the associated matches.";
        public const string DeleteConflictTitle = "Delete Conflict";

        // Match Endpoints
        public const string MatchesTag = "Matches";
        public const string GetAllMatchesSummary = "Get all matches";
        public const string GetMatchByIdSummary = "Get a match by ID";
        public const string CreateMatchSummary = "Create a new match";
        public const string UpdateMatchSummary = "Update an existing match";
        public const string DeleteMatchSummary = "Delete an existing match";

        // Ranking Endpoints
        public const string RankingTag = "Ranking";
        public const string GetRankingSummary = "Gets the current ranking of all teams.";

        // Validators
        public const string TeamNameRequiredMessage = "Team name is required.";
        public const string TeamNameLengthMessage = "Team name must be less than 100 characters.";
        public const string PlayedOnDateRequiredMessage = "Played On date is required.";
        public const string HomeTeamGoalsNegativeMessage = "Home team goals cannot be negative.";
        public const string AwayTeamGoalsNegativeMessage = "Away team goals cannot be negative.";
        public const string ScoredGoalsMaxValueMessage = "Scored goals must be less than 100.";
        public const string PlayedDateInFutureMessage = "Played date cannot be in the future.";
        public const string AwayTeamIdMinValueMessage = "Away team ID must be greater than 0.";
        public const string HomeTeamIdMinValueMessage = "Home team ID must be greater than 0.";
        public const string HomeAndAwayTeamSameMessage = "Home team and away team cannot be the same.";

        // Errors
        public const string UnexpectedErrorMessage = "An unexpected error occurred";
        public const string InternalServerErrorMessage = "Internal Server Error";
    }
}