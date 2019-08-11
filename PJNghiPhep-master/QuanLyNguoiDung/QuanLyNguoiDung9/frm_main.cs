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

namespace QuanLyNguoiDung9
{
    public partial class frm_main : Form
    {
        SqlDataAdapter adap;
        DataSet ds;
        SqlCommandBuilder cmdbl;
        
        public frm_main()
        {
            InitializeComponent();
        }

        string UID = Form1.ID_User;
        SqlConnection con = new SqlConnection(@"Data Source=SIEU\SQLEXPRESS;Initial Catalog=QuanLyNguoiDung2;Integrated Security=True");

        List<string> listper = null;

        private void frm_main_Load(object sender, EventArgs e)
        {
           listper = List_per();
            /////////////////
           
            /////////////////
           try
           {
               con.Open();
               adap = new SqlDataAdapter("SELECT [MaNV],[TenNV],[Email],[GioiTinh],[DiaChi],[SDT],[NgaySinh],[id_user] FROM [dbo].[NHANVIEN]", con);
           ds = new System.Data.DataSet();
           adap.Fill(ds, "NHANVIEN");
            dsNguoiDung.DataSource = ds.Tables[0];
           }
           catch (Exception ex)
           {
               MessageBox.Show("Lỗi" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           finally
           {
               con.Close();
           }
            /////////////////
        }

        private Boolean checkper(string code) {
            Boolean check = false;
            foreach (string item in listper)
            {
                if (item == code)
                {
                    check = true;
                    break;
                }
                else
                {
                    check = false;
                }
            }
            return check;
        }

        private string id_per(){
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_per_relationship WHERE id_user_rel = '" + UID + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["suspended"].ToString() == "False")
                        {
                            id = dr["id_per_rel"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally{
                con.Close();
            }
            
            return id;
        }

        private List<string> List_per() {
            string idper = id_per();
            List<string> termlist = new List<string>();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_permision_detail WHERE id_per = '" + idper + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {   
                            termlist.Add(dr["code_action"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra !");
            }
            finally
            {
                con.Close();
            }
            return termlist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (checkper("EDIT") == true)
            {
                /////////////////////
                try
                {
                    cmdbl = new SqlCommandBuilder(adap);
                    adap.Update(ds, "NHANVIEN");
                    MessageBox.Show("Sửa thành công", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btRe_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
                /////////////////////
            }
            else
            {
                MessageBox.Show("Nope :))");
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int currentIndex = dsNguoiDung.CurrentCell.RowIndex;
                int MaNV = Convert.ToInt32(dsNguoiDung.Rows[currentIndex].Cells[0].Value.ToString());
                //Convert.ToInt32(dsNguoiDung.Rows[currentIndex].Cells[0].Value.ToString());
                string deleteStr = "DELETE FROM [dbo].[NHANVIEN] WHERE MaNV = '" + MaNV + "'";
                SqlCommand deleteCmd = new SqlCommand(deleteStr, con);
                deleteCmd.CommandType = CommandType.Text;
                if (checkper("DELETE") == true)
                {
                    deleteCmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa Thành Công");
                    btRe_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Nope :))");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            if (checkper("ADD") == true)
            {
                Them formThem = new Them();
                formThem.Show();
            }
            else
            {
                MessageBox.Show("Nope :))");
            }
        }
        ///////////////////////////////////////////////////////////////////////////////
        private void btSearch_Click(object sender, EventArgs e)
        {
            TimKiem formTim = new TimKiem();
            formTim.Show();
        }

        private void btRe_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHANVIEN", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dsNguoiDung.DataSource = dt;
        }
        ///////////////////////////////////////////////////////////////////////////////
        
    }
}
