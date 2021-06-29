using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace DNS_Simulation
{
    public partial class ShowDNS : Form
    {
        public ShowDNS()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ShowDNS_Load(object sender, EventArgs e)
        {
            XmlReader xmlFile = XmlReader.Create("XMLFile1.xml", new XmlReaderSettings());
            DataSet dataSet = new DataSet();
            //Read xml to dataset
            dataSet.ReadXml(xmlFile);
            //Pass var table to datagridview datasource
            dataGridView1.DataSource = dataSet.Tables["var"];
            //Close xml reader
            xmlFile.Close();
        }
    }
}
