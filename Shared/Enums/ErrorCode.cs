namespace TaskManagementSystem.Shared.Enums
{
    public enum ErrorCode
    {
        None = 0,
        ValidationError = 1001,
        UserNotFound = 2000,
        UserAlreadyExists = 2001,
        TaskNotFound = 3000,
        TaskAlreadyExists = 3001,
        TaskAssignmentFailed = 4000,

        Unauthorize = 4001,

        UserHasAssignedTasks = 5000,

        UserEmailDuplicated = 5001,
        GeneralApiError = 100000000
    }
}
