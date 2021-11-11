namespace WebAPI
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public static class Account
        {
            public const string Base = Root + "/account";
            public const string Login = Base + "/login";
            public const string Register = Base + "/register";
        }

        public static class Database
        {
            public const string Base = Root + "/database";
            public const string SendQuery = Base;
        }
        public static class Exercise
        {
            public const string Base = Root + "/exercise";
            public const string Create = Base;
            public const string GetAllCreated = Base + "/getallcreated";
            public const string GetAllPublic = Base + "/getallpublic";
        }
        public static class Group
        {
            public const string Base = Root + "/group";
            public const string Create = Base;
            public const string GetUserGroups = Base + "/getusergroups";
            public const string DeleteGroup = Base + "/deletegroup/{groupId}";
        }
        public static class Invitation
        {
            public const string Base = Root + "/invitation";
            public const string Create = Base + "/{groupId}";
            public const string GetAllReceived = Base + "/getallreceived";
            public const string GetAllSent = Base + "/getallsent";
            public const string GetAll = Base + "/getall";
            public const string Accept = Base + "/accept/{invitationId}";
            public const string Reject = Base + "/reject/{invitationId}";
        }

        public static class Solution
        {
            public const string Base = Root + "/exercise/{exerciseId}/solution";
            public const string Create = Base;
            public const string GetAll = Base + "/getall";
            public const string GetQueryData = Base + "/getquerydata/{solutionId}";
            public const string Compare = Base + "/compare/{solutionId}";
        }
        
    }
}
