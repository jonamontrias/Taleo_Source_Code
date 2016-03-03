using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Data;

namespace TaleoAutomation
{
    static class ExcelReader
    {
        public static List<string[]> Open(string path)
        {
            try
            {
                // Use SpreadSheetDocument class of Open XML SDK to open excel file 
                OpenSettings openSettings = new OpenSettings();
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false, openSettings))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheetcollection = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string relationshipId = sheetcollection.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();

                    List<string[]> dt = new List<string[]>();
                    foreach (Row row in rowcollection)
                    {
                        List<string> conf = new List<string>();
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            string cellValue = cell.CellValue.InnerText;
                            SharedStringTablePart sharedString = spreadsheetDocument.WorkbookPart.SharedStringTablePart;
                            // The condition that the Cell DataType is SharedString 
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                            {
                                conf.Add(sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText);                                
                            }
                            else
                            {
                                conf.Add(cellValue);
                            }
                        }
                        dt.Add(conf.ToArray());
                    }
                    return dt;
                }
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }
    }
}
