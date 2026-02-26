// Placeholder for Google Mobile Ads SDK to resolve compile errors.
// This is NOT a functional implementation.
namespace GoogleMobileAds
{
    namespace Api
    {
        public class InitializationStatus 
        {
            // This class is normally part of the Google Mobile Ads SDK.
            // It's empty here because the project code does not use its members.
        }

        public class MobileAds
        {
            public static void Initialize(System.Action<InitializationStatus> initCompleteAction)
            {
                // Invoke the callback immediately with a dummy status.
                // The original code has an empty callback, so the parameter value does not matter.
                if (initCompleteAction != null)
                {
                    initCompleteAction(new InitializationStatus());
                }
            }
        }
    }
}
