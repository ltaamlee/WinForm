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

namespace BAI4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class KNDL
        {
            public SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=QLKhoa;Integrated Security=True");

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
                return dt;
            }

            public void add(string ma, string ten)
            {
                string sql = "INSERT INTO Khoa(makhoa,tenkhoa) " + "VALUES('" + ma + "', N'" + ten + "')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }

            public void del(string ma, string ten)
            {
                string sql = "DELETE FROM Khoa WHERE makhoa = '" + ma + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }

            public void upd(string ma, string ten)
            {
                string sql = "UPDATE Khoa SET makhoa='" + ma + "', tenkhoa=N'" + ten + "' WHERE makhoa='" + ma + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }



            public int count()
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string sql = "SELECT COUNT(*) FROM Khoa";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int cnt = (int)cmd.ExecuteScalar();
                conn.Close();
                return cnt;
            }
        }

        KNDL kn = new KNDL();

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            kn.myClose();
        }

        public void LoadData()
        {
            string sql = "SELECT * FROM Khoa";
            dataGridViewKhoa.DataSource = kn.create_table(sql);
            txt_sum.Text = kn.count().ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kn.myConnect();
            LoadData();
            this.btn_save.Enabled = false;
            this.btn_cancel.Enabled = false;
        }

        private void dataGridViewKhoa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = -1;
            DataTable tab = new DataTable();
            tab = (DataTable)dataGridViewKhoa.DataSource;
            index = dataGridViewKhoa.SelectedCells[0].RowIndex;
            DataRow row = tab.Rows[index];

            txt_makhoa.Text = row["makhoa"].ToString();
            txt_tenkhoa.Text = row["tenkhoa"].ToString();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            txt_makhoa.Text = "";
            txt_tenkhoa.Text = "";
            txt_makhoa.Focus();

            this.btn_save.Enabled = true;
            this.btn_cancel.Enabled = true;
            this.btn_add.Enabled = false;
            LoadData();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            this.btn_add.Enabled = true;
            this.btn_save.Enabled = false;
            this.btn_cancel.Enabled = false;

            string sql = "SELECT * FROM Khoa WHERE makhoa='" + txt_makhoa.Text+ "'";
            DataTable dt = new DataTable();
            dt = kn.create_table(sql);
            if (dt.Rows.Count == 0)
            {
                kn.add(txt_makhoa.Text, txt_tenkhoa.Text);
                txt_makhoa.ResetText();
                txt_tenkhoa.ResetText();
                txt_makhoa.Focus();
                MessageBox.Show("Đã thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Mã khoa đã tồn tại, vui lòng nhập lại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LoadData();

        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Khoa WHERE makhoa='" + txt_makhoa.Text + "'";
            DataTable dt = new DataTable();
            dt = kn.create_table(sql);
            if (dt.Rows.Count != 0)
            {
                kn.upd(txt_makhoa.Text, txt_tenkhoa.Text);
                MessageBox.Show("Đã sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để sửa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LoadData();

        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Khoa WHERE  makhoa='" + txt_makhoa.Text + "'";
            DataTable dt = new DataTable();
            dt = kn.create_table(sql);
            if (dt.Rows.Count != 0)
            {
                kn.del(txt_makhoa.Text, txt_tenkhoa.Text);

                txt_makhoa.ResetText();
                txt_tenkhoa.ResetText();
                txt_makhoa.Focus();
                MessageBox.Show("Đã xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để xóa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LoadData();

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            txt_makhoa.ResetText();
            txt_tenkhoa.ResetText();
            txt_makhoa.Focus();
            this.btn_add.Enabled = true;
            LoadData();
        }
    }
}
