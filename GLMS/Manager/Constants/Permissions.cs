namespace GLMS.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }
        public static class Actions
        {
            public const string Create = "Permissions.Actions.Create";
            public const string View = "Permissions.Actions.View";
            public const string Edit = "Permissions.Actions.Edit";
            public const string Delete = "Permissions.Actions.Delete";
        }
    }
}
