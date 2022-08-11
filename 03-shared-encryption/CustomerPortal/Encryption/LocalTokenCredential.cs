using Azure.Core;

namespace CustomerPortal.Encryption;

public class LocalTokenCredential : TokenCredential
{
    private const string AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFt" +
                                       "ZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE4OTAyMzkwMjIsImlzc" +
                                       "yI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEvIn0.bHLeGTRqjJrmIJbErE-1Azs724E5ib" +
                                       "zvrIc-UQL6pws";

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext,
        CancellationToken cancellationToken)
    {
        return new ValueTask<AccessToken>(new AccessToken(
            AccessToken,
            DateTimeOffset.MaxValue));
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new AccessToken(
            AccessToken,
            DateTimeOffset.MaxValue);
    }
}
