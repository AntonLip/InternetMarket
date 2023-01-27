using Contributors.Interfaces.Repository;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using InternetMarket.Interfaces.IService;
using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InternetMarket.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
            : base(productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetByType(Guid typeId)
        {
            var products = _productRepository.GetWithInclude(p => p.ProductTypeId == typeId);
            return products is null ? throw new ArgumentException() : products;
        }
        public List<Product> Find(string stringFind)
        {
            var productions = _productRepository.Find(stringFind);
            return productions is null ? throw new ArgumentNullException() : productions;

        }




        public FileDto GetReport(UsersProductsViewModel model)
        {
            FileDto fileDto = new FileDto();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SpreadsheetDocument document =
                        SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook, true))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                    FileVersion fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";
                    worksheetPart.Worksheet = new Worksheet(new SheetData());
                    worksheetPart = CreateColumns(worksheetPart);
                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Сводный чек" };
                    sheets.Append(sheet);
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    Row rowFirst = new Row { RowIndex = 1 };
                    sheetData.Append(rowFirst);
                    InsertCell(rowFirst, 4, "Список покупок покупателя", CellValues.String, 5);

                    Row rowSecond = new Row { RowIndex = 2 };
                    sheetData.Append(rowSecond);

                    

                    Row row3 = new Row { RowIndex = 3 };
                    sheetData.Append(row3);

                    InsertCell(row3, 1, " ", CellValues.String, 5);

                    Row row4 = new Row { RowIndex = 4 };
                    sheetData.Append(row4);

                    InsertCell(row4, 1, "№п/п", CellValues.String, 5);
                    InsertCell(row4, 2, "Артикул", CellValues.String, 5);
                    InsertCell(row4, 3, "Название товара", CellValues.String, 5);
                    InsertCell(row4, 4, "Стоимость", CellValues.String, 5);
                    InsertCell(row4, 5, "Количество", CellValues.String, 5);
                    InsertCell(row4, 6, "Дата", CellValues.String, 5);

                    uint idx = 5;
                    int counter = 1;
                    foreach (var pay in model.Products)
                    {                      

                        Row row = new Row { RowIndex = idx };
                        sheetData.Append(row);
                        InsertCell(row, 1, counter.ToString(), CellValues.String, 5);
                        InsertCell(row, 2, pay.ArticleNumber, CellValues.String, 5);
                        InsertCell(row, 3, pay.ProductName, CellValues.String, 5);
                        InsertCell(row, 4, pay.Cost.ToString(), CellValues.String, 5);
                        InsertCell(row, 5, pay.Count.ToString(), CellValues.String, 5);
                        InsertCell(row, 6, pay.BuyDate , CellValues.String, 5);

                        counter++;
                        idx++;
                    }
                    Row rowLast = new Row { RowIndex = idx };
                    sheetData.Append(rowLast);
                    InsertCell(rowLast, 1, " ", CellValues.String, 5);
                    InsertCell(rowLast, 2, " ", CellValues.String, 5);
                    InsertCell(rowLast, 3, "Итого", CellValues.String, 5);
                    InsertCell(rowLast, 4, model.TotalCost.ToString(), CellValues.String, 5);
                }
            fileDto.FileData = memoryStream.ToArray();
            fileDto.FileName = "отчет.xlsx";
            fileDto.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            
            return fileDto;
        }

        static void InsertCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
        {
            Cell refCell = null;
            Cell newCell = new Cell() { CellReference = cell_num.ToString() + ":" + row.RowIndex.ToString() };
            row.InsertBefore(newCell, refCell);

            // Устанавливает тип значения.
            newCell.CellValue = new CellValue(val);
            newCell.DataType = new EnumValue<CellValues>(type);

        }
        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
        public static void UpdateCell(WorksheetPart worksheetPart, string text,
            uint rowIndex, string columnName)
        {

            if (worksheetPart != null)
            {
                Cell cell = GetCell(worksheetPart.Worksheet,
                                         columnName, rowIndex);
                cell.CellValue = new CellValue(text);
                worksheetPart.Worksheet.Save();
            }

        }

        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);
            if (sheets.Count() == 0)
            {
                return null;
            }
            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;
        }

        private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);
            if (row == null)
                return null;
            return row.Elements<Cell>().Where(c => string.Compare
                (c.CellReference.Value, columnName +
                rowIndex, true) == 0).First();

        }
        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            var r = worksheet.GetFirstChild<SheetData>().
             Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            return r;
        }

        private WorksheetPart CreateColumns(WorksheetPart worksheetPart)
        {
            Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            Boolean needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }
            lstColumns.Append(new Column() { Min = 1, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 2, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 3, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 4, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 5, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 6, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 7, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 7, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 7, Max = 10, Width = 20, CustomWidth = true });
            lstColumns.Append(new Column() { Min = 7, Max = 10, Width = 20, CustomWidth = true });
            if (needToInsertColumns)
                worksheetPart.Worksheet.InsertAt(lstColumns, 0);


            return worksheetPart;
        }

      
    }

}
