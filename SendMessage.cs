  
첫번째 Form1 의 소스 (SendMessage를 받는  Form)

WindowTitle을 Form1으로 되어 있어야 함.



public const int WM_COPYDATA = 0x4A;
[DllImport("User32.dll")]
public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

[DllImport("user32.dll")]
public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

public struct COPYDATASTRUCT
{
      public IntPtr dwData;
      public int cbData;
      [MarshalAs(UnmanagedType.LPStr)]
      public string lpData;
}


///SendMessage Receive
protected override void WndProc(ref Message m)
{
     try
     {
         switch (m.Msg)
                {
                    case WM_COPYDATA:
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                        byte[] buff = System.Text.Encoding.Default.GetBytes(cds.lpData);
                        
                        COPYDATASTRUCT cs = new COPYDATASTRUCT();
                        cs.dwData = new IntPtr(0);
                        cs.cbData = buff.Length;
                        cs.lpData = cds.lpData;
                        
                        // 다시 보낼 Form2의 windows 헨들을 가져 온다.
                        IntPtr hwnd = FindWindow(null, "Form2");
                        SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref cs);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
         catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
 }





두번째 Form2 (SendMessage를 보내는 쪽)






        public const int WM_COPYDATA = 0x4A;
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = this.tbMsg.Text.Trim();
            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("메세지를 입력해주세요");
                return;
            }

            IntPtr hwnd = FindWindow(null, "Fom1");
            byte[] buff = System.Text.Encoding.Default.GetBytes(msg);
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = new IntPtr(1001);
            cds.cbData = buff.Length + 1;
            cds.lpData = msg;
            SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref cds);
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM_COPYDATA:
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                        MessageBox.Show(cds.lpData);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


예외 처리사항을 무시하고 간단하게 메시지 처리하는 기능만 구현.

추가적인 기능들은 개인 입맛에 맞게 수정하면 될듯함.

