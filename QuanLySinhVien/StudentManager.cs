using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien
{
    internal class StudentManager
    {
        public List<Student> Students { get; private set; } = new List<Student>();

        // Load
        public void Load(string path, string type)
        {
            if (type == "txt") Students = StudentIO.LoadFromTxt(path);
            else if (type == "json") Students = StudentIO.LoadFromJson(path);
            else if (type == "xml") Students = StudentIO.LoadFromXml(path);
        }

        // Save
        public void Save(string path, string type)
        {
            if (type == "txt") StudentIO.SaveToTxt(path, Students);
            else if (type == "json") StudentIO.SaveToJson(path, Students);
            else if (type == "xml") StudentIO.SaveToXml(path, Students);
        }

        // Thêm / Cập nhật
        public void AddOrUpdate(Student sv)
        {
            var existing = Students.FirstOrDefault(s => s.MSSV == sv.MSSV);
            if (existing != null)
            {
                Students.Remove(existing);
            }
            Students.Add(sv);
        }

        // Xóa
        public void Delete(string mssv)
        {
            Students.RemoveAll(s => s.MSSV == mssv);
        }

        public void DeleteMany(List<string> mssvList)
        {
            Students.RemoveAll(s => mssvList.Contains(s.MSSV));
        }

        // Tìm kiếm
        public List<Student> Search(string mssv = "", string ten = "", string lop = "")
        {
            return Students.Where(s =>
                (string.IsNullOrEmpty(mssv) || s.MSSV.Contains(mssv)) &&
                (string.IsNullOrEmpty(ten) || s.Ten.Contains(ten)) &&
                (string.IsNullOrEmpty(lop) || s.Lop.Contains(lop))
            ).ToList();
        }
    }   
}
