namespace InterestApi.Domain;

public static class RegistrationFormatter
{
    private static readonly char[] UnsafeStorageCharacters = ['\t', '\r', '\n'];

    public static string ToStorageLine(Registration registration)
    {
        return $"{registration.Name.Trim()}\t{registration.Email.Trim()}\t{registration.Course.Trim()}";
    }

    private static bool IsSafeField(string? value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               value.IndexOfAny(UnsafeStorageCharacters) < 0;
    }
}
