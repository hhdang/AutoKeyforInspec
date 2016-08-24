using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Test_AutoKey
{
    public partial class AutoKey : Form
    {
        private IList<string> codeList = null;
        private int codeIndex = 0;
        private int StartIndex = 3;
        private int RUN_WAIT = 100;

        Stage stage = Stage.START;




        public AutoKey()
        {
            InitializeComponent();
            pushStage( stage );
        }

        private void pushStage( Stage stg )
        {
            switch( stg ){
                case Stage.START:
                    if( "" == txtCode.Text ){
                        lblInfo.Text = "无执行指令";
                        return;
                    }

                    codeList = new List<string>();
                    codeIndex = 0;
                    foreach( string r in txtCode.Text.Split('\n') ){
                        if( "" != r.Trim() ){
                            codeList.Add( r.Trim() );
                        }
                    }

                    StartIndex = 3;
                    lblInfo.Text = StartIndex.ToString() + "秒后开始";
                    timer2.Start();
                    stage = Stage.STOP;
                    break;
                case Stage.STOP:
                    timer2.Stop();
                    timer1.Stop();
                    lblInfo.Text = "";
                    stage = Stage.EXIT;
                    break;

                default:
                    break;

            }

            
            
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
                codeContent = code.Substring(split + 1);
            }
            else {
                codeType = code;
            }

            codeType = codeType.Trim().ToUpper();
            if( Enum.IsDefined( typeof(CmdType), codeType ) ){
                try { 
                    CmdType cmdType = (CmdType)Enum.Parse( typeof(CmdType), codeType );
                    switch ( cmdType ){
                        case CmdType.INPUT:
                            cmdInput( codeContent );
                            break;
                        case CmdType.RUN:
                            cmdRun( codeContent );
                            break;
                        case CmdType.KEY:
                            cmdKey( codeContent );
                            break;
                        case CmdType.SLEEP:
                            cmdSleep( codeContent );
                            break;
                        default:
                            break;
                    }
                }catch( Exception ex ){
                    MessageBox.Show( "运行[" + code + "]时失败！\r\n\r\n错误原因：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }

            codeIndex++;
            if (codeList.Count <= codeIndex)
            {
                pushStage( stage );
            }
            else {
                timer1.Start();
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

        enum Stage { 
            START,
            STOP,
            EXIT
        }
        #endregion

        #region 命令
        private void cmdInput(string str)
        {
            Clipboard.Clear();
            Clipboard.SetText( str );
            SendKeys.SendWait( "^v" );
        }
        private void cmdRun(string str)
        {
            Process.Start( str );
        }
        private void cmdKey(string str)
        {
            SendKeys.SendWait( str );
        }
        private void cmdSleep(string str)
        {
            int t = 0;
            if( int.TryParse( str, out t ) ){
                if( t > RUN_WAIT ){
                    timer1.Interval = t;
                }
            }
        }
        #endregion




    }

}
