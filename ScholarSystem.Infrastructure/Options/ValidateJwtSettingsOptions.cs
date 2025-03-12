using Microsoft.Extensions.Options;

namespace ScholarSystem.Infrastructure.Options
{
    public class ValidateJwtSettingsOptions : IValidateOptions<JwtSettingsOptions>
    {
        public ValidateOptionsResult Validate(string? name, JwtSettingsOptions options)
        {
            if (string.IsNullOrEmpty(options.Secret))
                return ValidateOptionsResult.Fail("JWT Secret не может быть пустым.");

            if (options.AccessTokenExpirationMinutes <= 0)
                return ValidateOptionsResult.Fail("AccessTokenExpirationMinutes должен быть больше 0.");

            if (options.RefreshTokenExpirationDays <= 0)
                return ValidateOptionsResult.Fail("RefreshTokenExpirationDays должен быть больше 0.");

            if (string.IsNullOrEmpty(options.Issuer))
                return ValidateOptionsResult.Fail("JWT Issuer не может быть пустым.");

            if (string.IsNullOrEmpty(options.Audience))
                return ValidateOptionsResult.Fail("JWT Audience не может быть пустым.");

            return ValidateOptionsResult.Success;
        }
    }
}
