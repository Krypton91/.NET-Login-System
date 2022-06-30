using System;

namespace Login_Backend.classes.objects
{
    [Serializable]
    public class LoginRequestPayload
    {
        public string RequestType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
