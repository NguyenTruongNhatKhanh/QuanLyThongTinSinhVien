using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace QuanLySinhVien
{
    internal class StudentIO
    {
        public static void SaveToTxt(string path, List<Student> list)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var sv in list)
                {
                    sw.WriteLine($"{sv.MSSV}|{sv.HoTenLot}|{sv.Ten}|{sv.NgaySinh}|{sv.GioiTinh}|{sv.Lop}|{sv.SoCMND}|{sv.SoDT}|{sv.DiaChi}|{string.Join(",", sv.ChuyenNganh)}");
                }
            }
        }

        public static List<Student> LoadFromTxt(string path)
        {
            var list = new List<Student>();
            if (!File.Exists(path)) return list;

            foreach (var line in File.ReadAllLines(path))
            {
                var parts = line.Split('|');
                if (parts.Length >= 10)
                {
                    list.Add(new Student
                    {
                        MSSV = parts[0],
                        HoTenLot = parts[1],
                        Ten = parts[2],
                        NgaySinh = DateTime.Parse(parts[3]),
                        GioiTinh = parts[4],
                        Lop = parts[5],
                        SoCMND = parts[6],
                        SoDT = parts[7],
                        DiaChi = parts[8],
                        ChuyenNganh = new List<string>(parts[9].Split(',', (char)StringSplitOptions.RemoveEmptyEntries))
                    });
                }
            }
            return list;
        }


        // JSON
        public static void SaveToJson(string path, List<Student> list)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static List<Student> LoadFromJson(string path)
        {
            if (!File.Exists(path)) return new List<Student>();
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
        }

        // XML
        public static void SaveToXml(string path, List<Student> list)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Student>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                xs.Serialize(fs, list);
            }
        }

        public static List<Student> LoadFromXml(string path)
        {
            if (!File.Exists(path)) return new List<Student>();
            XmlSerializer xs = new XmlSerializer(typeof(List<Student>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return (List<Student>)xs.Deserialize(fs);
            }
        }
    }
}
