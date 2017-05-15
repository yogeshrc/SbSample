namespace MessagePublisher
{
    internal class PresoUser
    {
        public PresoUser()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public object Role { get; set; }
    }

    enum Roles
    {
        Presenter
    }
}