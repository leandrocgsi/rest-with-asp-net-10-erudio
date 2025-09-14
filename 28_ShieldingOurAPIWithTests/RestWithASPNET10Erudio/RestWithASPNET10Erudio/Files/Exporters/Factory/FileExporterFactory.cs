using RestWithASPNET10Erudio.Files.Exporters.Contract;
using RestWithASPNET10Erudio.Files.Exporters.Impl;
using RestWithASPNET10Erudio.Files.Importers.Factory;

namespace RestWithASPNET10Erudio.Files.Exporters.Factory
{
    public class FileExporterFactory(
        IServiceProvider serviceProvider,
        ILogger<FileExporterFactory> logger)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<FileExporterFactory> _logger = logger;

        public IFileExporter GetExporter(string acceptHeader)
        {
            if(string.Equals(acceptHeader, MediaTypes.ApplicationXlsx, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected Excel file exporter for media type: {AcceptHeader}", acceptHeader);
                return _serviceProvider.GetService<XlsxExporter>();
            }
            else if(string.Equals(acceptHeader, MediaTypes.ApplicationCsv, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected CSV file exporter for media type: {AcceptHeader}", acceptHeader);
                return _serviceProvider.GetService<CsvExporter>();
            }
            else
            {
                _logger.LogError("Unsupported media type: {AcceptHeader}", acceptHeader);
                throw new NotSupportedException($"The media type of '{acceptHeader}' is not supported.");
            }
        }
    }
}
