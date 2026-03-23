

    public static class UnlockTypeExtensions
    {
        public static string GetFormattedName(this UnlockType unlockType)
        {
            switch (unlockType)
            {
                case UnlockType.Purchased:
                    return "Purchased";
                case UnlockType.Earned:
                    return "Earned";
                case UnlockType.Gifted:
                    return "Gifted";
                case UnlockType.Milestone:
                    return "Milestone Reward";
                case UnlockType.Quest:
                    return "Quest Reward";
                default:
                    return "Other";
            }
        }
    }

