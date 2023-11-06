using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;


namespace GraphAWSJsonData.Plotter
{
    //public class RdsGraphPlotter
    //{
    //    //public static void RdsPlotGraph(RdsJsonData data, string chartName, string pngOutputFileName)
    //    public static void RdsPlotGraph(RdsJsonData data, string chartName, string pngOutputFileName)
    //    {
    //        // create a chart
    //        Chart chart = new Chart();
    //        chart.Size = new Size(2400, 900);

    //        // set chart type to column
    //        chart.ChartAreas.Add(new ChartArea());
    //        chart.Series.Add(new Series());
    //        chart.Series[0].ChartType = SeriesChartType.Column;

    //        // add data points to the chart
    //        for (int i = 0; i < data.Datapoints.Count; i++)
    //        {
    //            DateTime dt = DateTime.Parse(data.Datapoints[i].Timestamp.ToString());
    //            chart.Series[0].Points.AddXY(dt, data.Datapoints[i].Average);
    //        }


    //        // Set chart title and axes labels
    //        chart.ChartAreas[0].AxisX.Title = "Timestamps";
    //        chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 30, FontStyle.Regular);
    //        chart.ChartAreas[0].AxisY.Title = "CPU Usage";
    //        chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 30, FontStyle.Regular);

    //        // add the RDS instance name above the graph
    //        chart.Titles.Insert(0, new Title(chartName));
    //        chart.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 36, FontStyle.Bold);
    //        chart.Titles[0].ForeColor = Color.Red;


    //        // Format Y-axis labels to include percent symbol
    //        chart.ChartAreas[0].AxisY.LabelStyle.Format = "0'%'";
    //        chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 30);

    //        // save the chart
    //        chart.SaveImage(imageFileName: pngOutputFileName, format: ChartImageFormat.Png);



    //        //var chart = new Chart();


    //        //var chartArea = new ChartArea();
    //        //chart.ChartAreas.Add(chartArea);

    //        //var series = new Series();
    //        //series.ChartType = SeriesChartType.Line;
    //        //series.LegendText = "";
    //        //chart.Series.Add(series);

    //        //foreach (var datapoint in data.Datapoints)
    //        //{
    //        //    series.Points.AddXY(datapoint.Timestamp, datapoint.Average);
    //        //}

    //        //chart.Titles.Add(chartName).Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
    //        //chart.Titles[0].ForeColor = Color.Red;

    //        //chart.ChartAreas[0].AxisX.Title = "Timestamps";
    //        //chart.ChartAreas[0].AxisY.Title = "CPU Usage";

    //        //chart.SaveImage(pngOutputFileName, ChartImageFormat.Png);
    //    }
    //}
}
