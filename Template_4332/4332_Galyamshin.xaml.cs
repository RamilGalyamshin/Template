using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.Json;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;
using System.Data.Entity.Validation;


namespace Template_4332
{
    /// <summary>
    /// Interaction logic for _4332_Galyamshin.xaml
    /// </summary>
    public partial class _4332_Galyamshin : Window
    {
        public _4332_Galyamshin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "*.xls;*.xlsx",
                Filter = "файл Excel (Spisok.xlsx)|*.xlsx",
                Title = "Выберите файл базы данных"
            };
            if (!(ofd.ShowDialog() == true))
                return;
            string[,] list;
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
            int _columns = (int)lastCell.Column;
            int _rows = (int)lastCell.Row;
            list = new string[_rows, _columns];
            for (int j = 0; j < _columns; j++)
                for (int i = 0; i < _rows; i++)
                    list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text;
            ObjWorkBook.Close(false, Type.Missing, Type.Missing);
            ObjWorkExcel.Quit();
            GC.Collect();

            using (Galyamshin4332Container usersEntities = new Galyamshin4332Container())
            {
                for (int i = 0; i < _rows; i++)
                {
                    usersEntities.galyamshinSet.Add(new galyamshin()
                    {
                        CodeOrder = list[i, 1],
                        CreateDate = list[i, 2],
                        CodeClient = list[i, 3],
                        Services = list[i, 4],
                        ProkatTime= list[i, 5]
                    });
                }
                usersEntities.SaveChanges();
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<galyamshin> allStudents;

            using (Galyamshin4332Container usersEntities = new Galyamshin4332Container())
            {
                allStudents = usersEntities.galyamshinSet.ToList().OrderBy(s => s.Id).ToList();
            }
            var app = new Excel.Application();
            app.SheetsInNewWorkbook = 7;
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);

