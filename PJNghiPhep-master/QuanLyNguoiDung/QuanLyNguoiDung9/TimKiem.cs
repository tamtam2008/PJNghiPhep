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
namespace QuanLyNguoiDung9
{
   
    public partial class TimKiem : Form
    {
        public TimKiem()
        {

            InitializeComponent();
            

        }
         
        private void TimKiem_Load(object sender, EventArgs e)
        {
            
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=SIEU\SQLEXPRESS;Initial Catalog=QuanLyNguoiDung2;Integrated Security=True");
                con.Open();
               SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM [dbo].[NHANVIEN] WHERE TenNV like '%"+txtName.Text+"%'", con);
               DataSet ds = new System.Data.DataSet();
                adap.Fill(ds, "NHANVIEN");
                dsNhanVien.DataSource = ds.Tables["NHANVIEN"].DefaultView;
                con.Close();    
        }

        
    }
}
