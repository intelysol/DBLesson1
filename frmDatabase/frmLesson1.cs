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

namespace frmDatabase
{
    public partial class frmLesson1 : Form
    {
        SqlConnection Conn=new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Administrator\documents\visual studio 2013\Projects\frmDatabase\frmDatabase\Database.mdf';Integrated Security=True");
        SqlCommand cmd;
        string sSQL = "";

        public frmLesson1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillcmbDepartments();
            txtId.Clear();
            txtDptName.Clear();

            txtId.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FillcmbDepartments();
            txtId.Clear();
            txtDptName.Clear();
            btnSave.Text = "&Save";
            txtId.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtId.Text.Length==0)
            {
                MessageBox.Show("Please enter the id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
                return;
            }

            if (txtDptName.Text.Length==0)
            {
                MessageBox.Show("Please enter the Department Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDptName.Focus();
                return;                
            }

            if (btnSave.Text =="&Save")
            {

                sSQL = "Insert into Departments(id,DeptName) values(@id,@DeptName)";
            }
            else
            {
                sSQL = "Update Departments Set DeptName=@DeptName Where id=@id";
            }


            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }

            cmd = new SqlCommand(sSQL, Conn);
            cmd.Parameters.AddWithValue("@id", txtId.Text);
            cmd.Parameters.AddWithValue("@DeptName", txtDptName.Text);
            cmd.ExecuteNonQuery();

            btnNew_Click(sender, e);

            MessageBox.Show("Data Sucessfully Saved");
            
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0)
                return;

            sSQL = "Select id,DeptName from Departments where id=@id";
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            cmd = new SqlCommand(sSQL, Conn);
            cmd.Parameters.AddWithValue("@id", txtId.Text);
            SqlDataReader dr = cmd.ExecuteReader();

            btnSave.Text = "&Save";
            if(dr.HasRows)
            {
                dr.Read();
                btnSave.Text = "&Update";
                txtDptName.Text = dr["DeptName"].ToString();
            }

            dr.Close();
        }

        private void FillcmbDepartments()
        {
            sSQL = "Select id,DeptName from Departments";
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }

            SqlDataAdapter da = new SqlDataAdapter(sSQL, Conn);
            DataTable dt = new DataTable();

            da.Fill(dt);

            cmbDepartments.DisplayMember = "DeptName";
            cmbDepartments.ValueMember = "id";
            cmbDepartments.DataSource = dt;
        }

        private void cmbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtId.Text = cmbDepartments.SelectedValue.ToString();
            txtId_Leave(sender, e);
        }
    }
}
