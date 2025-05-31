using Microsoft.AspNetCore.Authentication;

namespace RottenPotatoes.Authentication
{
    public class CustomTokenAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "CustomToken";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}