using System;
using System.IO;
using System.Threading.Tasks;
using InterestApi.Domain;

namespace InterestApi.Repository
{
    public class FileRegistrationRepository 
    {
        private readonly string _filePath;

        public FileRegistrationRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
        }

        public async Task SaveAsync(Registration registration)
        {
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            var line = $"{DateTime.UtcNow:O}\t{registration.Name}\t{registration.Email}\t{registration.Course}\n";
            await System.IO.File.AppendAllTextAsync(_filePath, line);
        }
    }
}
