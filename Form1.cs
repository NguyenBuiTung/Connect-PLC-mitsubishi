using System;
using System.Windows.Forms;
using ActUtlTypeLib; // Thư viện cần thiết để làm việc với PLC Mitsubishi

namespace Connect_With_PLC_Mitsubishi
{
    public partial class Form1 : Form
    {
        public ActUtlType plc = new ActUtlType(); // Đối tượng ActUtlType để giao tiếp với PLC Mitsubishi

        public Form1()
        {
            InitializeComponent(); // Khởi tạo các thành phần giao diện người dùng
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Đây là sự kiện tải form. Bạn có thể thêm mã khởi tạo ở đây nếu cần.
        }
        private bool IsConnected()
        {
            int testValue;
            int result = plc.GetDevice("D0", out testValue); // Đọc dữ liệu từ địa chỉ "D0" để kiểm tra kết nối

            // Nếu phương thức GetDevice trả về 0, kết nối còn hoạt động; ngược lại, kết nối đã bị ngắt
            return result == 0;
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            plc.ActLogicalStationNumber = 2; // Thiết lập số trạm logic của PLC (ví dụ: 2)
            short connectResult = (short)plc.Open(); // Mở kết nối với PLC
            if (connectResult == 0)
            {
                MessageBox.Show("Connect with PLC Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Connect with PLC Error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                plc.Close(); // Đóng kết nối với PLC nếu đang kết nối
                MessageBox.Show("Disconnected from PLC", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("PLC is not connected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (!IsConnected()) // Kiểm tra kết nối trước khi đọc dữ liệu
            {
                MessageBox.Show("PLC is not connected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int readData = 0; // Biến để lưu dữ liệu đọc được
            int readResult = plc.GetDevice("D100", out readData); // Đọc dữ liệu từ thiết bị PLC, ví dụ từ địa chỉ "D100"
            if (readResult == 0)
            {
                txtData.Text = readData.ToString(); // Hiển thị dữ liệu đọc được trong ô văn bản
                MessageBox.Show("Read Success!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Read Error!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (!IsConnected()) // Kiểm tra kết nối trước khi ghi dữ liệu
            {
                MessageBox.Show("PLC is not connected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int writeData;
            if (int.TryParse(txtData.Text, out writeData))
            {
                int writeResult = plc.SetDevice("D100", writeData); // Ghi dữ liệu vào thiết bị PLC, ví dụ vào địa chỉ "D100"
                if (writeResult == 0)
                {
                    MessageBox.Show("Write Success!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Write Error!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid Data!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
