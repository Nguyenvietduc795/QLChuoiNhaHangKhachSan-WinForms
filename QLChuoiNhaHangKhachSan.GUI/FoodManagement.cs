using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmFoodManagement : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        private readonly List<FoodView> _foods = new List<FoodView>();
        private readonly List<OptionItem> _categories = new List<OptionItem>();
        private readonly List<OptionItem> _statuses = new List<OptionItem>();

        public frmFoodManagement()
        {
            InitializeComponent();

            Load += frmFoodManagement_Load;
            dgvDsMon.SelectionChanged += dgvDsMon_SelectionChanged;
            guna2Panel2.Click += guna2Panel2_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            cboTilter.SelectedIndexChanged += cboTilter_SelectedIndexChanged;
            bntAdd.Click += bntAdd_Click;
            btnRepair.Click += btnRepair_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void lblItemCode_Click(object sender, EventArgs e)
        {

        }

        private void frmFoodManagement_Load(object sender, EventArgs e)
        {
            txtDishName.Enabled = true;
            txtTotal.Enabled = true;
            txtType.Enabled = true;
            txtStatus.Enabled = true;

            LoadCategories();
            LoadStatuses();
            LoadFoods();
            ApplyFilter();
        }

        private void LoadCategories()
        {
            _categories.Clear();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM FoodCategory ORDER BY CategoryName", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _categories.Add(new OptionItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            txtType.DataSource = null;
            txtType.DisplayMember = nameof(OptionItem.Name);
            txtType.ValueMember = nameof(OptionItem.Id);
            txtType.DataSource = _categories.ToList();
            if (txtType.Items.Count > 0) txtType.SelectedIndex = 0;

            PopulateCategoryFilter();
        }

        private void PopulateCategoryFilter()
        {
            cboTilter.Items.Clear();
            cboTilter.Items.Add("Tất cả");
            foreach (var cat in _categories)
            {
                cboTilter.Items.Add(cat.Name);
            }
            if (cboTilter.Items.Count > 0) cboTilter.SelectedIndex = 0;
        }

        private void LoadStatuses()
        {
            _statuses.Clear();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT StatusID, StatusName FROM FoodStatus ORDER BY StatusID", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _statuses.Add(new OptionItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            txtStatus.DataSource = null;
            txtStatus.DisplayMember = nameof(OptionItem.Name);
            txtStatus.ValueMember = nameof(OptionItem.Id);
            txtStatus.DataSource = _statuses.ToList();
            if (txtStatus.Items.Count > 0) txtStatus.SelectedIndex = 0;
        }

        private void LoadFoods()
        {
            _foods.Clear();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"SELECT f.FoodID, f.FoodName, f.CategoryID, c.CategoryName, f.Price, f.StatusID, s.StatusName
                                              FROM Food f
                                              LEFT JOIN FoodCategory c ON f.CategoryID = c.CategoryID
                                              LEFT JOIN FoodStatus s ON f.StatusID = s.StatusID", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _foods.Add(new FoodView
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            CategoryId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            CategoryName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            StatusId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            StatusName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                        });
                    }
                }
            }
        }

        private void ApplyFilter()
        {
            string search = (txtSearch.Text ?? string.Empty).Trim();
            string categoryFilter = cboTilter.SelectedItem != null ? cboTilter.SelectedItem.ToString() : string.Empty;

            var filtered = _foods.AsEnumerable();

            if (!string.IsNullOrEmpty(categoryFilter) && !categoryFilter.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(f => string.Equals(RemoveDiacritics(f.CategoryName), RemoveDiacritics(categoryFilter), StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(f => f.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 || f.Id.ToString().Contains(search));
            }

            dgvDsMon.Rows.Clear();
            foreach (var item in filtered)
            {
                dgvDsMon.Rows.Add(item.Id, item.Name, item.CategoryName, item.Price.ToString("N0"), item.StatusName);
            }

            txtSum.Text = filtered.Count().ToString();
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void dgvDsMon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDsMon.SelectedRows.Count == 0) return;

            var row = dgvDsMon.SelectedRows[0];
            int id;
            if (!int.TryParse(Convert.ToString(row.Cells[0].Value), out id)) return;

            var item = _foods.FirstOrDefault(f => f.Id == id);
            if (item == null) return;

            txtItemCode.Text = item.Id.ToString();
            txtDishName.Text = item.Name;
            txtTotal.Text = item.Price.ToString("N0");

            if (item.CategoryId != 0)
            {
                txtType.SelectedValue = item.CategoryId;
            }

            if (item.StatusId != 0)
            {
                txtStatus.SelectedValue = item.StatusId;
            }
        }

        private void dgvDsMon_Leave(object sender, EventArgs e)
        {
            // Giữ nguyên thông tin đang hiển thị khi rời lưới
        }

        private void guna2Panel2_Click(object sender, EventArgs e)
        {
            // Khi nhấp vào panel bên phải (guna2Panel2), ẩn thông tin hiển thị
            txtDishName.Text = string.Empty;
            txtTotal.Text = string.Empty;
            txtItemCode.Text = string.Empty;
            try { dgvDsMon.ClearSelection(); } catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cboTilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private bool TryReadFoodInput(out string name, out int categoryId, out decimal price, out int statusId)
        {
            name = txtDishName.Text.Trim();
            categoryId = 0;
            price = 0;
            statusId = 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtType.SelectedValue == null || !int.TryParse(txtType.SelectedValue.ToString(), out categoryId))
            {
                MessageBox.Show("Vui lòng chọn loại món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var rawPrice = (txtTotal.Text ?? string.Empty).Trim().Replace(".", string.Empty).Replace(",", string.Empty);
            if (!decimal.TryParse(rawPrice, out price) || price < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtStatus.SelectedValue == null || !int.TryParse(txtStatus.SelectedValue.ToString(), out statusId))
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void bntAdd_Click(object sender, EventArgs e)
        {
            string name; int categoryId; decimal price; int statusId;
            if (!TryReadFoodInput(out name, out categoryId, out price, out statusId)) return;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(@"INSERT INTO Food (FoodName, CategoryID, Price, StatusID)
                                                  VALUES (@FoodName, @CategoryID, @Price, @StatusID);", conn))
                {
                    cmd.Parameters.AddWithValue("@FoodName", name);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@StatusID", statusId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadFoods();
                ApplyFilter();
                MessageBox.Show("Thêm món thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtItemCode.Text.Trim(), out id))
            {
                MessageBox.Show("Vui lòng chọn món cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _foods.FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                MessageBox.Show("Không tìm thấy món cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cho phép chỉ sửa trường được nhập: nếu trống thì giữ giá trị cũ
            string name = string.IsNullOrWhiteSpace(txtDishName.Text) ? existing.Name : txtDishName.Text.Trim();

            int categoryId = existing.CategoryId;
            if (txtType.SelectedValue != null)
            {
                int parsedCat;
                if (int.TryParse(txtType.SelectedValue.ToString(), out parsedCat))
                {
                    categoryId = parsedCat;
                }
            }

            int statusId = existing.StatusId;
            if (txtStatus.SelectedValue != null)
            {
                int parsedStatus;
                if (int.TryParse(txtStatus.SelectedValue.ToString(), out parsedStatus))
                {
                    statusId = parsedStatus;
                }
            }

            decimal price = existing.Price;
            var rawPriceInput = (txtTotal.Text ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(rawPriceInput))
            {
                var rawPrice = rawPriceInput.Replace(".", string.Empty).Replace(",", string.Empty);
                decimal parsedPrice;
                if (!decimal.TryParse(rawPrice, out parsedPrice) || parsedPrice < 0)
                {
                    MessageBox.Show("Đơn giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                price = parsedPrice;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(@"UPDATE Food
                                                  SET FoodName = @FoodName,
                                                      CategoryID = @CategoryID,
                                                      Price = @Price,
                                                      StatusID = @StatusID
                                                  WHERE FoodID = @FoodID;", conn))
                {
                    cmd.Parameters.AddWithValue("@FoodName", name);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@StatusID", statusId);
                    cmd.Parameters.AddWithValue("@FoodID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadFoods();
                ApplyFilter();
                MessageBox.Show("Cập nhật món thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtItemCode.Text.Trim(), out id))
            {
                MessageBox.Show("Vui lòng chọn món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("DELETE FROM Food WHERE FoodID = @FoodID", conn))
                {
                    cmd.Parameters.AddWithValue("@FoodID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadFoods();
                ApplyFilter();
                ClearInputs();
                MessageBox.Show("Xóa món thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtItemCode.Text = string.Empty;
            txtDishName.Text = string.Empty;
            txtTotal.Text = string.Empty;
            if (txtType.Items.Count > 0) txtType.SelectedIndex = 0;
            if (txtStatus.Items.Count > 0) txtStatus.SelectedIndex = 0;
        }

        private class FoodView
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public decimal Price { get; set; }
            public int StatusId { get; set; }
            public string StatusName { get; set; }
        }

        private class OptionItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void frmFoodManagement_Load_1(object sender, EventArgs e)
        {

        }
    }
}
