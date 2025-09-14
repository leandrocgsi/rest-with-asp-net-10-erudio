using RestWithASPNET10Erudio.Files.Importers.Contract;
using RestWithASPNET10Erudio.Files.Importers.Impl;

namespace RestWithASPNET10Erudio.Files.Importers.Factory
{
    public class FileImporterFactory(
        IServiceProvider serviceProvider,
        ILogger<FileImporterFactory> logger)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<FileImporterFactory> _logger = logger;

        public IFileImporter GetImporter(string fileName)
        {
           if(fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
           {
                _logger.LogInformation(
                    "Selected CSV file importer for file: {FileName}", fileName);
                return _serviceProvider.GetRequiredService<CsvImporter>();
           }
           else if(fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
           {
                _logger.LogInformation(
                    "Selected Excel file importer for file: {FileName}", fileName);

                return _serviceProvider.GetRequiredService<XlsxImporter>();
           }
           else
           {
                _logger.LogError("Unsupported file format: {FileName}", fileName);
                throw new NotSupportedException($"The file format of '{fileName}' is not supported.");
           }
        }
    }
}
