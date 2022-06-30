namespace Login_Backend.classes.objects
{
    internal class UserObject
    {
        public string Name { get; set; }
        public string Password { get; set; }

        internal UserObject(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }   
    }
}
