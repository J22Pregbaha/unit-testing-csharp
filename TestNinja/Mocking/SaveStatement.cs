using System;
using System.IO;

namespace TestNinja.Mocking
{
    public interface ISaveStatement
    {
        string SaveStatementToPdf(int housekeeperOid, string housekeeperName, DateTime statementDate);
    }

    public class SaveStatement : ISaveStatement
    {
        public string SaveStatementToPdf(int housekeeperOid, string housekeeperName, DateTime statementDate)
        {
            var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

            if (!report.HasData)
                return string.Empty;

            report.CreateDocument();

            var filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Sandpiper Statement {statementDate:yyyy-MM} {housekeeperName}.pdf");

            report.ExportToPdf(filename);

            return filename;
        }
    }
}