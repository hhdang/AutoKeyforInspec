using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_AutoKey
{
    public partial class AutoKey : Form
    {
        private IList<string> codeList = null;
        private int codeIndex = 0;
        private int StartIndex = 3;
        private int RUN_WAIT = 100;

        public AutoKey()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            if( "" == txtCode.Text ){
                lblInfo.Text = "无执行指令";
                return;
            }

            codeList = new List<string>();
            codeIndex = 0;
            foreach( string r in txtCode.Text.Split('\r') ){
                if( "" != r.Trim() ){
                    codeList.Add( r.Trim() );
                }
            }

            StartIndex = 3;
            lblInfo.Text = StartIndex.ToString() + "秒后开始";
            timer2.Start();
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            StartIndex--;
            lblInfo.Text = StartIndex.ToString() + "秒后开始";

            if( 0 >= StartIndex ){
                timer2.Stop();
                timer1.Interval = RUN_WAIT;
                timer1.Start();

            }
        }



    }
}
