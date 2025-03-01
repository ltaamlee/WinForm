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
using System.Collections;

namespace BAI5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class KNDL
        {
            public SqlConnection conn= new SqlConnection("Data Source=.;Initial Catalog=QLNV;Integrated Security=True");

            public void myConnect()
            {
                conn.Open();
            }

            public void myClose()
            {
                conn.Close();
            }


            public void add(string ma, string ten, string ngsinh, string sdt, string pb)
            {
                string sql = "INSERT INTO NV(manv, tennv, ngsinh, sdt, mapb) VALUES (@manv, @tennv, @ngsinh, @sdt, @mapb)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@manv", ma);
                cmd.Parameters.AddWithValue("@tennv", ten);
                cmd.Parameters.AddWithValue("@ngsinh", ngsinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@mapb", pb);
                cmd.ExecuteNonQuery();
            }

            public void upd(string ma, string ten, string ngsinh, string sdt, string pb)
            {

                string sql = "UPDATE NV SET tennv = @ten, ngsinh = @ngsinh, sdt = @sdt, mapb = @pb WHERE manv = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@ngsinh", ngsinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@pb", pb);
                cmd.ExecuteNonQuery();
            }

            public void del(string ma)
            {
                string sql = "DELETE FROM NV WHERE manv = @manv";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@manv", ma);
                cmd.ExecuteNonQuery();
            }


        }

        KNDL kn = new KNDL();

        public void LoadDepartment()
        {
            string sql = "SELECT * FROM PB";
            SqlDataAdapter da = new SqlDataAdapter(sql, kn.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lst_department.DisplayMember = "tenpb";
            lst_department.ValueMember = "mapb";
            lst_department.DataSource = dt;
        }
        public void LoadEmployee_Department(string mapb)
        {
            string sql = "SELECT * FROM NV WHERE mapb = @mapb";
            SqlDataAdapter da = new SqlDataAdapter(sql, kn.conn);
            da.SelectCommand.Parameters.AddWithValue("@mapb", mapb);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridViewNV.DataSource = dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            kn.myConnect();
            LoadDepartment();
            this.btn_save.Enabled = false;
            this.btn_cancel.Enabled = false;

        }
      
        private void lst_department_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maPB = lst_department.SelectedValue.ToString();
            LoadEmployee_Department(maPB);
        }

        private void dataGridViewNV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = -1;
            DataTable tab = new DataTable();
            tab = (DataTable)dataGridViewNV.DataSource;
            index = dataGridViewNV.SelectedCells[0].RowIndex;
            DataRow r = tab.Rows[index];

            txt_id.Text = r["manv"].ToString();
            txt_name.Text = r["tennv"].ToString();
            txt_dob.Text = r["ngsinh"].ToString();
            txt_phone.Text = r["sdt"].ToString();
            txt_id_department.Text = r["mapb"].ToString();
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            txt_id.ResetText();
            txt_name.ResetText();
            txt_dob.ResetText();
            txt_phone.ResetText();
            txt_id_department.ResetText();
            txt_name.Focus();

            this.btn_add.Enabled = false;
            this.btn_save.Enabled = true;
            this.btn_cancel.Enabled = true;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM NV WHERE manv = @manv";
            SqlCommand cmd = new SqlCommand(sql, kn.conn);
            cmd.Parameters.AddWithValue("@manv", txt_id.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                kn.upd(txt_id.Text, txt_name.Text, txt_dob.Text, txt_phone.Text, txt_id_department.Text);
                MessageBox.Show("Đã sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để sửa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            string mapb = txt_id_department.Text;
            LoadEmployee_Department(mapb);

        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM NV WHERE manv = @manv";
            SqlCommand cmd = new SqlCommand(sql, kn.conn);
            cmd.Parameters.AddWithValue("@manv", txt_id.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                string mapb = txt_id_department.Text;
                kn.del(txt_id.Text);
                LoadEmployee_Department(txt_id_department.Text);
                txt_id.ResetText();
                txt_name.ResetText();
                txt_dob.ResetText();
                txt_phone.ResetText();
                txt_id_department.ResetText();
                txt_name.Focus();
                MessageBox.Show("Đã xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để xóa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            this.btn_add.Enabled = true;
            this.btn_save.Enabled = false;
            this.btn_cancel.Enabled = false;

            string sql = "SELECT * FROM NV WHERE manv = @manv";
            SqlCommand cmd = new SqlCommand(sql, kn.conn);
            cmd.Parameters.AddWithValue("@manv", txt_id.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                kn.add(txt_id.Text, txt_name.Text, txt_dob.Text, txt_phone.Text, txt_id_department.Text);
                string mapb = txt_id_department.Text;
                LoadEmployee_Department(mapb);
                txt_id.ResetText();
                txt_name.ResetText();
                txt_dob.ResetText();
                txt_phone.ResetText();
                txt_id_department.ResetText();
                txt_name.Focus();
                MessageBox.Show("Đã thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Mã nhân viên đã tồn tại, vui lòng lập lại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}
