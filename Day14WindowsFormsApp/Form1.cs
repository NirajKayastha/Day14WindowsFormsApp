

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Day14WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private SqlDataReader reader = null;


        private void Form1_Load(object sender, EventArgs e)
        {
        // SqlConnection con = new SqlConnection();
        // con.ConnectionString = @"Data Source=LAPTOP-6FIM74JD\SQLEXPRESS; Initial Catalog=HR; Integrated Security=true;";
        con =new SqlConnection(@"Data Source=LAPTOP-6FIM74JD\SQLEXPRESS; Initial Catalog=HR; Integrated Security=true;");

            //  SqlCommand cmd = new SqlCommand();

            // cmd.Connection = con;
           // cmd.CommandText = "Select cEmployeeCode,vFirstName,cCity,cState from Employee";

            cmd =new SqlCommand("Select cEmployeeCode,vFirstName,cCity,cState,cSocialSecurityNo from Employee", con) ;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();

            txtEmployeeCode.Text = reader["cEmployeeCode"].ToString();
            txtFirstName.Text = reader["vFirstName"].ToString();
            txtCity.Text = reader["cCity"].ToString();
            txtState.Text = reader["cState"].ToString();
            txtSocialSecurityNo.Text = reader["cSocialSecurityNo"].ToString();

            reader.Close();

            cmd.Dispose();

            con.Close();


        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.ClearText();
        }
        public void ClearText()
        {
            txtEmployeeCode.Text = "";
            txtFirstName.Text = "";
            txtCity.Clear();
            txtState.Text = "";
            txtSocialSecurityNo.Text = "";

            txtEmployeeCode.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(@"Data Source=LAPTOP-6FIM74JD\SQLEXPRESS; Initial Catalog=HR; Integrated Security=true;"))
            {
                using (cmd = new SqlCommand("Select cEmployeeCode,vFirstName,cCity,cState from Employee  Where cEmployeeCode=@EmployeeCode", con))
                {
                    cmd.Parameters.AddWithValue("@EmployeeCode", txtEmployeeCode.Text);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    using (reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            reader.Read();
                            txtFirstName.Text = reader["vFirstName"].ToString();
                            txtCity.Text = reader["cCity"].ToString();
                            txtState.Text = reader["cState"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No record found");
                            this.ClearText();
                        }


                    }

                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConfigurationManager.ConnectionStrings["HRConn"].ConnectionString))
            {
                using (cmd = new SqlCommand("usp_AddNewEmployee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeCode", txtEmployeeCode.Text);
                    cmd.Parameters.AddWithValue("@vFirstName",txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@City",txtCity.Text);
                    cmd.Parameters.AddWithValue("@State",txtState.Text);
                    cmd.Parameters.AddWithValue("@SocialSecurityNo", txtSocialSecurityNo.Text);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("New Employee Created");

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConfigurationManager.ConnectionStrings["HRConn"].ConnectionString))
            {
                using (cmd = new SqlCommand("usp_UpdateCityAndStateByCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@City",txtCity.Text);
                    cmd.Parameters.AddWithValue("@State", txtState.Text);
                    cmd.Parameters.AddWithValue("@EmployeeCode",txtEmployeeCode.Text);
                   // cmd.Parameters.AddWithValue("@vFirstName", txtFirstName.Text);
                   // cmd.Parameters.AddWithValue("@SocialSecurityNo", txtSocialSecurityNo.Text);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Record Updated Successfully");
        }
    }
}

