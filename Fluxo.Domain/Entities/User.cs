using Fluxo.Domain.Enums;
using Fluxo.Domain.Exceptions;

namespace Fluxo.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string NormalizedEmail { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public UserRegistrationProvider RegistrationProvider { get; private set; }
    public string? PasswordHash { get; private set; }
    public string? GoogleSubject { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = default!;

    private User() { }

    public static User CreateLocal(Guid id, string email, string displayName, string passwordHash, Guid accountId)
    {
        ValidateBase(id, email, displayName, accountId);

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required for local registration.");

        if (passwordHash.Length > 500)
            throw new DomainException("Password hash must not exceed 500 characters.");

        return new User
        {
            Id = id,
            Email = NormalizeEmailForDisplay(email),
            NormalizedEmail = NormalizeEmail(email),
            DisplayName = displayName.Trim(),
            RegistrationProvider = UserRegistrationProvider.Local,
            PasswordHash = passwordHash,
            AccountId = accountId
        };
    }

    public static User CreateWithGoogle(Guid id, string email, string displayName, string googleSubject, Guid accountId)
    {
        ValidateBase(id, email, displayName, accountId);

        if (string.IsNullOrWhiteSpace(googleSubject))
            throw new DomainException("Google subject is required for Google registration.");

        if (googleSubject.Length > 255)
            throw new DomainException("Google subject must not exceed 255 characters.");

        return new User
        {
            Id = id,
            Email = NormalizeEmailForDisplay(email),
            NormalizedEmail = NormalizeEmail(email),
            DisplayName = displayName.Trim(),
            RegistrationProvider = UserRegistrationProvider.Google,
            GoogleSubject = googleSubject.Trim(),
            AccountId = accountId
        };
    }

    public void UpdateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new DomainException("Display name is required.");

        if (displayName.Length > 100)
            throw new DomainException("Display name must not exceed 100 characters.");

        DisplayName = displayName.Trim();
    }

    private static void ValidateBase(Guid id, string email, string displayName, Guid accountId)
    {
        if (id == Guid.Empty)
            throw new DomainException("User ID is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        if (email.Length > 320)
            throw new DomainException("Email must not exceed 320 characters.");

        if (!email.Contains('@'))
            throw new DomainException("Email is invalid.");

        if (string.IsNullOrWhiteSpace(displayName))
            throw new DomainException("Display name is required.");

        if (displayName.Length > 100)
            throw new DomainException("Display name must not exceed 100 characters.");

        if (accountId == Guid.Empty)
            throw new DomainException("Account ID is required.");
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }

    private static string NormalizeEmailForDisplay(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}
