using System.Data;
using ExcelDataReader;

namespace BookingAPI.Tests.DataSource
{
    public class ExcelDataSource
    {
        // Load test data from Excel file
        public static IEnumerable<TestCaseData> GetBookingTestData(string sheetName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string excelFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "BookingTestData.xlsx");

            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    // Get the specified sheet
                    DataTable table = result.Tables[sheetName];

                    if (table == null)
                        throw new ArgumentException($"Sheet '{sheetName}' not found in Excel file");

                    // Loop through rows (skipping header)
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];

                        // Create dynamic test case data based on the sheet columns
                        var testCase = new TestCaseData();

                        // Add all columns as parameters
                        foreach (DataColumn column in table.Columns)
                        {
                            testCase.SetProperty(column.ColumnName, row[column.ColumnName]?.ToString());
                        }

                        // Set test case name using the TestCaseName column if it exists, otherwise use row index
                        string testName = row["TestCaseName"]?.ToString() ?? $"Test Case {i + 1}";
                        testCase.SetName(testName);

                        yield return testCase;
                    }
                }
            }
        }
    }
}