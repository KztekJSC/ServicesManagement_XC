using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Kztek_Library.Helpers
{
    public class PrintHelper
    {
        public static PrintModel Template_Excel_V1(PrintConfig.HeaderType headertype, string title, DateTime datetime, SessionModel user, string companyname, List<SelectListModel_Print_Column_Header> Data_ColumnHeader, int leftcolumn_tocol = 0, int rightcolumn_fromcol = 0, int footer_fromcol = 0)
        {
            //
            var columnCount = Data_ColumnHeader.Count;

            //One column - header
            var Data_Header = new List<SelectListModel_Print_Header>();
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Ngày tạo: " + datetime.ToString("dd/MM/yyyy HH:mm:ss") });
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Nguời tạo: " + user.Username });
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Đơn vị: " + companyname });

            //One column - header position
            var Data_HeaderPosition = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(1, 1, 1, columnCount));
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(2, 1, 2, columnCount));
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(3, 1, 3, columnCount));

            //Two columns - header
            var Data_Header_Left = new List<SelectListModel_Print_Header>();
            Data_Header_Left.Add(new SelectListModel_Print_Header { ItemText = "Đơn vị: " + companyname });
            Data_Header_Left.Add(new SelectListModel_Print_Header { ItemText = "Ngày: " + datetime.ToString("dd/MM/yyyy HH:mm:ss") });

            var Data_Header_Right = new List<SelectListModel_Print_Header>();
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Cộng Hoà Xã Hội Chủ Nghĩa Việt Nam" });
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Độc Lập - Tự Do - Hạnh Phúc" });
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Số: ...../....." });

            var Data_Headers = new Tuple<List<SelectListModel_Print_Header>, List<SelectListModel_Print_Header>>(Data_Header_Left, Data_Header_Right);

            //Two columns - header positions
            var Data_HeaderPosition_Left = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition_Left.Add(new Tuple<int, int, int, int>(1, 1, 1, leftcolumn_tocol));
            Data_HeaderPosition_Left.Add(new Tuple<int, int, int, int>(2, 1, 2, leftcolumn_tocol));

            var Data_HeaderPosition_Right = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(1, rightcolumn_fromcol, 1, columnCount));
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(2, rightcolumn_fromcol, 2, columnCount));
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(3, rightcolumn_fromcol, 3, columnCount));

            var Data_HeaderPositions = new Tuple<List<Tuple<int, int, int, int>>, List<Tuple<int, int, int, int>>>(Data_HeaderPosition_Left, Data_HeaderPosition_Right);

            var printConfig = new PrintModel()
            {
                Title = title,
                Header_Type = headertype,
                ColumnHeader_Data = Data_ColumnHeader,

                Header_OneCol_Data = Data_Header,
                Header_OneCol_FromRow_FromCol_ToRow_ToCol = Data_HeaderPosition,
                Header_OneCol_Align = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left,

                Header_TwoCol_Data = Data_Headers,
                Header_TwoCol_FromRow_FromCol_ToRow_ToCol = Data_HeaderPositions,
                Header_TwoCol_Align = new Tuple<ExcelHorizontalAlignment, ExcelHorizontalAlignment>(OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center),

                Footer_FromCol = footer_fromcol
            };
            //FromRow_FromCol_ToRow_ToCol
           
       
            return printConfig;
        }

        public static PrintModel Template_Excel_V3(PrintConfig.HeaderType headertype, string title, DateTime datetime, SessionModel user, tblSystemConfig systemConfig, List<SelectListModel_Print_Column_Header> Data_ColumnHeader, int leftcolumn_tocol = 0, int rightcolumn_fromcol = 0, int footer_fromcol = 0)
        {
            //
            var columnCount = Data_ColumnHeader.Count;

            //One column - header
            var Data_Header = new List<SelectListModel_Print_Header>();
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Đơn vị: " + systemConfig.Company + " - " + "Mã thuế: " + systemConfig.Tax });
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Địa chỉ: " + systemConfig.Address });
            Data_Header.Add(new SelectListModel_Print_Header { ItemText = "Ngày: " + datetime.ToString("dd/MM/yyyy HH:mm:ss") });

            //One column - header position
            var Data_HeaderPosition = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(1, 1, 1, columnCount));
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(2, 1, 2, columnCount));
            Data_HeaderPosition.Add(new Tuple<int, int, int, int>(3, 1, 3, columnCount));

            //Two columns - header
            var Data_Header_Left = new List<SelectListModel_Print_Header>();
            Data_Header_Left.Add(new SelectListModel_Print_Header { ItemText = "Đơn vị: " + systemConfig.Company + " - " + "Mã thuế: " + systemConfig.Tax });
            Data_Header_Left.Add(new SelectListModel_Print_Header { ItemText = "Địa chỉ: " + systemConfig.Address });
            Data_Header_Left.Add(new SelectListModel_Print_Header { ItemText = "Ngày: " + datetime.ToString("dd/MM/yyyy HH:mm:ss") });

            var Data_Header_Right = new List<SelectListModel_Print_Header>();
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Cộng Hoà Xã Hội Chủ Nghĩa Việt Nam" });
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Độc Lập - Tự Do - Hạnh Phúc" });
            Data_Header_Right.Add(new SelectListModel_Print_Header { ItemText = "Số: ...../....." });

            var Data_Headers = new Tuple<List<SelectListModel_Print_Header>, List<SelectListModel_Print_Header>>(Data_Header_Left, Data_Header_Right);

            //Two columns - header positions
            var Data_HeaderPosition_Left = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition_Left.Add(new Tuple<int, int, int, int>(1, 1, 1, leftcolumn_tocol));
            Data_HeaderPosition_Left.Add(new Tuple<int, int, int, int>(2, 1, 2, leftcolumn_tocol));
            Data_HeaderPosition_Left.Add(new Tuple<int, int, int, int>(3, 1, 3, leftcolumn_tocol));

            var Data_HeaderPosition_Right = new List<Tuple<int, int, int, int>>();
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(1, rightcolumn_fromcol, 1, columnCount));
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(2, rightcolumn_fromcol, 2, columnCount));
            Data_HeaderPosition_Right.Add(new Tuple<int, int, int, int>(3, rightcolumn_fromcol, 3, columnCount));

            var Data_HeaderPositions = new Tuple<List<Tuple<int, int, int, int>>, List<Tuple<int, int, int, int>>>(Data_HeaderPosition_Left, Data_HeaderPosition_Right);

            var printConfig = new PrintModel()
            {
                Title = title,
                Header_Type = headertype,
                ColumnHeader_Data = Data_ColumnHeader,

                Header_OneCol_Data = Data_Header,
                Header_OneCol_FromRow_FromCol_ToRow_ToCol = Data_HeaderPosition,
                Header_OneCol_Align = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left,

                Header_TwoCol_Data = Data_Headers,
                Header_TwoCol_FromRow_FromCol_ToRow_ToCol = Data_HeaderPositions,
                Header_TwoCol_Align = new Tuple<ExcelHorizontalAlignment, ExcelHorizontalAlignment>(OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center),

                Footer_FromCol = footer_fromcol
            };

            return printConfig;
        }

        public static Task<bool> Excel_Write<T>(HttpContext context, List<T> data, string filename, PrintModel config)
        {
            try
            {
                byte[] dataContent = null;

                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                    //
                    var workSheetFile = package.Workbook.Worksheets[1];

                    //Header
                    Excel_HeaderRender(workSheetFile, config);

                    //Title
                    Excel_Title(workSheetFile, config);

                    //Column header
                    Excel_ColumnHeaderRender(workSheetFile, config);

                    //Data
                    Excel_DataRender<T>(workSheetFile, config, data);

                    //Footer
                    Excel_FooterRender<T>(workSheetFile, config, data);

                    dataContent = package.GetAsByteArray();
                }

                Excel_Execute(context, dataContent, filename);

                return Task.FromResult(true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static Task<bool> Excel_Write(HttpContext context, DataTable data, string filename, PrintModel config)
        {
            try
            {
                byte[] dataContent = null;

                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                    //
                    var workSheetFile = package.Workbook.Worksheets[1];

                    //Header
                    Excel_HeaderRender(workSheetFile, config);

                    //Title
                    Excel_Title(workSheetFile, config);

                    //Column header
                    Excel_ColumnHeaderRender(workSheetFile, config);

                    //Data
                    Excel_DataRender(workSheetFile, config, data);

                    //Footer
                    Excel_FooterRender(workSheetFile, config, data);

                    dataContent = package.GetAsByteArray();
                }

                Excel_Execute(context, dataContent, filename);

                return Task.FromResult(true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static Task<bool> Excel_Write2(HttpContext context, DataTable data, string filename, PrintModel config)
        {
            try
            {
                byte[] dataContent = null;

                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                    //
                    var workSheetFile = package.Workbook.Worksheets[1];

                    //Header
                    Excel_HeaderRender(workSheetFile, config);

                    //Title
                    Excel_Title(workSheetFile, config);

                    //Column header
                    Excel_ColumnHeaderRender2(workSheetFile, config);

                    //Data
                    Excel_DataRender2(workSheetFile, config, data);

                    //Footer
                    Excel_FooterRender2(workSheetFile, config, data);

                    dataContent = package.GetAsByteArray();
                }

                Excel_Execute(context, dataContent, filename);

                return Task.FromResult(true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static Task<bool> Excel_Write_Private(HttpContext context, DataTable data, string filename, PrintModel config)
        {
            try
            {
                byte[] dataContent = null;

                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                    //
                    var workSheetFile = package.Workbook.Worksheets[1];

                    //Header
                    Excel_HeaderRender(workSheetFile, config);

                    //Title
                    Excel_Title(workSheetFile, config);

                    //Column header
                    Excel_ColumnHeaderRender(workSheetFile, config);

                    //Data
                    Excel_DataRender_Private(workSheetFile, config, data);

                    //Footer
                    Excel_FooterRender_Private(workSheetFile, config, data);

                    dataContent = package.GetAsByteArray();
                }

                Excel_Execute(context, dataContent, filename);

                return Task.FromResult(true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private static void Excel_HeaderRender(ExcelWorksheet workSheet, PrintModel config)
        {
            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:


                    break;

                case PrintConfig.HeaderType.OneColumn:

                    foreach (var item in config.Header_OneCol_FromRow_FromCol_ToRow_ToCol)
                    {
                        workSheet.Cells[item.Item1, item.Item2, item.Item3, item.Item4].Merge = true;
                    }

                    workSheet.Cells.AutoFitColumns();
                    workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    for (int i = 0; i < config.Header_OneCol_Data.Count; i++)
                    {
                        workSheet.Cells[i + 1, 1].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[i + 1, 1].Style.Font.Size = 12;
                        workSheet.Cells[i + 1, 1].Value = config.Header_OneCol_Data[i].ItemText;
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                        workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = config.Header_OneCol_Align;
                    }

                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    foreach (var item in config.Header_TwoCol_FromRow_FromCol_ToRow_ToCol.Item1)
                    {
                        workSheet.Cells[item.Item1, item.Item2, item.Item3, item.Item4].Merge = true;
                    }

                    foreach (var item in config.Header_TwoCol_FromRow_FromCol_ToRow_ToCol.Item2)
                    {
                        workSheet.Cells[item.Item1, item.Item2, item.Item3, item.Item4].Merge = true;
                    }

                    workSheet.Cells.AutoFitColumns();
                    workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    for (int i = 0; i < config.Header_TwoCol_Data.Item1.Count; i++)
                    {
                        workSheet.Cells[i + 1, 1].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[i + 1, 1].Style.Font.Size = 12;
                        workSheet.Cells[i + 1, 1].Value = config.Header_TwoCol_Data.Item1[i].ItemText;
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                        workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = config.Header_TwoCol_Align.Item1;
                    }

                    var k = config.Header_TwoCol_FromRow_FromCol_ToRow_ToCol.Item1.Last().Item4;

                    for (int i = 0; i < config.Header_TwoCol_Data.Item2.Count; i++)
                    {
                        workSheet.Cells[i + 1, k + 1].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[i + 1, k + 1].Style.Font.Size = 12;
                        workSheet.Cells[i + 1, k + 1].Value = config.Header_TwoCol_Data.Item2[i].ItemText;
                        workSheet.Cells[i + 1, k + 1].Style.Font.Bold = true;
                        workSheet.Cells[i + 1, k + 1].Style.HorizontalAlignment = config.Header_TwoCol_Align.Item2;
                    }

                    break;

                default:
                    break;
            }

        }

        private static void Excel_Title(ExcelWorksheet workSheet, PrintModel config)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:
                    row = 2;

                    break;

                case PrintConfig.HeaderType.OneColumn:
                    row = config.Header_OneCol_Data.Count + 2;
                    row = row + 2;
                    workSheet.Cells[row, 1, row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    break;

                case PrintConfig.HeaderType.TwoColumns:
                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 2;
                    row = row + 2;
                    workSheet.Cells[row, 1, row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    break;

                default:
                    row = 2;
                    break;
            }

            workSheet.Cells[row - 2, 1, row - 2, 5].Merge = true;
            workSheet.Cells[row - 2, 1, row - 2, 5].Style.Font.Size = 12;
            workSheet.Cells[row - 2, 1, row - 2, 5].Style.Font.Bold = true;
            workSheet.Cells[row - 2, 1, row - 2, 5].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 2, 1, row - 2, 5].Value = config.Time;
            workSheet.Cells[row - 2, 1, row - 2, 5].AutoFitColumns();

            if (!string.IsNullOrEmpty(config.Search))
            {
                workSheet.Cells[row - 2, 6, row - 2, 7].Merge = true;
                workSheet.Cells[row - 2, 6, row - 2, 7].Style.Font.Size = 12;
                workSheet.Cells[row - 2, 6, row - 2, 7].Style.Font.Bold = true;
                workSheet.Cells[row - 2, 6, row - 2, 7].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row - 2, 6, row - 2, 7].Value = config.Search;
                workSheet.Cells[row - 2, 6, row - 2, 7].AutoFitColumns();

            }

            if (!string.IsNullOrEmpty(config.Group))
            {
                workSheet.Cells[row - 1, 1, row - 1, count].Merge = true;
                workSheet.Cells[row - 1, 1, row - 1, count].Style.Font.Size = 12;
                workSheet.Cells[row - 1, 1, row - 1, count].Style.Font.Bold = true;
                workSheet.Cells[row - 1, 1, row - 1, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row - 1, 1, row - 1, count].Value = config.Group;
                workSheet.Cells[row - 1, 1, row - 1, count].AutoFitColumns();

            }

            workSheet.Cells[row, 1, row, count].Merge = true;
            workSheet.Cells[row, 1, row, count].Style.Font.Size = 16;
            workSheet.Cells[row, 1, row, count].Style.Font.Bold = true;
            workSheet.Cells[row, 1, row, count].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row, 1, row, count].Value = config.Title;
            workSheet.Cells[row, 1, row, count].AutoFitColumns();
        }

        private static void Excel_ColumnHeaderRender(ExcelWorksheet workSheet, PrintModel config)
        {
            var count = 0;
            var row = 0;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:
                    row = 4;

                    break;

                case PrintConfig.HeaderType.OneColumn:
                    row = config.Header_OneCol_Data.Count + 4;

                    break;

                case PrintConfig.HeaderType.TwoColumns:
                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 4;

                    break;

                default:
                    row = 4;
                    break;
            }
            row = row + 2;
            //
            foreach (var item in config.ColumnHeader_Data)
            {
                count++;

                workSheet.Cells[row, count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[row, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, count].Style.Font.Size = 14;
                workSheet.Cells[row, count].Value = item.ItemText;
                workSheet.Cells[row, count].Style.Font.Bold = true;
                workSheet.Cells[row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_ColumnHeaderRender2(ExcelWorksheet workSheet, PrintModel config)
        {
            var count = 0;
            var row = 0;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:
                    row = 4;

                    break;

                case PrintConfig.HeaderType.OneColumn:
                    row = config.Header_OneCol_Data.Count + 4;

                    break;

                case PrintConfig.HeaderType.TwoColumns:
                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 4;

                    break;

                default:
                    row = 4;
                    break;
            }

            row = row + 3;

            workSheet.Cells[row - 1, 1, row - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            workSheet.Cells[row - 1, 1, row - 1, 4].Merge = true;
            workSheet.Cells[row - 1, 1, row - 1, 4].Style.Font.Size = 14;
            workSheet.Cells[row - 1, 1, row - 1, 4].Style.Font.Bold = true;
            workSheet.Cells[row - 1, 1, row - 1, 4].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 1, 1, row - 1, 4].Value = "Thông tin chung";
            workSheet.Cells[row - 1, 1, row - 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[row - 1, 1, row - 1, 4].AutoFitColumns();

            workSheet.Cells[row - 1, 5, row - 1, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            workSheet.Cells[row - 1, 5, row - 1, 7].Merge = true;
            workSheet.Cells[row - 1, 5, row - 1, 7].Style.Font.Size = 14;
            workSheet.Cells[row - 1, 5, row - 1, 7].Style.Font.Bold = true;
            workSheet.Cells[row - 1, 5, row - 1, 7].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 1, 5, row - 1, 7].Value = "Sự kiện dưới cổng nhà máy";
            workSheet.Cells[row - 1, 5, row - 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[row - 1, 5, row - 1, 7].AutoFitColumns();

            workSheet.Cells[row - 1, 8, row - 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            workSheet.Cells[row - 1, 8, row - 1, 10].Merge = true;
            workSheet.Cells[row - 1, 8, row - 1, 10].Style.Font.Size = 14;
            workSheet.Cells[row - 1, 8, row - 1, 10].Style.Font.Bold = true;
            workSheet.Cells[row - 1, 8, row - 1, 10].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 1, 8, row - 1, 10].Value = "Sự kiện trên cổng bãi xỉ";
            workSheet.Cells[row - 1, 8, row - 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[row - 1, 8, row - 1, 10].AutoFitColumns();

            //
            foreach (var item in config.ColumnHeader_Data)
            {
                count++;

                workSheet.Cells[row, count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[row, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, count].Style.Font.Size = 14;
                workSheet.Cells[row, count].Value = item.ItemText;
                workSheet.Cells[row, count].Style.Font.Bold = true;
                workSheet.Cells[row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_DataRender<T>(ExcelWorksheet workSheet, PrintModel config, List<T> data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = 5;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + 5;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 5;

                    break;

                default:

                    row = 5;

                    break;
            }
            row = row + 2;
            //FromRow_FromCol_ToRow_ToCol
            if (data.Count != 0)
            {
                workSheet.Cells[row, 1, row + data.Count - 1, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, 1, row + data.Count - 1, count].Style.Font.Size = 12;
            }

            workSheet.Cells[row, 1].LoadFromCollection<T>(data, false);

            //workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_DataRender(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = 5;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + 5;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 5;

                    break;

                default:

                    row = 5;

                    break;
            }
            row = row + 2;
            //FromRow_FromCol_ToRow_ToCol
            if (data.Rows.Count != 0)
            {
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Size = 12;
            }

            workSheet.Cells[row, 1].LoadFromDataTable(data, false);

            //workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_DataRender2(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = 5;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + 5;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 5;

                    break;

                default:

                    row = 5;

                    break;
            }
            row = row + 3;
            //FromRow_FromCol_ToRow_ToCol
            if (data.Rows.Count != 0)
            {
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Size = 12;
            }

            workSheet.Cells[row, 1].LoadFromDataTable(data, false);

            //workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_DataRender_Private(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = 5;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + 5;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + 5;

                    break;

                default:

                    row = 5;

                    break;
            }
            row = row + 2;
            //FromRow_FromCol_ToRow_ToCol
            if (data.Rows.Count != 0)
            {
               
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, 1, row + data.Rows.Count - 1, count].Style.Font.Size = 12;
            }

            for (int i = 0; i < data.Rows.Count - 1; i++)
            {
                DataRow item = data.Rows[i];
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    var _fromRow = row + (i);
                    var _fromCol = j;

                    workSheet.Cells[_fromRow, _fromCol + 1].Value = item[j].ToString();

                }
            }
           // workSheet.Cells[row, 1].LoadFromDataTable(data, false);

            //workSheet.Cells.AutoFitColumns();
        }

        private static void Excel_FooterRender<T>(ExcelWorksheet workSheet, PrintModel config, List<T> data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = data.Count + 6;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + data.Count + 6;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + data.Count + 6;

                    break;

                default:

                    row = data.Count + 6;

                    break;
            }

            //FromRow_FromCol_ToRow_ToCol
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Merge = true;
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Style.Font.Size = 14;
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Value = "Người lập";
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Style.Font.Bold = true;
            workSheet.Cells[row + 1, config.Footer_FromCol, row + 1, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private static void Excel_FooterRender_Private(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = data.Rows.Count + 6;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + data.Rows.Count + 6;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + data.Rows.Count + 6;

                    break;

                default:

                    row = data.Rows.Count + 6;

                    break;
            }
            row = row + 3;
            var objTotal = data.Rows[data.Rows.Count - 1];

            workSheet.Cells[row - 2, 1, row - 2, count - 2].Merge = true;
            workSheet.Cells[row - 2, 1, row - 2, count - 2].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 2, 1, row - 2, count - 2].Style.Font.Size = 14;
            workSheet.Cells[row - 2, 1, row - 2, count - 2].Value = "Tổng số lượt";
            workSheet.Cells[row - 2, 1, row - 2, count - 2].Style.Font.Bold = true;
            workSheet.Cells[row - 2, 1, row - 2, count - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[row - 2, count - 1, row - 2, count - 1].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 2, count - 1, row - 2, count - 1].Style.Font.Size = 14;
            workSheet.Cells[row - 2, count - 1, row - 2, count - 1].Value = objTotal["Turn"];
            workSheet.Cells[row - 2, count - 1, row - 2, count - 1].Style.Font.Bold = true;
            workSheet.Cells[row - 2, count - 1, row - 2, count - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[row - 1, 1, row - 1, count - 2].Merge = true;
            workSheet.Cells[row - 1, 1, row - 1, count - 2].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 1, 1, row - 1, count - 2].Style.Font.Size = 14;
            workSheet.Cells[row - 1, 1, row - 1, count - 2].Value = "Tổng tải trọng";
            workSheet.Cells[row - 1, 1, row - 1, count - 2].Style.Font.Bold = true;
            workSheet.Cells[row - 1, 1, row - 1, count - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[row - 1, count - 1, row - 1, count - 1].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row - 1, count - 1, row - 1, count - 1].Style.Font.Size = 14;
            workSheet.Cells[row - 1, count - 1, row - 1, count - 1].Value = objTotal["Weight"];
            workSheet.Cells[row - 1, count - 1, row - 1, count - 1].Style.Font.Bold = true;
            workSheet.Cells[row - 1, count - 1, row - 1, count - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            workSheet.Cells[row, config.Footer_FromCol, row, count].Merge = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Size = 14;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Value = "Người lập";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Bold = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
        private static void Excel_FooterRender(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = data.Rows.Count + 6;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + data.Rows.Count + 6;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + data.Rows.Count + 6;

                    break;

                default:

                    row = data.Rows.Count + 6;

                    break;
            }
            row = row + 2;
            //FromRow_FromCol_ToRow_ToCol
            workSheet.Cells[row, config.Footer_FromCol, row, count].Merge = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Size = 14;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Value = "Người lập";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Bold = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
        private static void Excel_FooterRender2(ExcelWorksheet workSheet, PrintModel config, DataTable data)
        {
            var row = 0;
            var count = config.ColumnHeader_Data.Count;

            switch (config.Header_Type)
            {
                case PrintConfig.HeaderType.NoHeader:

                    row = data.Rows.Count + 6;
                    break;

                case PrintConfig.HeaderType.OneColumn:

                    row = config.Header_OneCol_Data.Count + data.Rows.Count + 6;


                    break;

                case PrintConfig.HeaderType.TwoColumns:

                    row = Math.Max(config.Header_TwoCol_Data.Item1.Count, config.Header_TwoCol_Data.Item2.Count) + data.Rows.Count + 6;

                    break;

                default:

                    row = data.Rows.Count + 6;

                    break;
            }
            row = row + 3;
            //FromRow_FromCol_ToRow_ToCol
            workSheet.Cells[row, config.Footer_FromCol, row, count].Merge = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Name = "Times New Roman";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Size = 14;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Value = "Người lập";
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.Font.Bold = true;
            workSheet.Cells[row, config.Footer_FromCol, row, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private static void Excel_Execute(HttpContext context, byte[] data, string filename)
        {
            //package.Workbook.Properties.Title = "Attempts";
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;

                httpContext.Response.Headers.Add(
                     "content-disposition",
                      string.Format("attachment; filename={0}.xlsx", filename)
                );

                httpContext.Response.ContentLength = data.Length;
                httpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpContext.Response.Body.Write(data);

                return Task.FromResult(0);
            }, context);
        }

        public static Task<DataTable> ReadFromExcelCardCustomer(string path, ref string errorText)
        {
            try
            {
                // Khởi tạo data table
                var dt = new DataTable();
                // Load file excel và các setting ban đầu
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        // Đọc tất cả các header
                        foreach (var firstRowCell in workSheet.Cells[7, 1, 7, workSheet.Dimension.End.Column])
                        {
                            if (!string.IsNullOrWhiteSpace(firstRowCell.Text))
                            {
                                dt.Columns.Add(firstRowCell.Text.Trim());
                            }
                        }
                        // Đọc tất cả data bắt đầu từ row thứ n
                        for (var rowNumber = 8; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                        {
                            var rowCell1 = workSheet.Cells[rowNumber, 1];

                            if (!string.IsNullOrWhiteSpace((rowCell1.Text)))
                            {
                                // Lấy 1 row trong excel để truy vấn
                                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                // tạo 1 row trong data table
                                var newRow = dt.NewRow();
                                foreach (var cell in row)
                                {
                                    //if (!string.IsNullOrWhiteSpace(cell.Text))
                                    //{
                                    //    newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                    //}
                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                    {
                                        if (cell.Address.Contains("E"))
                                        {
                                            if (cell.Text.Length < 8)
                                            {
                                                var a = Convert.ToDateTime(cell.Value).Date;
                                                newRow[cell.Start.Column - 1] = a.Year + "/" + a.Day + "/" + a.Month;
                                            }
                                            else
                                            {
                                                DateTime d = DateTime.ParseExact(!string.IsNullOrEmpty(cell.Text) ? cell.Text : DateTime.Now.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                                newRow[cell.Start.Column - 1] = d.ToString("yyyy/MM/dd");
                                            }

                                        }
                                        else
                                        {
                                            newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                        }
                                    }
                                }
                                dt.Rows.Add(newRow);
                            }
                        }
                    }
                    return Task.FromResult(dt);
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return null;
        }
    }
}