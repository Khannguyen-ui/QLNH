namespace GUI_QLNH
{
    public static class AppSession
    {
        public static string CurrentUser
        {
            get => AppUser?.CurrentUser;
            set { if (AppUser != null) AppUser.CurrentUser = value; }
        }

        public static string CurrentRole
        {
            get => AppUser?.CurrentRole;
            set { if (AppUser != null) AppUser.CurrentRole = value; }
        }

        public static string CurrentMaNV
        {
            get => AppUser?.CurrentMaNV;
            set { if (AppUser != null) AppUser.CurrentMaNV = value; }
        }

        // Nếu bạn không có lớp AppUser, tạm dùng backing fields:
        private static InternalAppUser _fallback = new InternalAppUser();
        private static InternalAppUser AppUser => _fallback;

        private class InternalAppUser
        {
            public string CurrentUser { get; set; }
            public string CurrentRole { get; set; }
            public string CurrentMaNV { get; set; }
        }
    }
}
