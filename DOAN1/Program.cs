using  System;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {

        string connectionString = "Data Source=KhamBenh.db";

        string fullScript = @"
        1.
        CREATE TABLE IF NOT EXISTS VAITRO (
        MaVT INTEGER PRIMARY KEY AUTOINCREMENT,
        TenVT TEXT NOT NULL UNIQUE);

        CREATE TABLE IF NOT EXISTS BENHNHAN (
        MaBN INTEGER PRIMARY KEY AUTOINCREMENT,
        HoTen TEXT NOT NULL,
        NgaySinh TEXT,
        CCCD TEXT UNIQUE,
        SDT TEXT
           );

        CREATE TABLE IF NOT EXISTS THUOC (
        MaThuoc INTEGER PRIMARY KEY AUTOINCREMENT,
        TenThuoc TEXT NOT NULL,
        GiaBan REAL NOT NULL
        );

        2. 
        CREATE TABLE IF NOT EXISTS NGUOIDUNG (
        MaND INTEGER PRIMARY KEY AUTOINCREMENT,
        TenDangNhap TEXT NOT NULL UNIQUE,
        MatKhau TEXT NOT NULL,
        HoTen TEXT,
        MaVT INTEGER NOT NULL,
        FOREIGN KEY (MaVT) REFERENCES VAITRO(MaVT)
            );

        CREATE TABLE IF NOT EXISTS LOTHUOC (
        MaLo INTEGER PRIMARY KEY AUTOINCREMENT,
        MaThuoc INTEGER NOT NULL,
        HanDung TEXT,
        SoLuong INTEGER NOT NULL,
        FOREIGN KEY (MaThuoc) REFERENCES THUOC(MaThuoc)
        );

        3. QUY TRÌNH TIẾP NHẬN , KHÁM BỆNH
        CREATE TABLE IF NOT EXISTS LICHHEN (
        MaLH INTEGER PRIMARY KEY AUTOINCREMENT,
        MaBN INTEGER NOT NULL,
        MaBS INTEGER NOT NULL,
        NgayGio TEXT,
        TrangThai TEXT,
        FOREIGN KEY (MaBN) REFERENCES BENHNHAN(MaBN),
        FOREIGN KEY (MaBS) REFERENCES NGUOIDUNG(MaND)
        );

        CREATE TABLE IF NOT EXISTS PHIEUKHAM (
        MaPK INTEGER PRIMARY KEY AUTOINCREMENT,
        MaLH INTEGER NOT NULL,
        ChuanDoan TEXT,
        TienKham TEXT,
        FOREIGN KEY (MaLH) REFERENCES LICHHEN(MaLH)
        );

        4. HÓA ĐƠN & ĐƠN THUỐC
        CREATE TABLE IF NOT EXISTS HOADON (
        MaHD INTEGER PRIMARY KEY AUTOINCREMENT,
        MaPK INTEGER NOT NULL,
        TongTien REAL NOT NULL,
        TrangThai TEXT,
        FOREIGN KEY (MaPK) REFERENCES PHIEUKHAM(MaPK)
        );

        CREATE TABLE IF NOT EXISTS DONTHUOC (
        MaDT INTEGER PRIMARY KEY AUTOINCREMENT,
        MaPK INTEGER NOT NULL,
        FOREIGN KEY (MaPK) REFERENCES PHIEUKHAM(MaPK)
        );

        5. BẢNG CHI TIẾT ĐƠN THUỐC 
        CREATE TABLE IF NOT EXISTS CHITIETDONTHUOC (
        MaCT INTEGER PRIMARY KEY AUTOINCREMENT,
        MaDT INTEGER NOT NULL,
        MaThuoc INTEGER NOT NULL,
        SoLuong INTEGER NOT NULL,
        LieuDung TEXT,
        FOREIGN KEY (MaDT) REFERENCES DONTHUOC(MaDT),
        FOREIGN KEY (MaThuoc) REFERENCES THUOC(MaThuoc)
        );";
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (SqliteCommand cmd = new SqliteCommand(fullScript, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}






    