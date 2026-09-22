using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InterestApi.Domain;

namespace InterestApi.Repository
{
    public class FileRegistrationRepository
    {
        private readonly string _filePath;

        public FileRegistrationRepository(string? filePath = null)
        {
            _filePath = filePath ?? Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
        }

        public async Task<IReadOnlyList<Registration>> GetAllAsync()
        {
            if (!File.Exists(_filePath)) return Array.Empty<Registration>();

            var registrations = new List<Registration>();
            foreach (var line in await File.ReadAllLinesAsync(_filePath))
            {
                var fields = line.Split('\t');
                if (fields.Length < 4) continue;
                if (string.Equals(fields[0], "Timestamp", StringComparison.OrdinalIgnoreCase)) continue;

                registrations.Add(new Registration
                {
                    Name = fields[1],
                    Email = fields[2],
                    Course = fields[3],
                    // Rows written before status support represent registrations that were accepted.
                    Status = fields.Length > 4
                        && Enum.TryParse<RegistrationDecision>(fields[4], ignoreCase: true, out var status)
                        ? status
                        : RegistrationDecision.Accepted
                });
            }

            return registrations;
        }

        public async Task SaveAsync(Registration registration)
        {
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            var line = $"{DateTime.UtcNow:O}\t{registration.Name}\t{registration.Email}\t{registration.Course}\t{registration.Status}\n";
            await System.IO.File.AppendAllTextAsync(_filePath, line);
        }
    }
}
