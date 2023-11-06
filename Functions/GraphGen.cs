using GraphAWSJsonData.Models.JData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphAWSJsonData.Functions
{
    //public class GraphGen
    //{
    //    public void GenPng(string dirP)
    //    {
    //        // Directory path
    //        string directoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "CBA", "data");

    //        // File name pattern
    //        string fileNamePattern = "SYDA-P*-data.json";

    //        // Get all files in the directory that match the pattern
    //        string[] files = Directory.GetFiles(directoryPath, fileNamePattern);

    //        foreach (string jsonFilePath in files)
    //        {
    //            string jsonText = File.ReadAllText(jsonFilePath);

    //            Console.WriteLine(jsonText);

    //            JCpuUtilizationRoot data = JsonConvert.DeserializeObject<JCpuUtilizationRoot>(jsonText);

    //            foreach (var metricDataResult in data.MetricDataResults)
    //            {
    //                // Create a chart
    //                Chart chart = new Chart();

    //                // Set chart type to Line
    //                chart.ChartAreas.Add(new ChartArea());
    //                chart.Series.Add(new Series());
    //                chart.Series[0].ChartType = SeriesChartType.Column;

    //                // Add data points to the chart
    //                for (int i = 0; i < metricDataResult.Timestamps.Length; i++)
    //                {
    //                    DateTime dt = DateTime.Parse(metricDataResult.Timestamps[i]);
    //                    chart.Series[0].Points.AddXY(dt, metricDataResult.Values[i]);
    //                }

    //                // Set chart title and axes labels
    //                chart.Titles.Add(metricDataResult.Label);
    //                chart.ChartAreas[0].AxisX.Title = "Timestamp";
    //                chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 30, FontStyle.Regular); //
    //                chart.ChartAreas[0].AxisY.Title = "Value";
    //                chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 30, FontStyle.Regular); //

    //                // Set chart image size to 800x600 pixels
    //                chart.Size = new System.Drawing.Size(2400, 900);

    //                // Add the "Id" value as the header above the image
    //                chart.Titles.Insert(0, new Title(metricDataResult.Id));
    //                chart.Titles[0].Font = new Font("Arial", 36, FontStyle.Bold);
    //                chart.Titles[0].ForeColor = Color.Red;

    //                // Format Y-axis labels to include percent symbol
    //                chart.ChartAreas[0].AxisY.LabelStyle.Format = "0'%'";
    //                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 30);

    //                // Save the chart as a PNG file
    //                string chartFileName = $@"{directoryPath}\{metricDataResult.Id}_chart.png";
    //                Console.WriteLine(chartFileName);
    //                //string chartFileName = $"{metricDataResult.Id}_chart.png";
    //                chart.SaveImage(chartFileName, ChartImageFormat.Png);

    //                // Remove source data.json file
    //                //File.Delete(jsonFilePath);

    //                //Console.WriteLine($"Line graph saved as {chartFileName}");
    //            }
    //        }
    //    }
    //}
}
