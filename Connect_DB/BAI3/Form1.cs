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
using System.Data.SqlTypes;

namespace BAI3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class KNDL
        {
            public SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=SinhVien;Integrated Security=True");
            public void myConnect()
            {
                conn.Open();
            }
            public void myClose()
            {
                conn.Close();
            }
            public DataTable create_table(string sql)
            {
                DataTable dt = new DataTable();
                SqlDataAdapter ds = new SqlDataAdapter(sql, conn);
                ds.Fill(dt);
                return (dt);
            }

        }

        KNDL kn = new KNDL();
        private bool found;

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            kn.myClose();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            kn.myConnect();
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text.Trim();
            if (id == "")
            {
                MessageBox.Show("Vui lòng nhập Mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bool found = false;
                string sql = "SELECT MH.MaMonHoc, MH.TenMon, MH.DiemThi FROM MH INNER JOIN SV ON SV.MSSV = MH.MSSV WHERE SV.MSSV = '" + id + "'";
                dataGridViewDiem.DataSource = kn.create_table(sql);

                string sv = "SELECT * FROM SV";
                DataTable t = kn.create_table(sv);
                foreach (DataRow row in t.Rows)
                {
                    if (row["MSSV"].ToString().Trim() == id)
                    {
                        txt_name.Text = row["HoTen"].ToString().Trim();
                        txt_gender.Text = row["GioiTinh"].ToString().Trim();
                        txt_faculty.Text = row["MaKhoa"].ToString().Trim();
                        dtp_dob.Value = Convert.ToDateTime(row["NgaySinh"]);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    MessageBox.Show("Không tìm thấy sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_id.ResetText();
                    txt_id.Focus();
                }
            }    
        }
    }
}
