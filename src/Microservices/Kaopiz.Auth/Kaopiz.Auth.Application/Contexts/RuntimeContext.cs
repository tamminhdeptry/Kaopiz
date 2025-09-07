namespace Kaopiz.Auth.Application
{
    public static class RuntimeContext
    {
        private static AsyncLocal<Guid> _currentUserId { get; set; } = new AsyncLocal<Guid>();
        private static AsyncLocal<string> _currentJti { get; set; } = new AsyncLocal<string>();
        private static AsyncLocal<string> _currentIPAddress { get; set; } = new AsyncLocal<string>();
        private static AsyncLocal<string> _currentAccessToken { get; set; } = new AsyncLocal<string>();

        static RuntimeContext()
        {
        }

        public static Guid CurrentUserId
        {
            get => _currentUserId.Value;
            set => _currentUserId.Value = value;
        }

        public static string CurrentIPAddress
        {
            get => _currentIPAddress.Value ?? string.Empty;
            set => _currentIPAddress.Value = value;
        }

        public static string CurrentJti
        {
            get => _currentJti.Value ?? string.Empty;
            set => _currentJti.Value = value;
        }

        public static string CurrentAccessToken
        {
            get => _currentAccessToken.Value ?? string.Empty;
            set => _currentAccessToken.Value = value;
        }
    }
}