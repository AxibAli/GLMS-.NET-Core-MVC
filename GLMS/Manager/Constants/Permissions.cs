namespace GLMS.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionForModule(string module)
        {
            return new List<string>()
            {
                {module},
                {module},
                {module},
                {module},
            };
        }
        public static class Actions
        {
            public const string Create = "Create";
            public const string View = "View";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }
    }
}
