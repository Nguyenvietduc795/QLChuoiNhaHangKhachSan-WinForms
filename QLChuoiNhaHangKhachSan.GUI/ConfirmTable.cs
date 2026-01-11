using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class ConfirmTable : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;

        /// <summary>
        /// Trả về true nếu người dùng xác nhận thành công
        /// </summary>
        public bool IsConfirmed { get; private set; }

        /// <summary>
        /// TableID tìm được (dùng để gọi món)
        /// </summary>
        public int FoundTableId { get; private set; }

        /// <summary>
        /// Tên bàn tìm được
        /// </summary>
        public string FoundTableName { get; private set; }

        /// <summary>
        /// CustomerID tìm được (nếu có)
        /// </summary>
        public int? FoundCustomerId { get; private set; }

        public ConfirmTable()
        {
            InitializeComponent();

            btnLookUp.Click += btnLookUp_Click;
            btnConfirm.Click += btnConfirm_Click;

            btnConfirm.Enabled = false;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void txtStick_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnLookUp_Click(object sender, EventArgs e)
        {
            string codeStick = txtCodeStick.Text.Trim();
            string customerName = txtCustomer.Text.Trim();

            if (string.IsNullOrEmpty(codeStick) && string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Vui lòng nhập mã đặt bàn hoặc tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Tìm theo mã đặt bàn hoặc tên khách hàng
                    string query = @"
                        SELECT TOP 1 
                            b.BookingID,
                            b.TableID,
                            rt.TableName,
                            b.CustomerID,
                            c.FullName,
                            c.PhoneNumber,
                            c.Email,
                            b.GuestCount,
                            b.BookingDate,
                            b.BookingTime
                        FROM BookingsTable b
                        INNER JOIN RestaurantTable rt ON b.TableID = rt.TableID
                        LEFT JOIN Customers c ON b.CustomerID = c.CustomerId
                        WHERE b.Status = N'Đã đặt'";

                    if (!string.IsNullOrEmpty(codeStick))
                    {
                        query += " AND b.BookingID = @BookingID";
                    }
                    else if (!string.IsNullOrEmpty(customerName))
                    {
                        query += " AND c.FullName LIKE @CustomerName";
                    }

                    query += " ORDER BY b.BookingID DESC";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(codeStick))
                        {
                            int bookingId;
                            if (!int.TryParse(codeStick, out bookingId))
                            {
                                MessageBox.Show("Mã đặt bàn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            cmd.Parameters.AddWithValue("@BookingID", bookingId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CustomerName", "%" + customerName + "%");
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FoundTableId = Convert.ToInt32(reader["TableID"]);
                                FoundTableName = reader["TableName"].ToString();
                                FoundCustomerId = reader["CustomerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CustomerID"]);

                                txtCodeTable.Text = FoundTableName;
                                txtPhone.Text = reader["PhoneNumber"] == DBNull.Value ? string.Empty : reader["PhoneNumber"].ToString();
                                txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                                txtNumber.Text = reader["GuestCount"].ToString();

                                DateTime bookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate"));
                                TimeSpan bookingTime = reader.GetTimeSpan(reader.GetOrdinal("BookingTime"));

                                txtDay.Text = bookingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtTime.Text = bookingDate.Date.Add(bookingTime).ToString("HH:mm", CultureInfo.InvariantCulture);

                                // Hiển thị tên khách nếu có
                                if (!string.IsNullOrEmpty(customerName))
                                {
                                    // Đã nhập tên, giữ nguyên
                                }
                                else
                                {
                                    txtCustomer.Text = reader["FullName"] == DBNull.Value ? string.Empty : reader["FullName"].ToString();
                                }

                                btnConfirm.Enabled = true;

                                MessageBox.Show("Tìm thấy bàn đã đặt.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                ClearFields();
                                btnConfirm.Enabled = false;
                                MessageBox.Show("Không tìm thấy bàn đã đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (FoundTableId == 0)
            {
                MessageBox.Show("Vui lòng tra cứu bàn trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IsConfirmed = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ClearFields()
        {
            txtCodeTable.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtNumber.Text = string.Empty;
            txtDay.Text = string.Empty;
            txtTime.Text = string.Empty;
            FoundTableId = 0;
            FoundTableName = null;
            FoundCustomerId = null;
        }
    }
}
