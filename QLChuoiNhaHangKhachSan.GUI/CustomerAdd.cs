using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomerAdd : Form
    {
        private Dictionary<string, string> _countryPhonePrefixes;

        public enum CustomerFormMode { Add, Edit }

        public CustomerFormMode Mode { get; set; } = CustomerFormMode.Add;

        // Thuộc tính public để đọc/ghi dữ liệu
        public string CustomerId
        {
            get => TxbCustomersID.Text.Trim();
            set => TxbCustomersID.Text = value;
        }

        public string CustomerName
        {
            get => TxbCustomersName.Text.Trim();
            set => TxbCustomersName.Text = value;
        }

        // CCCD -> txbCCCD
        public string CCCD
        {
            get => txbCCCD.Text.Trim();
            set => txbCCCD.Text = value;
        }

        // Giới tính -> cboSex
        public string Sex
        {
            get => cboSex.Text.Trim();
            set => cboSex.Text = value;
        }

        // Số điện thoại lưu dạng +84901234567
        public string PhoneNumber
        {
            get
            {
                var prefix = cboPhoneCountryCode.SelectedItem != null
                    ? cboPhoneCountryCode.SelectedItem.ToString()
                    : "+84";

                var number = TxbCustomersNumberPhone.Text.Trim();
                return prefix + number;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    cboPhoneCountryCode.SelectedItem = "+84";
                    TxbCustomersNumberPhone.Text = string.Empty;
                    return;
                }

                // Tách mã quốc gia (bắt đầu bằng '+', tiếp theo là số) và phần còn lại
                var match = Regex.Match(value, @"^(\+[0-9]{1,3})([0-9]+)$");
                if (match.Success)
                {
                    var prefix = match.Groups[1].Value;
                    var number = match.Groups[2].Value;

                    // Chọn prefix trong combobox nếu có, nếu không thì thêm vào
                    int index = cboPhoneCountryCode.Items.IndexOf(prefix);
                    if (index >= 0)
                    {
                        cboPhoneCountryCode.SelectedIndex = index;
                    }
                    else
                    {
                        cboPhoneCountryCode.Items.Add(prefix);
                        cboPhoneCountryCode.SelectedItem = prefix;
                    }

                    TxbCustomersNumberPhone.Text = number;
                }
                else
                {
                    // Không đúng định dạng, gán tất cả vào textbox số, prefix mặc định +84
                    if (cboPhoneCountryCode.Items.Count > 0)
                        cboPhoneCountryCode.SelectedIndex = 0;
                    else
                        cboPhoneCountryCode.Items.Add("+84");

                    TxbCustomersNumberPhone.Text = value;
                }
            }
        }

        // ĐỊA CHỲ -> TxbAddress (textbox Địa chỉ)
        public string Address
        {
            get => TxbAddress.Text.Trim();
            set => TxbAddress.Text = value;
        }

        // EMAIL -> TxbCustomersGmail (textbox Email)
        public string Email
        {
            get => TxbCustomersGmail.Text.Trim();
            set => TxbCustomersGmail.Text = value;
        }

        public string CustomerType
        {
            get => cboCustomerType.Text.Trim();
            set => cboCustomerType.Text = value;
        }

        public string Nationality
        {
            get => cboQuocTich.Text.Trim();
            set => cboQuocTich.Text = value;
        }

        public CustomerAdd()
        {
            InitializeComponent();
        }

        private void InitializeCountryPhonePrefixes()
        {
            _countryPhonePrefixes = new Dictionary<string, string>
            {
                { "Việt Nam", "+84" },
                { "United States", "+1" },
                { "United Kingdom", "+44" },
                { "France", "+33" },
                { "Germany", "+49" },
                { "Japan", "+81" },
                { "South Korea", "+82" },
                { "China", "+86" },
                { "Taiwan", "+886" },
                { "Thailand", "+66" },
                { "Malaysia", "+60" },
                { "Singapore", "+65" },
                { "Philippines", "+63" },
                { "Laos", "+856" },
                { "Cambodia", "+855" },
                { "Australia", "+61" },
                { "India", "+91" },
            };

            cboQuocTich.Items.Clear();
            foreach (var country in _countryPhonePrefixes.Keys)
            {
                cboQuocTich.Items.Add(country);
            }

            // optional: default Viet Nam
            cboQuocTich.SelectedItem = "Việt Nam";
        }

        private void CustomerAdd_Load(object sender, EventArgs e)
        {
            InitializeCountryPhonePrefixes();

            // Khởi tạo default prefix nếu chưa có chọn
            if (cboPhoneCountryCode.Items.Count == 0)
            {
                foreach (var code in new HashSet<string>(_countryPhonePrefixes.Values))
                {
                    cboPhoneCountryCode.Items.Add(code);
                }
            }

            if (cboPhoneCountryCode.SelectedIndex < 0 && cboPhoneCountryCode.Items.Count > 0)
            {
                cboPhoneCountryCode.SelectedItem = "+84";
            }

            if (Mode == CustomerFormMode.Edit)
            {
                TxbCustomersID.ReadOnly = true;
                guna2HtmlLabel1.Text = "Cập nhật khách hàng";
                guna2Button1.Text = "Lưu";
            }
            else
            {
                TxbCustomersID.ReadOnly = true; // không cho nhập khi thêm
                TxbCustomersID.Text = string.Empty;
                guna2HtmlLabel1.Text = "Thêm khách hàng mới";
                guna2Button1.Text = "Thêm";
            }
        }

        private void cboQuocTich_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_countryPhonePrefixes == null) return;

            var selectedCountry = cboQuocTich.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedCountry)) return;

            string prefix;
            if (_countryPhonePrefixes.TryGetValue(selectedCountry, out prefix))
            {
                int index = cboPhoneCountryCode.Items.IndexOf(prefix);
                if (index >= 0)
                {
                    cboPhoneCountryCode.SelectedIndex = index;
                }
                else
                {
                    cboPhoneCountryCode.Items.Add(prefix);
                    cboPhoneCountryCode.SelectedItem = prefix;
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerName) ||
                string.IsNullOrWhiteSpace(TxbCustomersNumberPhone.Text.Trim()) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(CustomerType) ||
                string.IsNullOrWhiteSpace(CCCD) ||
                string.IsNullOrWhiteSpace(Sex))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng.",
                                "Thiếu thông tin",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Email không đúng định dạng.",
                                "Email không hợp lệ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                TxbCustomersGmail.Focus();
                TxbCustomersGmail.SelectAll();
                return;
            }

            if (!IsValidPhoneNumber(out var phoneError))
            {
                MessageBox.Show(phoneError,
                                "Số điện thoại không hợp lệ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                TxbCustomersNumberPhone.Focus();
                TxbCustomersNumberPhone.SelectAll();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void guna2TextBox3_TextChanged(object sender, EventArgs e) { }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(out string errorMessage)
        {
            errorMessage = null;

            var prefix = cboPhoneCountryCode.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = "+84";
                int defaultIndex = cboPhoneCountryCode.Items.IndexOf(prefix);
                if (defaultIndex >= 0)
                {
                    cboPhoneCountryCode.SelectedIndex = defaultIndex;
                }
            }

            if (!Regex.IsMatch(prefix ?? string.Empty, @"^\+\d{1,3}$"))
            {
                errorMessage = "Mã quốc gia không hợp lệ.";
                return false;
            }

            var localNumber = TxbCustomersNumberPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(localNumber))
            {
                errorMessage = "Vui lòng nhập số điện thoại.";
                return false;
            }

            if (!Regex.IsMatch(localNumber, @"^\d{7,12}$"))
            {
                errorMessage = "Số điện thoại chỉ được phép chứa chữ số và có độ dài 7-12 ký tự.";
                return false;
            }

            return true;
        }
    }
}
