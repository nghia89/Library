using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BiTech.Library.DTO
{
    public class Sach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string TenSach { get; set; }
        public string IdDauSach { get; set; }
        public string IdTheLoai { get; set; }
        public string IdKeSach { get; set; }
        public string IdNhaXuatBan { get; set; }
        public string IdTrangThai { get; set; }

        public string MaKiemSoat { get; set; }

        public string Hinh { get; set; }
        public int SoLuong { get; set; }
        public int SoTrang { get; set; }
        public ENgonNgu NgonNgu { get; set; }

        public string NamSanXuat { get; set; }
        public double GiaSach { get; set; }
        public string LinkBiaSach { get; set; }
        public string TomTat { get; set; }      


    }
}
