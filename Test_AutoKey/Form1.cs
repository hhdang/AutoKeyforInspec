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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = RUN_WAIT;
            lblInfo.Text = string.Format( "{0}/{1} {2}", codeIndex+1, codeList.Count, codeList[codeIndex] );
            
            string code = codeList[codeIndex];
            int split = code.IndexOf( ':' );
            string codeType = "";
            string codeContent = "";

            if (0 < split)
            {
                codeType = code.Substring(0, split);
                codeType = code.Substring(split + 1);
            }
            else {
                codeType = code;
            }

            codeType = codeType.Trim().ToUpper();
            if( Enum.IsDefined( typeof(CmdType), codeType ) ){
            
            }
        }



    }

    #region 命令类型
    enum CmdType
    {
        /// <summary>
        /// 输入
        /// </summary>
        INPUT,
        /// <summary>
        /// 运行
        /// </summary>
        RUN,
        /// <summary>
        /// 按键
        /// </summary>
        KEY,
        /// <summary>
        /// 暂停
        /// </summary>
        SLEEP,
        /// <summary>
        /// 鼠标移动
        /// </summary>
        MOUSE_MOVE,
        /// <summary>
        /// 鼠标单击
        /// </summary>
        MOUSE_CLICK,
        /// <summary>
        /// 鼠标双击
        /// </summary>
        MOUSE_DBCLICK,
        /// <summary>
        /// 截屏
        /// </summary>
        SCREEN,
        /// <summary>
        /// 截全屏
        /// </summary>
        ALL_SCREEN
    }
    #endregion

}
