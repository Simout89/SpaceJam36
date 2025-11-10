namespace Users.FateX.Scripts
{
    public class UserInfo
    {
        public string UserName { get; private set; } = "null";

        public bool firstPlay = true;

        public void SetName(string user)
        {
            UserName = user;
        }
    }
}