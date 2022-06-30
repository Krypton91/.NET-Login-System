using System;

namespace LoginApplication.classes.objects
{
    [Serializable]
    internal class LoginPayload
    {
        public string RequestType = "LoginRequest";
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
