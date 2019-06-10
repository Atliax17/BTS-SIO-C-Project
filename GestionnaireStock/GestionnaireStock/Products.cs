using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionnaireStock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }
        public void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                SqlConnection conn = new SqlConnection("Data Source=192.168.18.144;Initial Catalog=Stock;User ID=sa;Password=abcd4ABCD");
                // Insert
                conn.Open();
                bool status = false;
                if (comboBox1.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                var sqlQuery = "";
                if (IfProductExists(conn, textBox1.Text))
                {
                    sqlQuery = @"UPDATE [Products] SET [ProductName] = '" + textBox2.Text + "', [ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + textBox1.Text + "'";
                }
                else
                {
                    sqlQuery = @"INSERT INTO [dbo].[Products]([ProductCode],[ProductName],[ProductStatus])
                VALUES
           ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                //Load
                LoadData();
                ResetRecords();
            }
        }

        private bool IfProductExists(SqlConnection conn, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 from [dbo].[Products] WHERE [ProductCode] = '" + productCode +"'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void LoadData()
        {
            SqlConnection conn = new SqlConnection("Data Source=192.168.18.144;Initial Catalog=Stock;User ID=sa;Password=abcd4ABCD");
            // Reading
            SqlDataAdapter sda = new SqlDataAdapter("Select * from [dbo].[Products]", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2.Text = "Update";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogresult = MessageBox.Show("Voulez-vous vraiment supprimer cette élément ?", "Message", MessageBoxButtons.YesNo);
            if (dialogresult == DialogResult.Yes)
            {
                if (Validation())
                {
                    SqlConnection conn = new SqlConnection("Data Source=192.168.18.144;Initial Catalog=Stock;User ID=sa;Password=abcd4ABCD");
                    var sqlQuery = "";
                    if (IfProductExists(conn, textBox1.Text))
                    {
                        conn.Open();
                        sqlQuery = @"DELETE FROM [Products] WHERE [ProductCode] = '" + textBox1.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Element introuvable");
                    }
                    //Load
                    LoadData();
                    ResetRecords();
                }
            }
        }

        private void ResetRecords()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            button2.Text = "Add";
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetRecords();
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox1, "Code Produit Nécessaire");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Nom Produit Nécessaire");
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Sélectionné un status");
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
