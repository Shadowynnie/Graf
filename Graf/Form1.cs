using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bindingSource1.DataSource = new List<DataPoint>();

            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Months",

            });

            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Value"
            });

            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Right;
        }

        private void UpgradeGraph() 
        {
            if (bindingSource1.DataSource == null)
            {
                return;
            }

            cartesianChart1.Series.Clear();
            LiveCharts.SeriesCollection series = new LiveCharts.SeriesCollection();

            var years = (from o in bindingSource1.DataSource as List<DataPoint> select new {Year = o.Year}).Distinct();

            foreach (var year in years)
            {
                List<double> values = new List<double>();

                for (int month = 1; month <= 12; month++) 
                {
                    double value = 0;

                    var data = from o in bindingSource1.DataSource as List<DataPoint>
                               where o.Year.Equals(year.Year) && o.Month.Equals(month)
                               orderby o.Month ascending
                               select new { o.Value, o.Month };

                    if(data.SingleOrDefault() != null) 
                    {
                        value = data.SingleOrDefault().Value;
                        values.Add(value);
                    }
                }

                series.Add(new LineSeries()
                {
                    Title = year.Year.ToString(),
                    Values = new ChartValues<double>(values)
                });
            }

            cartesianChart1.Series = series;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpgradeGraph();
        }

        // Export graph to PNG:
        private void ExportButton_Click(object sender, EventArgs e)
        {
            string filePath = "export/exported.png";
            try
            {
                using (Bitmap bmp = new Bitmap(cartesianChart1.Width, cartesianChart1.Height)) 
                {
                    cartesianChart1.DrawToBitmap(bmp, cartesianChart1.ClientRectangle);
                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }
                MessageBox.Show("Graph was successfully exported to: " + filePath);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return;
            }

        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            PrintDialog printD = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            if (printD.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e) 
        {
            Bitmap bmp = new Bitmap(cartesianChart1.Width, cartesianChart1.Height);
            cartesianChart1.DrawToBitmap(bmp, cartesianChart1.ClientRectangle);

            e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
}
