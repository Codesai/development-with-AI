// -----------------------------------------------------------------------------
// OperatorCredentials.cs
// -----------------------------------------------------------------------------
// Loads the operator accounts that guard the admin endpoint. Accounts live in
// config/credentials.json as plaintext username/password pairs - naive on
// purpose; this is a training sample, not a real auth system.
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace InterestApi.Security
{
    /// <summary>
    /// Verifies a username/password pair against config/credentials.json.
    /// </summary>
    public class OperatorCredentials
    {
        private readonly Operator[] _operators;

        /// <summary>
        /// Reads and parses the credentials file once, at construction time.
        /// </summary>
        public OperatorCredentials()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "config", "credentials.json");
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var file = JsonSerializer.Deserialize<CredentialsFile>(json, options)
                       ?? throw new InvalidOperationException("credentials.json could not be parsed");
            _operators = file.Operators ?? Array.Empty<Operator>();
        }

        /// <summary>
        /// True when <paramref name="username"/> and <paramref name="password"/>
        /// match one of the configured operator accounts exactly.
        /// </summary>
        public bool Verify(string username, string password)
        {
            return _operators.Any(o =>
                string.Equals(o.Username, username, StringComparison.Ordinal) &&
                string.Equals(o.Password, password, StringComparison.Ordinal));
        }

        private sealed class CredentialsFile
        {
            public Operator[]? Operators { get; set; }
        }

        private sealed class Operator
        {
            public string Username { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
    }
}
