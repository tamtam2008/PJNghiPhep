using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace QuanLyNguoiDung9
{
    public partial class Them : Form
    {
        //SqlDataAdapter adap;
        //DataSet ds;
        public Them()
        {
            InitializeComponent();
        }
        
         
        SqlConnection con = new SqlConnection(@"Data Source=SIEU\SQLEXPRESS;Initial Catalog=QuanLyNguoiDung2;Integrated Security=True");

        public static string ID_user = "";

        private void btLuu_Click(object sender, EventArgs e)
        {
            
            //ID_user = getID();
            ////Xử lý thêm dữ liệu/////////////////
            string conString = ConfigurationManager.ConnectionStrings["QLNguoiDung"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();
            string sqlINSERT = "INSERT INTO [dbo].[NHANVIEN]([TenNV],[Email],[GioiTinh],[DiaChi],[SDT],[NgaySinh],[id_user]) VALUES (@TenNV, @Email, @GioiTinh,@DiaChi,@SDT,@NgaySinh,@id_user)";
            SqlCommand cmd = new SqlCommand(sqlINSERT, con);
           
            cmd.Parameters.AddWithValue("TenNV", tbTen.Text);
            cmd.Parameters.AddWithValue("NgaySinh", dtpNgaySinh.Value);
            cmd.Parameters.AddWithValue("GioiTinh", tbGioiTinh.Text);
            cmd.Parameters.AddWithValue("DiaChi", tbDiaChi.Text);
            cmd.Parameters.AddWithValue("SDT", tbSdt.Text);
            cmd.Parameters.AddWithValue("Email", tbEmail.Text);
            cmd.Parameters.AddWithValue("id_user", tbUID.Text);
            /////////////////////////////
            if (tbUID.Text == "")
            {
                MessageBox.Show("Vui lòng điền User ID");
            }
            else if (tbTen.Text =="")
            {
                MessageBox.Show("Vui lòng điền Tên");
            }
            else if (tbEmail.Text == "")
            {
                MessageBox.Show("Vui lòng điền Email");
            }
            else if (tbDiaChi.Text == "")
            {
                MessageBox.Show("Vui lòng điền Địa chỉ");
            }
            else
            {
                SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM [dbo].[NHANVIEN] WHERE id_user  = '"+tbUID.Text+"'",con);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("User ID tồn tại, vui lòng nhập User ID khác");
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới thành công rồi nha cưng :))");
                }
            }
            ////////////////////////////
            
            con.Close();
        }
        /////////////////////////////////////////////////
        //public DataTable truyvan(string sql) {
        //    con.Open();
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand(sql, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    con.Close();
        //    return dt;
        //}
        //////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Them_Load(object sender, EventArgs e)
        {
            //string conString = ConfigurationManager.ConnectionStrings["QLNguoiDung"].ConnectionString.ToString();
            //con = new SqlConnection(conString);
            //con.Close();

            //try
            //{
            //    con.Open();
            //    adap = new SqlDataAdapter("SELECT [MaNV],[TenNV],[Email],[GioiTinh],[DiaChi],[SDT],[NgaySinh],[id_user] FROM [dbo].[NHANVIEN]", con);
            //    ds = new System.Data.DataSet();
            //    adap.Fill(ds, "NHANVIEN");
            //    dataView.DataSource = ds.Tables[0];

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    con.Close();
            //}
        //}
        ///////////////////////////////////////////////////////////////////////////////
        
        ///////////////////////////////////////////////////////////////////////////////
    }
    }
}
