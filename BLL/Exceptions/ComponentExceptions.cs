namespace BLL.Exceptions
{
    [Serializable]
    public class ComponentExceptions : Exception
    {
        public ComponentExceptions()
        {
        }

        public ComponentExceptions(string? message) : base(message)
        {
        }

        public ComponentExceptions(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}