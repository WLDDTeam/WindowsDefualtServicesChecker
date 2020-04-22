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
using System.Management;
using System.Data.OleDb;

namespace WindowsDefualtServicesChecker
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public static List<string> src2;
        public Form1()
        {
            InitializeComponent();
        }
        static List<Tuple<string ,string ,string>> allServices()
        {
            
            List<Tuple<string, string ,string>> allServicesList = new List<Tuple<string, string ,string>>();
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                string status;
                if (scTemp.Status == ServiceControllerStatus.Running)
                {
                    status = "Running";
                }
                else
                {
                    status = "Stopped";
                }
                
                allServicesList.Add(new Tuple<string, string, string>(scTemp.DisplayName, scTemp.ServiceName.ToString(), status));
            }
            return allServicesList;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=AppData/WDS.mdb;User Id=admin;Password=;";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string cmd = "select DName,SNameRegistry,DW10Home,DW10Pro,notes from WDS";
                OleDbDataAdapter da = new OleDbDataAdapter(cmd, conn);
                DataSet dt = new DataSet();
                da.Fill(dt);
                dataGridView2.DataSource = dt.Tables[0];
                src2 = dt.Tables[0].AsEnumerable()
                .Select(s => s.Field<string>("SNameRegistry"))
                .Distinct()
                .ToList();
                dataGridView2.Columns[0].HeaderText = "Display Name";
                dataGridView2.Columns[1].HeaderText = "Service Name";
                dataGridView2.Columns[2].HeaderText = "Win10 Home Defualts";
                dataGridView2.Columns[3].HeaderText = "Win10 Pro Defualts";
                dataGridView2.Columns[4].HeaderText = "Notes";

            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void sFetch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = allServices();
            dataGridView1.Columns[0].HeaderText = "Display Name";
            dataGridView1.Columns[1].HeaderText = "Service Name";
            dataGridView1.Columns[2].HeaderText = "Status";

        }

        public void metroButton1_Click(object sender, EventArgs e)
        {
            
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            var dt = ((DataTable)dgv.DataSource).Copy();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (!column.Visible)
                {
                    dt.Columns.Remove(column.Name);
                }
            }
            return dt;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Form3 cyka = new Form3();
            cyka.Show();
        }
    }
}
