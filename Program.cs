using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using GraphAWSJsonData.Config;
using System.Collections.Generic;
using GraphAWSJsonData.Models.JData;
using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;
using GraphAWSJsonData.Models.SData;
using GraphAWSJsonData.Functions.SqliteFunc;
using System.Diagnostics;
using GraphAWSJsonData.Functions.Menu;
using System.Threading;

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

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "--gen-sql-report":
                        string outputPath = null;
                        string computerName = null;
                        string inputFile = null;

                        for (int i = 0; i < args.Length; i++)
                        {
                            string arg = args[i].ToLower();

                            switch (arg)
                            {
                                case "--output-path":
                                case "-o":
                                    outputPath = (i + 1 < args.Length && !args[i + 1].StartsWith("-")) ? args[i + 1] : null;
                                    i += outputPath != null ? 1 : 0;
                                    break;

                                case "--computer-name":
                                case "-h":
                                    computerName = (i + 1 < args.Length && !args[i + 1].StartsWith("-")) ? args[i + 1] : null;
                                    i += computerName != null ? 1 : 0;
                                    break;

                                case "--input-file":
                                case "-i":
                                    inputFile = (i + 1 < args.Length && !args[i + 1].StartsWith("-")) ? args[i + 1] : null;
                                    i += inputFile != null ? 1 : 0;
                                    break;
                            }
                        }

                        // null value check
                        bool allNotNull = !(outputPath is null || computerName is null || inputFile is null);
                        if (!allNotNull)
                        {
                            Console.WriteLine("One or more value are null. Re-run with '-h' for help");
                            Environment.Exit(0);
                        }

                        // test path of input
                        if (!File.Exists(inputFile))
                        {
                            MenuError.InvalidSqliteDbPath(inputFile);
                        }

                        // create if not exist output directory
                        if (!Directory.Exists(outputPath))
                        {
                            // exists
                            try
                            {
                                Directory.CreateDirectory(outputPath);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unable to create directory: {outputPath}");
                                Console.WriteLine($"Error: {ex.Message}");
                                Environment.Exit(0);
                            }
                        }

                        // can write to
                        try
                        {
                            string tmpFilename = Guid.NewGuid().ToString();
                            using (StreamWriter sw = File.CreateText(Path.Combine(outputPath, $"{tmpFilename}.txt")))
                            {
                                // write something to the file
                                sw.WriteLine("She got that boot stomp, stomp, stomp!");
                            }
                            Thread.Sleep(200);

                            File.Delete(Path.Combine(outputPath, $"{tmpFilename}.txt"));
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"Unable to write to directory: ");
                            Console.ResetColor();
                            Console.WriteLine($"{outputPath}\n");
                            Debug.WriteLine(ex.ToString());
                            Environment.Exit(0);
                        }

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("╔═══════════════════════╗");
                        Console.WriteLine("║ Generating SQL Report ║");
                        Console.WriteLine("╚═══════════════════════╝\n");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("  Output path => ");
                        Console.ResetColor();
                        Console.WriteLine(outputPath);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("  Computer    => ");
                        Console.ResetColor();
                        Console.WriteLine(computerName);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("  SQL input   => ");
                        Console.ResetColor();
                        Console.WriteLine(inputFile);

                        // process sqlgen now
                        string jsonInputFile = SqliteGen.SqlDbToJson(
                            sqliteDbPath: inputFile,
                            serverName: computerName,
                            outPath: outputPath);

                        // create images
                        SqliteGen.JsonToPng(
                            jsonInputFilePath: jsonInputFile,
                            serverName: computerName,
                            outPath: outputPath);
                        break;


                    default:
                        Console.WriteLine("No args provided, using settings.json instead");
                        break;
                }

               

                // get the arguments into variables
                //string sqliteDB = args[0].ToString();
                //string pcName = args[1].ToString();

                //SqliteGen.SqlDbToJson(sqliteDB, pcName);

                //if (!File.Exists(sqliteDB))
                //{
                //    Console.WriteLine(@"Usage: graphawsjsondata.exe <C:\path\to\data.sqlite>");
                //    Environment.Exit(1);
                //}

                //// create list of table names found
                //List<string>tablesFound = new List<string>();

                //// create a list to store the table data
                //List<SDataRoot> sDataRoots = new List<SDataRoot>();

                //// access the SqliteDB
                //using (var connection = new SQLiteConnection($"Data Source={sqliteDB};Version=3;"))
                //{
                //    // open connection
                //    connection.Open();

                //    // access tables
                //    DataTable tableNames = connection.GetSchema("Tables");

                //    // iterate all the tables in the schema
                //    foreach (DataRow row in tableNames.Rows)
                //    {
                //        string tableName = row["TABLE_NAME"].ToString();
                //        Console.WriteLine("Found table: " + tableName);
                //        tablesFound.Add(tableName);
                //    }

                //    // get the data from each tablename into sDataRoots
                //    foreach (string tbl in tablesFound)
                //    {
                //        // create a new collection of SDataPoint
                //        List<SDataPoint> points = new List<SDataPoint>();

                //        using (var command = new SQLiteCommand($"SELECT Timestamp, Value FROM {tbl};", connection))
                //        using (var reader = command.ExecuteReader())
                //        {
                //            while (reader.Read())
                //            {
                //                // Parse the Timestamp and Value columns
                //                string timestampStr = reader["Timestamp"].ToString();
                //                DateTime timestamp = DateTime.Parse(timestampStr);
                //                double value = Convert.ToDouble(reader["Value"]);

                //                // Create a SDataPoint object
                //                SDataPoint tableDataRow = new SDataPoint
                //                {
                //                    Timestamp = timestamp,
                //                    Value = value
                //                };

                //                // add the SDataPoint to the list
                //                points.Add(tableDataRow);
                //            }
                //        }

                //        // finished reading the current table, now add it to the sDataRoots collection
                //        SDataRoot sDataRoot = new SDataRoot
                //        {
                //            SDataTable = tbl,
                //            SDataPoints = points,
                //        };

                //        // add the current SDataRoot to the main list of SDataRoot values
                //        sDataRoots.Add(sDataRoot);
                //    }
                //}

                //// using statement complete, sanity check here
                //Console.WriteLine($"Total Tables: {sDataRoots.Count}");

                //// compile each table into a graph
                //foreach (SDataRoot dRoot in sDataRoots)
                //{
                //    // organise the SData first
                //    List<SDataPoint> sDataPoints = dRoot.SDataPoints.ToList();
                //    sDataPoints = sDataPoints.OrderBy(point => point.Timestamp).ToList();

                //    var chart = new Chart();
                //    chart.Size = new Size(2400, 900);

                //    var chartArea = new ChartArea();
                //    chart.ChartAreas.Add(chartArea);

                //    var series = new Series();
                //    series.ChartType = SeriesChartType.Line;
                //    series.Legend = "Legend goes here";
                //    chart.Series.Add(series);

                //    for (int i = 0; i < sDataPoints.Count; i++)
                //    {
                //        series.Points.AddXY(sDataPoints[i].Timestamp, sDataPoints[i].Value);
                //    }

                //    chart.Titles.Add($"{pcName} {dRoot.SDataTable}").Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                //    chart.Titles[0].ForeColor = Color.Red;

                //    chart.ChartAreas[0].AxisX.Title = "Timstamps";
                //    if (dRoot.SDataTable == "RAM")
                //    {
                //        chart.ChartAreas[0].AxisY.Title = "GB Usage";
                //    }
                //    else
                //    {
                //        chart.ChartAreas[0].AxisY.Title = "% Usage";
                //    }

                //    string outfilePath = Path.GetDirectoryName(sqliteDB);
                //    outfilePath = Path.Combine(outfilePath, $"{dRoot.SDataTable}.png");
                //    chart.SaveImage(outfilePath, ChartImageFormat.Png);
                //}
            }
            // no sql file was referred to, so run the process that concerns the JSON data only
            else
            {
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
}
