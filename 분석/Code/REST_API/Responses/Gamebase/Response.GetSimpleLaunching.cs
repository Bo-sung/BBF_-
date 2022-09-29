namespace BBF.REST_API.Response.Gamebase
{
    public class Get_Simple_Launching : Response_GameBase
    {
        public class LaunchingData
        {
            public Status status;

            public App app;

            public Maintenance maintenance;
        }
    }
}