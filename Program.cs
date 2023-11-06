using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using GraphAWSJsonData.Config;
using System.Collections.Generic;
using GraphAWSJsonData.Models.JData;
using Newtonsoft.Json;


namespace GraphAWSJsonData
{
    internal class Program
    {
        static readonly string rootPath = Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            string executablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executablePath);
            string settingsPath = Path.Combine(rootPath, "settings.json");

            string settingsJson = string.Empty;
            if (File.Exists(settingsPath))
            {
                settingsJson = File.ReadAllText(settingsPath);
            }
            else if (File.Exists(Path.Combine(executableDirectory, "settings.json")))
            {
                settingsJson = File.ReadAllText(Path.Combine(executableDirectory, "settings.json"));
            }
            else
            {
                Console.WriteLine("Settings.json file is not where it's supposed to be");
                Console.WriteLine("Terminating");
                Environment.Exit(1);
            }
            
            Settings config = JsonConvert.DeserializeObject<Settings>(settingsJson);

            foreach (string client in config.Clients)
            {
                string scanDir = Path.Combine(config.ClientRootPath, client);
                Console.WriteLine($"Target directory: {scanDir}");

                // Test path exists
                if (!Directory.Exists(scanDir))
                {
                    Console.WriteLine("Directory not exists, update 'settings.json' file required");
                    return;
                }

                // File name pattern
                string fileNamePattern = "*SYDA-*.json";

                // Get all files in the directory that match the pattern
                string[] files = Directory.GetFiles(scanDir, fileNamePattern);

                if (files.Length < 1)
                {
                    Console.WriteLine("There was not JSON files found, re-run main.ps1 file first");
                    return;
                }

                foreach (string jsonFilePath in files)
                {
                    try
                    {
                        // Read the JSON content from the file
                        string jsonText = File.ReadAllText(jsonFilePath);

                        string jsonFileName = Path.GetFileNameWithoutExtension(jsonFilePath);

                        // Deserialize the JSON content into a JCpuUtilizationRoot object
                        JCpuUtilizationRoot cpuUtilization = JsonConvert.DeserializeObject<JCpuUtilizationRoot>(jsonText);

                        List<JDatapoint> jDataPoints = cpuUtilization.JDatapoints.ToList();

                        jDataPoints = jDataPoints.OrderBy(point => point.Timestamp).ToList();

                        if (jDataPoints != null)
                        {
                            Console.WriteLine(jDataPoints[3].Unit);
                            Console.WriteLine(jDataPoints[3].Timestamp);
                            Console.WriteLine(jDataPoints[3].Average);
                        }

                        var chart = new Chart();
                        chart.Size = new Size(2400, 900);

                        var chartArea = new ChartArea();
                        chart.ChartAreas.Add(chartArea);

                        var series = new Series();
                        series.ChartType = SeriesChartType.Line;
                        series.LegendText = "Legend Text Goes Here";
                        chart.Series.Add(series);

                        for (int i = 0; i < jDataPoints.Count; i++)
                        {
                            series.Points.AddXY(jDataPoints[i].Timestamp, jDataPoints[i].Average);
                        }

                        chart.Titles.Add(jsonFileName).Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                        chart.Titles[0].ForeColor = Color.Red;

                        chart.ChartAreas[0].AxisX.Title = "Timestamps";
                        chart.ChartAreas[0].AxisY.Title = "CPU Utilization %";

                        chart.SaveImage(Path.Combine(scanDir, $"{jsonFileName}.png"), ChartImageFormat.Png);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }
            }


            // Create PDF report
            string today = DateTime.Now.ToString("yyyyMMdd");
            string todayPdf = DateTime.Now.ToString("yyyy-MM-dd");
            string pdfOutput = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"CBA Reporting WE {today}.pdf");
            string htmlOutput = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"CBA Reporting WE {today}.html");
            string jpgOutput = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"CBA Reporting WE {today}.jpg");
            string[] outputFiles = { pdfOutput, htmlOutput, jpgOutput };
            //foreach (string f in outputFiles)
            //{
            //    try
            //    {
            //        File.Delete(f);
            //    }
            //    catch { }
            //}

            //// setup the PDF file
            //PdfWriter writer = new PdfWriter(pdfOutput);
            //PdfDocument pdf = new PdfDocument(writer);
            //Document document = new Document(pdf);

            //Paragraph header = new Paragraph("Production CBA Reporting")
            //   .SetTextAlignment(TextAlignment.CENTER)
            //   .SetFontSize(20);
            //document.Add(header);

            //Paragraph subheader = new Paragraph($"WE {todayPdf}")
            //   .SetTextAlignment(TextAlignment.CENTER)
            //   .SetFontSize(15);
            //document.Add(subheader);

            //// Line separator
            //LineSeparator ls = new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine());
            //document.Add(ls);

            //// create next page
            //pdf.AddNewPage();
            //document.Add(new AreaBreak());

            //// get all the PNG files
            //string pngFileNamePattern = "SYDA-P*_chart.png";
            //string[] pngFiles = Directory.GetFiles(directoryPath, pngFileNamePattern);

            //// count how many PNG files are found
            //int pngCount = pngFiles.Count();

            //int batchSize = 3; // Number of items to process in each batch
            //for (int i = 0; i <pngFiles.Length; i += batchSize)
            //{
            //    Console.WriteLine($"Batch {i / batchSize + 1}:"); // Print batch number

            //    // create a table with single column for the iteration
            //    float[] pointColumnWidths = { 700f };
            //    Table table = new Table(pointColumnWidths);

            //    for (int j = i; j < i + batchSize && j < pngFiles.Length; j++)
            //    {
            //        Console.WriteLine(pngFiles[j]); // Process item in batch
            //        Cell cellX = new Cell();
            //        cellX.SetBorder(Border.NO_BORDER);
            //        ImageData data = ImageDataFactory.Create(pngFiles[j]);
            //        iText.Layout.Element.Image img = new iText.Layout.Element.Image(data);
            //        cellX.Add(img.SetAutoScale(true));
            //        table.AddCell(cellX);
            //    }

            //    Console.WriteLine(); // Print empty line to separate batches
            //    document.Add(table);
            //    pdf.AddNewPage();
            //    document.Add(new AreaBreak());
            //}

            //// remove the last page, it's blank
            //pdf.RemovePage(pdf.GetLastPage());

            //document.Close();
        }
    }
}
