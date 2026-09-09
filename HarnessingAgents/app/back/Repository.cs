// -----------------------------------------------------------------------------
// Repository.cs
// -----------------------------------------------------------------------------
// Persistence layer. Registrations are stored as tab-separated lines in a plain
// text file (interests.txt) that lives in the process working directory. This is
// a deliberately naive store: no locking, no schema, no rotation.
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using InterestApi.Domain;

namespace InterestApi.Repository
{
    /// <summary>
    /// Appends <see cref="Registration"/> records to a text file on disk.
    /// </summary>
    public class FileRegistrationRepository
    {
        private readonly string _filePath;

        /// <summary>
        /// Resolves the data file path once so every write targets the same file.
        /// </summary>
        public FileRegistrationRepository()
        {
            _filePath = ResolveDataFilePath();
        }

        /// <summary>
        /// Appends one registration to the data file as a single line.
        /// </summary>
        /// <param name="registration">The record to persist. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="registration"/> is null.
        /// </exception>
        public async Task SaveAsync(Registration registration)
        {
            // Fail fast on a null argument rather than writing a malformed line.
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            var line = FormatRecordLine(registration);

            // Append (do not overwrite). AppendAllTextAsync creates the file on
            // first use.
            await System.IO.File.AppendAllTextAsync(_filePath, line);
        }

        private static string ResolveDataFilePath()
        {
            // Resolve against the current working directory rather than the
            // assembly location: in the container that directory is /app, and
            // interests.txt there is bind-mounted back to the host.
            return Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
        }

        private static string FormatRecordLine(Registration registration)
        {
            // One tab-separated line per registration: an ISO-8601 UTC timestamp
            // ("O" round-trips) then the three fields, newline at the end.
            // Column order is fixed - readers split on '\t' by position.
            return $"{DateTime.UtcNow:O}\t{registration.Name}\t{registration.Email}\t{registration.Course}\n";
        }
    }
}
