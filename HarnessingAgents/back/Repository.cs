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
        // Absolute path to the data file. Resolved once in the constructor so
        // every write targets the same file even if the working directory
        // changes later.
        private readonly string _filePath;

        /// <summary>
        /// Resolves the data file path relative to the current working
        /// directory. In the container this is <c>/app/interests.txt</c>, which
        /// is bind-mounted back to the host.
        /// </summary>
        public FileRegistrationRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
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

            // Build the line: an ISO-8601 UTC timestamp followed by the three
            // fields, separated by tabs and terminated with a newline. The
            // "O" format specifier gives a round-trippable timestamp.
            var line = $"{DateTime.UtcNow:O}\t{registration.Name}\t{registration.Email}\t{registration.Course}\n";

            // Append (do not overwrite). AppendAllTextAsync creates the file on
            // first use.
            await System.IO.File.AppendAllTextAsync(_filePath, line);
        }
    }
}