            for (int i = 1; i < 8; i++)
            {
                int startRowIndex = 1;
                Excel.Worksheet worksheet = app.Worksheets.Item[i];
                worksheet.Name = "Категория " + Convert.ToString(i);
                worksheet.Cells[1][startRowIndex] = "Id";
                worksheet.Cells[2][startRowIndex] = "Code order";
                worksheet.Cells[3][startRowIndex] = "CreateDate";
                worksheet.Cells[4][startRowIndex] = "CodeClient";
                worksheet.Cells[5][startRowIndex] = "Service";
                startRowIndex++;

               
                foreach (var usluga in allStudents)
                {
                    if (usluga.Services != "Стоимость, руб.  за час")
                    {
                        string tip = "";
                        if (usluga.ProkatTime == "120 минут" || usluga.ProkatTime == "2 часа") { tip = "Категория 1"; }
                        if (usluga.ProkatTime == "600 минут" || usluga.ProkatTime == "10 часов")  { tip = "Категория 2"; }
                        if (usluga.ProkatTime == "320 минут") { tip = "Категория 3"; }
                        if (usluga.ProkatTime == "480 минут") { tip = "Категория 4"; }
                        if (usluga.ProkatTime == "4 часа") { tip = "Категория 5"; }
                        if (usluga.ProkatTime == "6 часов") { tip = "Категория 6"; }
                        if (usluga.ProkatTime == "12 часов") { tip = "Категория 7"; }
                        if (tip == worksheet.Name)
                        {
                            worksheet.Cells[1][startRowIndex] = usluga.Id;
                            worksheet.Cells[2][startRowIndex] = usluga.CodeOrder;
                            worksheet.Cells[3][startRowIndex] = usluga.CreateDate;
                            worksheet.Cells[4][startRowIndex] = usluga.CodeClient;
                            worksheet.Cells[5][startRowIndex] = usluga.Services;
                            startRowIndex++;
                        }
                    }

                }

                worksheet.Columns.AutoFit();
            }
            app.Visible = true;

        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "*.json",
                Filter = "файл Json |*.json",
                Title = "Выберите файл"
            };

            if (!(ofd.ShowDialog() == true))
                return;

            List<galyamshin> list = new List<galyamshin>();

            
            
            
            using (Galyamshin4332Container db = new Galyamshin4332Container())
            {
                list = await JsonSerializer.DeserializeAsync<List<galyamshin>>(new FileStream(ofd.FileName, FileMode.Open));
                foreach (galyamshin person in list)
                {


                    db.galyamshinSet.Add(new galyamshin()
                    {
                        Id = person.Id,
                        CodeOrder = person.CodeOrder,
                        CreateDate = person.CreateDate,
                        CodeClient = person.CodeClient,
                        Services = person.Services,
                        ProkatTime = person.ProkatTime
                    });

                }
                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Данные импортированы!");
                }
                catch (DbEntityValidationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<galyamshin> people = new List<galyamshin>();
            using (Galyamshin4332Container db = new Galyamshin4332Container())
            {
                people = db.galyamshinSet.ToList();
                var app = new Word.Application();
                Word.Document document = app.Documents.Add();

                List<galyamshin> category_1 = new List<galyamshin>();
                List<galyamshin> category_2 = new List<galyamshin>();
                List<galyamshin> category_3 = new List<galyamshin>();
                List<galyamshin> category_4 = new List<galyamshin>();
                List<galyamshin> category_5 = new List<galyamshin>();
                List<galyamshin> category_6 = new List<galyamshin>();
                List<galyamshin> category_7 = new List<galyamshin>();

                using (Galyamshin4332Container isrpoEntities = new Galyamshin4332Container())
                {
                    category_1 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "120" || x.ProkatTime == "2").OrderBy(y => y.Id).ToList();
                    category_2 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "600" || x.ProkatTime == "10").OrderBy(y => y.Id).ToList();
                    category_3 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "320").OrderBy(y => y.Id).ToList();
                    category_4 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "480").OrderBy(y => y.Id).ToList();
                    category_5 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "4").OrderBy(y => y.Id).ToList();
                    category_6 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "6").OrderBy(y => y.Id).ToList();
                    category_7 = isrpoEntities.galyamshinSet.Where(x => x.ProkatTime == "12").OrderBy(y => y.Id).ToList();
                }

                var allCategories = new List<List<galyamshin>>()
                {
                    category_1,
                    category_2,
                    category_3,
                    category_4,
                    category_5,
                    category_6,
                    category_7
                };
                int i = 1;
                foreach (var category in allCategories)
                {
                    Word.Paragraph paragraph = document.Paragraphs.Add();
                    Word.Range range = paragraph.Range;
                    range.Text = "Категория " + i;
                    i++;
                    paragraph.set_Style("Заголовок 1");
                    range.InsertParagraphAfter();

                    Word.Paragraph tableParagraph = document.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    Word.Table peopleTable = document.Tables.Add(tableRange, category.Count() + 1, 5);
                    peopleTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    peopleTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    peopleTable.Range.Cells.VerticalAlignment = (Word.WdCellVerticalAlignment)Word.WdVerticalAlignment.wdAlignVerticalCenter;

                    Word.Range cellRange;
                    cellRange = peopleTable.Cell(1, 1).Range;
                    cellRange.Text = "Id";
                    cellRange = peopleTable.Cell(1, 2).Range;
                    cellRange.Text = "Код заказа";
                    cellRange = peopleTable.Cell(1, 3).Range;
                    cellRange.Text = "Дата создания";
                    cellRange = peopleTable.Cell(1, 4).Range;
                    cellRange.Text = "Код клиента";
                    cellRange = peopleTable.Cell(1, 5).Range;
                    cellRange.Text = "Услуги";
                    peopleTable.Rows[1].Range.Bold = 1;
                    peopleTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    int j = 1;
                    foreach (var person in category)
                    {
                        cellRange = peopleTable.Cell(j + 1, 1).Range;
                        cellRange.Text = person.Id.ToString();
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        cellRange = peopleTable.Cell(j + 1, 2).Range;
                        cellRange.Text = person.CodeOrder;
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        cellRange = peopleTable.Cell(j + 1, 3).Range;
                        cellRange.Text = person.CreateDate;
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        cellRange = peopleTable.Cell(j + 1, 4).Range;
                        cellRange.Text = person.CodeClient;
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        cellRange = peopleTable.Cell(j + 1, 5).Range;
                        cellRange.Text = person.Services;
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        j++;
                    }

                    

                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

                }
                app.Visible = true;

                document.SaveAs2(@"D:\outputFileWord.docx");
                document.SaveAs2(@"D:\outputFilePdf.pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
        }
    }
}
