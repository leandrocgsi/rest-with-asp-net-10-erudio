namespace RestWithASPNET10Erudio.Auth.Contract
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}
