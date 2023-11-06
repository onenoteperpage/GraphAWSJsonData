using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphAWSJsonData.EC2GraphPlotter
{
    //public class EC2GraphPlotter
    //{
    //    private EC2JsonData data;

    //    public EC2GraphPlotter(EC2JsonData jsonData)
    //    {
    //        data = jsonData;
    //    }

    //    public void EC2PlotGraph(string chartFileName)
    //    {
    //        var chart = new Chart();
    //        chart.Size = new Size(2400, 900);

    //        var chartArea = new ChartArea();
    //        chart.ChartAreas.Add(chartArea);

    //        var series = new Series();
    //        series.ChartType = SeriesChartType.Line;
    //        series.LegendText = data.MetricDataResults[0].Label;
    //        chart.Series.Add(series);

    //        for (int i = 0; i < data.MetricDataResults[0].Timestamps.Count; i++)
    //        {
    //            series.Points.AddXY(data.MetricDataResults[0].Timestamps[i], data.MetricDataResults[0].Values[i]);
    //        }

    //        chart.Titles.Add(data.MetricDataResults[0].Id).Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
    //        chart.Titles[0].ForeColor = Color.Red;

    //        chart.ChartAreas[0].AxisX.Title = "Timestamps";
    //        chart.ChartAreas[0].AxisY.Title = "Values";

    //        chart.SaveImage(chartFileName, ChartImageFormat.Png);
    //    }
    //}
}
