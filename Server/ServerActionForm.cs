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
using System.Xml.Linq;

namespace DNS_Simulation
{
    public partial class ServerActionForm : Form
    {
        public ServerActionForm()
        {
            InitializeComponent();
        }

        private void ServerActionForm_Load(object sender, EventArgs e)
        {

        }
        public bool Found(string dnsName)
        {

            XmlDocument root = new XmlDocument();
            root.Load(@"XMLFile1.xml");
            XmlNode node = root.SelectSingleNode("DNS/row/var[@name='" + dnsName + "']");
            try
            {
                if (node.Attributes["value"].Value != null)
                {
                    //string value = node.Attributes["value"].Value; ///NOT-FOUND // CRASHED // NHO FIX NHA NGOC HUYEN
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool checkIPAvailable(string ipAddress)
        {

            XmlDocument root = new XmlDocument();
            root.Load(@"XMLFile1.xml");
            XmlNode node = root.SelectSingleNode("DNS/row/var[@value='" + ipAddress + "']");
            try
            {
                if (node.Attributes["value"].Value != null)
                {
                    //string value = node.Attributes["value"].Value; ///NOT-FOUND // CRASHED // NHO FIX NHA NGOC HUYEN
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public string findIP(string dnsName)
        {

            XmlDocument root = new XmlDocument();
            root.Load(@"XMLFile1.xml");
            XmlNode node = root.SelectSingleNode("DNS/row/var[@name='" + dnsName + "']");
            string value = node.Attributes["value"].Value;
            return value;
        }

        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(this.tbDNS.Text == "" || this.tbIP.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Found(this.tbDNS.Text))
                {
                    MessageBox.Show("Tên miền đã tồn tại!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (ValidateIPv4(this.tbIP.Text) && checkIPAvailable(this.tbIP.Text))
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(@"XMLFile1.xml");
                        //find the row node to be able to add to him more "var" nodes
                        XmlNode rowNode = xdoc.SelectSingleNode("/DNS/row");
                        //create new "var" node
                        XmlNode serverPathNode = xdoc.CreateElement("var");
                        //the details are attributes and not nodes so we add them as attributes to the var node
                        XmlAttribute xmlAttribute = xdoc.CreateAttribute("name");
                        xmlAttribute.Value = this.tbDNS.Text;
                        serverPathNode.Attributes.Append(xmlAttribute);
                        xmlAttribute = xdoc.CreateAttribute("value");
                        xmlAttribute.Value = tbIP.Text;
                        serverPathNode.Attributes.Append(xmlAttribute);
                        //add the var node to the row node
                        rowNode.AppendChild(serverPathNode);
                        string xmlcontents = xdoc.InnerXml;
                        xdoc.Save(@"XMLFile1.xml");
                        MessageBox.Show("Thêm thành công tên miền!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Định dạng IP Sai! hoặc IP đã tồn tại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
             
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.tbDNS.Text == "" || this.tbIP.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!Found(this.tbDNS.Text))
                {
                    MessageBox.Show("Tên miền chưa tồn tại! \n Thêm tên miền để chỉnh sửa", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (ValidateIPv4(this.tbIP.Text) && checkIPAvailable(this.tbIP.Text))
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(@"XMLFile1.xml");

                        XmlNodeList aNodes = xdoc.SelectNodes("/DNS/row/var[@name='" + this.tbDNS.Text + "']");
                        foreach (XmlNode aNode in aNodes)
                        {
                            XmlAttribute ipAttribute = aNode.Attributes["value"];
                            ipAttribute.Value = this.tbIP.Text;
                        }
                        string xmlcontents = xdoc.InnerXml;
                        xdoc.Save(@"XMLFile1.xml");
                        MessageBox.Show("Sửa thành công tên miền!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Định dạng IP Sai! hoặc IP đã tồn tại ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }       
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.tbDNS.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!Found(this.tbDNS.Text))
                {
                    MessageBox.Show("Tên miền chưa tồn tại! Không thể xoá! ", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(@"XMLFile1.xml");

                    XmlNode aNodes = xdoc.SelectSingleNode("/DNS/row/var[@name='" + this.tbDNS.Text + "']");
                    if (aNodes != null)
                    {
                        aNodes.ParentNode.RemoveChild(aNodes);
                    }
                    string xmlcontents = xdoc.InnerXml;
                    xdoc.Save(@"XMLFile1.xml");
                    MessageBox.Show("Xoá thành công tên miền!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }       
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowDNS showDNS = new ShowDNS();
            showDNS.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
