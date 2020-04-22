using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;

namespace WindowsDefualtServicesChecker
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {

        public List<string> src1 = new List<string>();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                src1.Add(scTemp.ServiceName.ToString());
            }
            List<string> comparedList = new List<string>();
            foreach (var i in src1)
            {
                if (Form1.src2.Contains(i) == false)
                {
                    comparedList.Add(i);
                }
            }

            DataTable table = ConvertListToDataTable(comparedList);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].HeaderText = "New Services";
        }
        static DataTable ConvertListToDataTable(List<string> list)
        {
            // New table.
            DataTable table = new DataTable();

            table.Columns.Add();

            // Add rows.
            foreach (string array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            
                            string[] outputCsv = new string[1];
                            
                                foreach (var item in src1)
                                {
                                    outputCsv[0] += item + ",";
                                }
                            
                            
                               

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully.", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export.", "Info");
            }
        }
    }
}
