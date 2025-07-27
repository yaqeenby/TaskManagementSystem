namespace TaskManagementSystem.Tasks.Models
{
    public static class TaskItemStatus
    {
        public const string New = "New";
        public const string Pending = "Pending";
        public const string Active = "Active";
        public const string Done = "Done";
        public static readonly string[] All = { New, Pending, Active, Done };
    }
}
