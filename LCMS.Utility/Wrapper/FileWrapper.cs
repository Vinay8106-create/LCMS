using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Galaxy.Utility;
using LCMS.Dto;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace LCMS.Utility
{
    public static class FileWrapper
    {
        public static List<string> image_extensions = new List<string> { "JPG", "JPE", "JPEG", "BMP", "GIF", "TIF", "PNG", "JFIF" };
        public static List<string> application_extensions = new List<string> { "PDF" };
        public static List<string> excel_extensions = new List<string> { "XLSX", "XLS" };

        public static string file_type_image = "image/";
        public static string file_type_application = "application/";
        public static string file_type_pdf = "application/pdf";
        public static string file_type_excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string file_type_text = "text/plain";

        public static DataTable ConvertListObjectToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static ExcelExportResponse GenerateExcelFile(ExcelExportRequest request, bool isneedEOR = false)
        {
            ExcelExportResponse response = new ExcelExportResponse();
            response.GeneratedFile = null;

            response.FileName = $"{request.ScreenName}_Search_Results_{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
            string actualFolderPath = GetPlatFormFilePath(request.BasePath, request.FolderPath);

            if (!IsDirectoryExists(actualFolderPath))
                CreateDirectory(actualFolderPath);

            string actualFilePath = GetPlatFormFilePath(actualFolderPath, response.FileName);

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(actualFilePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Apply Styles
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Set Column Width
                SetColumnWidthForExcel(worksheetPart);

                // Add Sheet
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = $"{request.ScreenName} Results" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // **Filter Out Unwanted Columns**
                List<string> validColumns = request.ColumnMappings
                                            .Where(mapping => request.Data.Columns
                                                .Cast<DataColumn>()
                                                .Any(col => string.Equals(col.ColumnName, mapping.Key, StringComparison.InvariantCultureIgnoreCase)))
                                            .Select(mapping => request.Data.Columns
                                                .Cast<DataColumn>()
                                                .First(col => string.Equals(col.ColumnName, mapping.Key, StringComparison.InvariantCultureIgnoreCase))
                                                .ColumnName)
                                            .ToList();

                // **Construct Header Row**
                Row headerRow = new Row();
                foreach (string col in validColumns)
                {
                    if (request.ColumnMappings.TryGetValue(col.ToUpperInvariant(), out string headerName))
                        headerRow.AppendChild(ConstructCell(headerName, CellValues.String, 2));
                }
                if (isneedEOR == true)
                {
                    headerRow.AppendChild(ConstructCell("EOR", CellValues.String, 2));

                }
                sheetData.AppendChild(headerRow);

                // **Construct Data Rows**
                foreach (DataRow dsrow in request.Data.Rows)
                {
                    Row newRow = new Row();
                    foreach (string col in validColumns)
                    {
                        object cellValue = dsrow[col];
                        CellValues cellType = CellValues.String;
                        string cellText = string.Empty;

                        if (cellValue == DBNull.Value)
                        {
                            cellText = "";
                            cellType = CellValues.Number;
                        }
                        else if (cellValue is decimal || cellValue is double || cellValue is float)
                        {
                            cellType = CellValues.Number;
                            //styleIndex = 3; // right aligned numeric style
                            cellText = Convert.ToString(cellValue, CultureInfo.InvariantCulture);
                        }
                        else if (cellValue is DateTime dateTimeValue)
                        {
                            cellText = GetDateTimeConversion(dateTimeValue);
                            cellType = CellValues.String;
                        }
                        else if (cellValue is DateOnly dateOnlyValue)
                        {
                            cellText = dateOnlyValue.ToString("dd MMM yyyy");
                            cellType = CellValues.String;
                        }
                        else if (cellValue is bool isBool)
                        {
                            cellText = isBool ? "Yes" : "No";
                            cellType = CellValues.String;
                        }
                        else
                        {
                            cellText = cellValue.ToString();
                        }

                        newRow.AppendChild(ConstructCell(cellText, cellType, 1));
                    }

                    // Append End of Record
                    if (isneedEOR == true)
                    {
                        newRow.AppendChild(ConstructCell("!", CellValues.String, 1));
                    }
                    sheetData.AppendChild(newRow);
                }


                worksheetPart.Worksheet.Save();
            }

            // Check if File Exists
            if (IsFileExists(actualFilePath))
                response.GeneratedFile = GetFileAsByteArrayFromFilePath(actualFilePath);

            return response;
        }
        public static string GetPlatFormFilePath(string aFilePath, string aFileName = "")
        {
            if (string.IsNullOrEmpty(aFileName))
                return CommonUtil.GetPlatformFilePath(aFilePath);
            else
                return CommonUtil.GetPlatformFilePath(Path.Join(aFilePath, aFileName));
        }
        public static bool IsFileExists(string aActualFilePath)
        {
            return File.Exists(aActualFilePath);

        }
        public static void DeleteFile(string aActualFilePath)
        {
            File.Delete(aActualFilePath);
        }
        public static bool IsDirectoryExists(string aActualFilePath)
        {
            return Directory.Exists(aActualFilePath);
        }
        public static void CreateDirectory(string aActualFilePath)
        {
            Directory.CreateDirectory(aActualFilePath);
        }
        public static byte[] GetFileAsByteArrayFromFilePath(string aActualFilePath)
        {
            return File.ReadAllBytes(aActualFilePath);
        }
        public static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;


            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 11 },
                    new FontName() { Val = "Calibri" }
                ),
                new Font( // Index 1 - header.
                    new FontSize() { Val = 11 },
                     new FontName() { Val = "Calibri" },
                    new Bold(),
                    new Color() { Rgb = "000000" }
                ),
                new Font( // Index 2 - header.
                    new FontSize() { Val = 11 },
                     new FontName() { Val = "Calibri" },
                   new Bold()
                ));
            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None, ForegroundColor = new ForegroundColor() { Rgb = "F8F8FF" }, BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U } }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 1 - default
                    new Fill(new PatternFill()
                    {
                        PatternType = PatternValues.Solid,
                        ForegroundColor = new ForegroundColor() { Rgb = "e89854" },
                        BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U }
                    }) // Index 2 - default
                );
            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()));
            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat(new Alignment() { WrapText = true }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 2, FillId = 2, BorderId = 1, ApplyBorder = true },// header // body
                    new CellFormat { FontId = 2, FillId = 0, BorderId = 1, ApplyBorder = true }, // header // body
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, WrapText = true })
                    {
                        FontId = 2,
                        FillId = 2,
                        BorderId = 1,
                        ApplyBorder = true
                    } // Index 3 – Enhanced Professional Style
                );
            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);
            return styleSheet;
        }
        public static void SetColumnWidthForExcel(WorksheetPart worksheetPart)
        {
            Columns columns = new Columns(
                                    new Column
                                    {
                                        Min = 1,
                                        Max = 50,
                                        Width = 25,
                                        CustomWidth = true
                                    },
                                     new Column
                                     {
                                         Min = 1,
                                         Max = 50,
                                         Width = 25,
                                         CustomWidth = true
                                     });
            worksheetPart.Worksheet.AppendChild(columns);
        }
        public static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
        private static string GetDateTimeConversion(DateTime? aDateTime)
        {
            DateTime minValue = DateTime.MinValue;
            if (aDateTime != null)
            {

                string text = "dd MMM yyyy";

                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(text))
                {
                    list.Add(text);
                }
                aDateTime = ApplicationWrapper.ToLocalTimeZone("India Standard Time", aDateTime.Value);
                list.AddRange(CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns().ToList());

                var Datetime = DateTime.ParseExact(Convert.ToString(aDateTime), list.ToArray(), CultureInfo.CurrentCulture).ToString(text);
                return Datetime;
            }
            return minValue.ToString();
        }
        public static bool WriteFile(string filePath, string fileName, string base64FileContent, out int fileSize)
        {
            fileSize = 0;

            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(base64FileContent))
                return false;


            string fullFilePath = GetPlatFormFilePath(Path.Combine(filePath, fileName));

            // Ensure the directory exists
            if (!IsDirectoryExists(filePath))
                CreateDirectory(filePath);

            // Delete the file if it exists
            if (IsFileExists(fullFilePath))
                DeleteFile(fullFilePath);

            // Extract actual base64 content
            string base64Data = base64FileContent.Contains(",")
                ? base64FileContent.Split(',')[1]
                : base64FileContent;

            byte[] fileBytes = Convert.FromBase64String(base64Data);

            File.WriteAllBytes(fullFilePath, fileBytes);

            fileSize = fileBytes.Length;
            return true;
        }
        public static bool DeleteFileFromDirectory(string relative_path, string file_name)
        {
            bool IsPhotoFileDeleted = true;

            if (!string.IsNullOrEmpty(relative_path) && !string.IsNullOrEmpty(file_name))
            {
                string file_path = GetPlatFormFilePath(relative_path);

                if (IsDirectoryExists(file_path))
                {
                    string actual_file_path = GetPlatFormFilePath(file_path, file_name);
                    if (IsFileExists(actual_file_path))
                        DeleteFile(actual_file_path);
                }
            }

            return IsPhotoFileDeleted;
        }
        public static string GetFileNameWithoutExtension(string file_name)
        {
            return Path.GetFileNameWithoutExtension(file_name);

        }
        public static string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell == null) return string.Empty;

            string value = cell.CellValue?.InnerText ?? string.Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var stringTable = workbookPart.SharedStringTablePart?.SharedStringTable;
                if (int.TryParse(value, out int index) && stringTable != null)
                {
                    value = stringTable.ElementAt(index).InnerText;
                }
            }
            else
            {
                // Check if the value is a date based on style index
                if (cell.StyleIndex != null && workbookPart.WorkbookStylesPart != null)
                {
                    var styles = workbookPart.WorkbookStylesPart.Stylesheet;
                    var cellFormats = styles.CellFormats;
                    var numberingFormats = styles.NumberingFormats;

                    int styleIndex = (int)cell.StyleIndex.Value;
                    var cellFormat = cellFormats.ElementAt(styleIndex) as CellFormat;

                    if (cellFormat?.NumberFormatId != null)
                    {
                        uint numFmtId = cellFormat.NumberFormatId.Value;

                        // Common Excel built-in date format ids: 14–22 and custom > 164
                        if ((numFmtId >= 14 && numFmtId <= 22) || numFmtId >= 164)
                        {
                            if (double.TryParse(value, out double oaDate))
                            {
                                DateTime date = DateTime.FromOADate(oaDate);
                                value = date.ToString("yyyy-MM-dd"); // Customize format here
                            }
                        }
                    }
                }
            }

            return value;
        }
    }
}

