namespace Common.Helpers;

public interface IKeyNameProvider
{
    string GetKeyName();
}

public class GuidKeyNameProvider : IKeyNameProvider
{
    public string GetKeyName()
    {
        return Guid.NewGuid().ToString();
    }
}
