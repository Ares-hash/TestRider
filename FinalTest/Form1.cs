using Microsoft.Data.Sqlite;
namespace WinFormsApp.db;

public class Database
{
    
    public static string DuongDan = LayDuongDan();

    static string LayDuongDan()
    {
        string thumuc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "HospitalMS");
        Directory.CreateDirectory(thumuc);
        return Path.Combine(thumuc, "HospitalMS.db");
    }

    public static SqliteConnection MoKetNoi()
    {   
        var ketnoi = new SqliteConnection("Data Source=" + DuongDan);
        ketnoi.Open();
        return ketnoi;
    }

    public static void KhoiTao()
    {
        using var ketnoi = MoKetNoi();

        // Bật kiểm tra ràng buộc khóa ngoại
        using (var pragmaCmd = new SqliteCommand("PRAGMA foreign_keys = ON;", ketnoi))
        {
            pragmaCmd.ExecuteNonQuery();
        }

        string sql = @"
            -- 1. DANH MỤC CƠ BẢN
            CREATE TABLE IF NOT EXISTS VAITRO (
                MaVT INTEGER PRIMARY KEY AUTOINCREMENT,
                TenVT TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS BENHNHAN (
                MaBN INTEGER PRIMARY KEY AUTOINCREMENT,
                HoTen TEXT NOT NULL,
                NgaySinh TEXT,
                GioiTinh TEXT,
                CCCD TEXT UNIQUE,
                SDT TEXT,
                DiaChi TEXT
            );

            CREATE TABLE IF NOT EXISTS THUOC (
                MaThuoc INTEGER PRIMARY KEY AUTOINCREMENT,
                TenThuoc TEXT NOT NULL,
                GiaBan REAL NOT NULL
            );

            2. TÀI KHOẢN VÀ KHO
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

            3. QUY TRÌNH TIẾP NHẬN, KHÁM BỆNH
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
                TienKham REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (MaLH) REFERENCES LICHHEN(MaLH)
            );

            4. HÓA ĐƠN , ĐƠN THUỐC
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

            5. CHI TIẾT ĐƠN THUỐC
            CREATE TABLE IF NOT EXISTS CHITIETDONTHUOC (
                MaCT INTEGER PRIMARY KEY AUTOINCREMENT,
                MaDT INTEGER NOT NULL,
                MaThuoc INTEGER NOT NULL,
                SoLuong INTEGER NOT NULL,
                LieuDung TEXT,
                FOREIGN KEY (MaDT) REFERENCES DONTHUOC(MaDT),
                FOREIGN KEY (MaThuoc) REFERENCES THUOC(MaThuoc)
            );
        ";

        using var cmd = new SqliteCommand(sql, ketnoi);
        cmd.ExecuteNonQuery();
    }
}