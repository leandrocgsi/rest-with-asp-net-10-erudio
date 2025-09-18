using CsvHelper;
using CsvHelper.Configuration;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Files.Importers.Contract;
using System.Globalization;

namespace RestWithASPNET10Erudio.Files.Importers.Impl
{
    internal class CsvImporter : IFileImporter
    {
        public async Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
            });

            var people = new List<PersonDTO>();

            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                var person = new PersonDTO
                {
                    FirstName = record.first_name,
                    LastName = record.last_name,
                    Address = record.address,
                    Gender = record.gender,
                    Enabled = true
                };
                people.Add(person);
            }
            return people;
        }
    }
}