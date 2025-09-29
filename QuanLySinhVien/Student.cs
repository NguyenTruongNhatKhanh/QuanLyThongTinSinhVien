using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien
{
    internal class Student
    {
        public string MSSV { get; set; }
        public string HoTenLot { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Lop { get; set; }
        public string SoCMND { get; set; }
        public string SoDT { get; set; }
        public string DiaChi { get; set; }
        public List<string> ChuyenNganh { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{MSSV}|{HoTenLot}|{Ten}|{NgaySinh.ToShortDateString()}|{GioiTinh}|{Lop}|{SoCMND}|{SoDT}|{DiaChi}|{string.Join(",", ChuyenNganh)}";
        }
    }
}
