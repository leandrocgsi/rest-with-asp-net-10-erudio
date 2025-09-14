using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Files.Exporters.Contract;
using RestWithASPNET10Erudio.Files.Importers.Factory;
using System.Globalization;
using System.Text;

namespace RestWithASPNET10Erudio.Files.Exporters.Impl
{
    internal class CsvExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> people)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(
                memoryStream, Encoding.UTF8, leaveOpen:true);

            using var csv = new CsvWriter(writer, 
                new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });

            csv.WriteRecords(people);
            writer.Flush();

            var fileBytes = memoryStream.ToArray();

            return new FileContentResult(fileBytes,
                MediaTypes.ApplicationCsv)
            {
                FileDownloadName = $"people_exported_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            };
        }
    }
}