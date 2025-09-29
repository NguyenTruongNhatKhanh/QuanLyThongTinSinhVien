using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frm1 : Form
    {
        private StudentManager ql = new StudentManager();
        private string filePath = "students.json";   // lưu JSON mặc định
        private string fileType = "json";

        public frm1()
        {
            InitializeComponent();
        }

        // Khi mở form
        private void frm1_Load(object sender, EventArgs e)
        {
            ql.Load(filePath, fileType);
            CapNhatDataGrid();
        }

        // Khi đóng form -> lưu dữ liệu
        private void frm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            ql.Save(filePath, fileType);
        }

        // Hiển thị danh sách
        private void CapNhatDataGrid()
        {
            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = ql.Students.Select(s => new
            {
                s.MSSV,
                HoTen = s.HoTenLot + " " + s.Ten,
                s.NgaySinh,
                s.GioiTinh,
                s.Lop,
                s.SoCMND,
                s.SoDT,
                s.DiaChi,
                Mon = string.Join(",", s.ChuyenNganh)
            }).ToList();
        }

        // Lấy thông tin từ form
        private Student LayThongTin()
        {
            if (txtMSSV.Text.Length != 7 || !txtMSSV.Text.All(char.IsDigit))
                throw new Exception("MSSV phải gồm 7 chữ số.");

            if (txtCMND.Text.Length != 9 || !txtCMND.Text.All(char.IsDigit))
                throw new Exception("CMND phải gồm 9 chữ số.");

            if (txtSoDT.Text.Length != 10 || !txtSoDT.Text.All(char.IsDigit))
                throw new Exception("SĐT phải gồm 10 chữ số.");

            if (string.IsNullOrWhiteSpace(txtHoLot.Text) ||
                string.IsNullOrWhiteSpace(txtTen.Text) ||
                string.IsNullOrWhiteSpace(cboLop.Text))
                throw new Exception("Phải nhập đầy đủ thông tin.");

            var sv = new Student
            {
                MSSV = txtMSSV.Text,
                HoTenLot = txtHoLot.Text,
                Ten = txtTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rdNam.Checked ? "Nam" : "Nữ",
                Lop = cboLop.Text,
                SoCMND = txtCMND.Text,
                SoDT = txtSoDT.Text,
                DiaChi = txtDiaChi.Text,
                ChuyenNganh = chkChuyenNganh.CheckedItems.Cast<string>().ToList()
            };
            return sv;
        }

        // Thêm / cập nhật
        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            try
            {
                var sv = LayThongTin();
                ql.AddOrUpdate(sv);
                ql.Save(filePath, fileType);
                CapNhatDataGrid();
                MessageBox.Show("Thêm/Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu");
            }
        }

        // Khi chọn 1 dòng -> load vào form
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var mssv = dgvSinhVien.Rows[e.RowIndex].Cells["Mssv"].Value.ToString();
            var sv = ql.Students.FirstOrDefault(s => s.MSSV == mssv);
            if (sv != null)
            {
                txtMSSV.Text = sv.MSSV;
                txtHoLot.Text = sv.HoTenLot;
                txtTen.Text = sv.Ten;
                dtpNgaySinh.Value = sv.NgaySinh;
                if (sv.GioiTinh == "Nam") rdNam.Checked = true; else rdNu.Checked = true;
                cboLop.Text = sv.Lop;
                txtCMND.Text = sv.SoCMND;
                txtSoDT.Text = sv.SoDT;
                txtDiaChi.Text = sv.DiaChi;

                for (int i = 0; i < chkChuyenNganh.Items.Count; i++)
                {
                    chkChuyenNganh.SetItemChecked(i, sv.ChuyenNganh.Contains(chkChuyenNganh.Items[i].ToString()));
                }
            }
        }

        // Tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text.Trim();
            string ten = txtTen.Text.Trim();
            string lop = cboLop.Text.Trim();

            var kq = ql.Search(mssv, ten, lop);

            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = kq.Select(s => new
            {
                s.MSSV,
                HoTen = s.HoTenLot + " " + s.Ten,
                s.NgaySinh,
                s.GioiTinh,
                s.Lop,
                s.SoCMND,
                s.SoDT,
                s.DiaChi,
                Mon = string.Join(",", s.ChuyenNganh)
            }).ToList();
        }

        // Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
