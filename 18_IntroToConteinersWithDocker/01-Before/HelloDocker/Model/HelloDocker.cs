namespace HelloDocker.Model
{
    public class HelloDocker
    {
        public string Content { get; }
        public string Environment { get; }

        public HelloDocker(string content, string environment)
        {
            Content = content;
            Environment = environment;
        }
    }
}