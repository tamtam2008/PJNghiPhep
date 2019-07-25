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
using System.Security.Cryptography;

namespace QuanLyNguoiDung9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=SIEU\SQLEXPRESS;Initial Catalog=QuanLyNguoiDung2;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string getID() {
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_user WHERE user_name = '" + txt_username.Text + "' AND pass = '" + txt_pass.Text + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        id = dr["id_user"].ToString();
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Loi");
            }
            finally { con.Close(); }
            return id;
        }
        public static string ID_User = "";

        private void btLogin_Click(object sender, EventArgs e)
        {
            ID_User = getID();
            if (ID_User != "")
            {
                frm_main fmain = new frm_main();
                fmain.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tai khoan hoac mat khau sai");
            }
        }
        public String Hash(byte[] val) {
            using(SHA1Managed sha1 = new SHA1Managed()){
                var hash = sha1.ComputeHash(val);
                return Convert.ToBase64String(hash);

            }
        }
        public string Md5hash(byte[] value) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                var hash = md5.ComputeHash(value);

                return Convert.ToBase64String(hash);
            }
        }
        private void txt_pass_TextChanged(object sender, EventArgs e)
        {
            byte[] passtohash = System.Text.Encoding.UTF8.GetBytes(txt_pass.Text.ToString());
            hashed.Text = Md5hash(passtohash);
        }
    }
}
