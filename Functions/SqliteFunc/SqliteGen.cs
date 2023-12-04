using GraphAWSJsonData.Functions.Menu;
using GraphAWSJsonData.Models.SData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphAWSJsonData.Functions.SqliteFunc
{
    public class SqliteGen
    {
        public static string SqlDbToJson(string sqliteDbPath, string serverName, string outPath)
        {
            // create a list of tables found in the sqliteDB
            List<string> sqliteTablesFound = new List<string>();

            // create a list to store the sqliteDataRoot data tables
            List<SDataRoot> sqliteDataRoots = new List<SDataRoot>();

            // accesss the sqliteDB file
            using (var connection = new SQLiteConnection($"Data Source={sqliteDbPath};Version=3;"))
            {
                // open connection
                connection.Open();

                // access the table data
                DataTable tableNames = connection.GetSchema("Tables");

                // iterate through the tables and add them to sqliteTablesFound
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("    Tables mapped:");
                Console.ResetColor();
                foreach (DataRow row in tableNames.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("        => ");
                    Console.ResetColor();
                    Console.WriteLine(tableName);
                    sqliteTablesFound.Add(tableName);
                }

                // get data from each table in tableNames
                foreach (string tbl in sqliteTablesFound)
                {
                    // create a new collection of SqliteDataPoint
                    List<SDataPoint> points = new List<SDataPoint>();

                    using (var command = new SQLiteCommand($"SELECT Timestamp, Value FROM {tbl};", connection))
                        using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // parse the timestamp and value columns
                            string timestampStr = reader["Timestamp"].ToString();
                            DateTime timestamp = DateTime.Parse(timestampStr);

                            double value;
                            switch (tbl)
                            {
                                case var x when x.Contains("CPU"):
                                    value = Convert.ToDouble(reader["Value"]);
                                    value = value * 10;
                                    break;
                                case var x when Regex.IsMatch(x, @"^[A-Z]_Drive$"):
                                    // Code for any letter followed by _Drive pattern
                                    value = Convert.ToDouble(reader["Value"]);
                                    value = value * 100;
                                    break;
                                default:
                                    value = Convert.ToDouble(reader["Value"]);
                                    break;
                            }

                            // create teh SDataPoint object (in-memory)
                            SDataPoint tableDataRow = new SDataPoint
                            {
                                Timestamp = timestamp,
                                Value = value,
                            };

                            // add the SDataPoint to the list
                            points.Add(tableDataRow);
                        }
                    }

                    // finished reading the current table, not add it to the sDataRoots collection
                    SDataRoot sDataRoot = new SDataRoot
                    {
                        SDataTable = tbl,
                        SDataPoints = points,
                    };

                    // add teh current SDataRoot to the main list of SDataRoot list (created at the top =>  sqliteDataRoots)
                    sqliteDataRoots.Add(sDataRoot);
                }
            }

            // write to JSON output
            if (!Directory.Exists(Path.Combine(outPath, DateTime.Now.ToString("yyyy-MM-dd"))))
                Directory.CreateDirectory(Path.Combine(outPath, DateTime.Now.ToString("yyyy-MM-dd")));
            string jsonOutputFilePath = Path.Combine(outPath, DateTime.Now.ToString("yyyy-MM-dd"), $"{serverName}.json");
            
            // serialize the list to JSON format
            string jsonData = JsonSerializer.Serialize(sqliteDataRoots, new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON file more readable
            });

            // write the JSON data to the file
            if (File.Exists(jsonOutputFilePath))
                File.Delete(jsonOutputFilePath);
            File.WriteAllText(jsonOutputFilePath, jsonData);

            // return jsonfile output
            return jsonOutputFilePath;
        }

        public static void JsonToPng(string jsonInputFilePath, string serverName, string outPath)
        {
            if (!File.Exists (jsonInputFilePath))
            {
                MenuError.InvalidJsonInputPath(jsonInputFilePath);
                Environment.Exit(1);
            }

            // read the JSON data
            string jsonDataFromFile = File.ReadAllText(jsonInputFilePath);

            // deserialize back to SDataRoot
            List<SDataRoot> deserializedList = JsonSerializer.Deserialize<List<SDataRoot>>(jsonDataFromFile);

            // access deserialized objects
            foreach (var item in deserializedList)
            {
                var root = item as SDataRoot;
                if (root != null)
                {
                    List<SDataPoint> points = root.SDataPoints;

                    // put the points in chronological order, this is faster than LINQ, because it's in-place
                    points.Sort((point1, point2) => point1.Timestamp.CompareTo(point2.Timestamp));

                    //else you do it like this, but need to create a new in-memory thingy:
                    //sDataPoints = sDataPoints.OrderBy(point => point.Timestamp).ToList();

                    // create a chart
                    var chart = new Chart();
                    chart.Size = new System.Drawing.Size(2400, 900);

                    var chartArea = new ChartArea();
                    chart.ChartAreas.Add(chartArea);

                    var series = new Series();
                    series.ChartType = SeriesChartType.Line;
                    series.Legend = "Legend goes here?";
                    series.BorderWidth = 3;
                    series.Color = System.Drawing.Color.Red;
                    chart.Series.Add(series);

                    // add the data
                    for (int i = 0; i < points.Count; i++)
                    {
                        series.Points.AddXY(points[i].Timestamp, points[i].Value);
                    }

                    DateTime lastMonthDate = DateTime.Now.AddMonths(-1);
                    string formattedDate = lastMonthDate.ToString("MMMM yyyy");
                    

                    chart.ChartAreas[0].AxisX.Title = "Timestamps";
                    chart.ChartAreas[0].AxisY.Title = "% Usage";
                    if (root.SDataTable == "RAM")
                    {
                        chart.Titles.Add($"{serverName} RAM Utilisation {formattedDate}").Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    }
                    if (root.SDataTable == "CPU")
                    {
                        chart.Titles.Add($"{serverName} CPU Utilisation {formattedDate}").Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    }
                    if (root.SDataTable.EndsWith("_Drive"))
                    {
                        string driveLetter = root.SDataTable.Replace("_Drive", "");
                        chart.Titles.Add($"{serverName} {driveLetter} Drive HDD Utilisation {formattedDate}").Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    }

                    string outFilePath = Path.Combine(outPath, $"{DateTime.Now.AddMonths(-1).ToString("yyyyMM")}-{serverName}-{root.SDataTable}.png");
                    if (File.Exists(outFilePath))
                    {
                        File.Delete(outFilePath);
                    }
                    chart.SaveImage(outFilePath, ChartImageFormat.Png);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("    Chart saved to => ");
                    Console.ResetColor();
                    Console.WriteLine(outFilePath);
                }
            }
        }
    }
}
