using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator_modeled_Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private double temp, op1, op2, memory, result;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 用于删除输入的数字的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if(itisbignum.Text != "")
            {
                //若不添加千分位，则直接使用：
                //itisbignum.Text = itisbignum.Text.Substring(0, itisbignum.Text.Length - 1);
                string str = itisbignum.Text.Replace(",", "");
                string[] temp = str.Substring(0, str.Length - 1).Split('.');
                if (temp.Length == 2)
                {
                    itisbignum.Text = System.Text.RegularExpressions.Regex.Replace(temp[0], @"(\d{3})", ",$1", System.Text.RegularExpressions.RegexOptions.RightToLeft).Trim(',') + "." + temp[1];
                }
                else
                {
                    itisbignum.Text = System.Text.RegularExpressions.Regex.Replace(temp[0], @"(\d{3})", ",$1", System.Text.RegularExpressions.RegexOptions.RightToLeft).Trim(',');
                }
            }
        }

        /// <summary>
        /// 用于清除历史记录的时间函数，按钮位置位于界面右下角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteHistory_Button_Click(object sender, RoutedEventArgs e)
        {
            History.Items.Clear();
        }

        /// <summary>
        /// 按下数字和小数点按键时候的事件函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumButtonClicked(object sender, EventArgs e)
        {
            int count = 0;
            string str;
            str = (string)((Button)sender).Content;
            //判断是否为小数点，是则前置0
            if (itisbignum.Text == "")
            {
                itisbignum.Text = (str == ".") ? "0." : str;
            }
            else
            {
                
                if(str == ".")
                {
                    if (!itisbignum.Text.Contains("."))
                    {
                        itisbignum.Text += str;
                    }
                }
                //如果不是小数点，直接加入数字
                else
                {
                    //检查数字位数，反正微软计算器按到最多就16个数字，就设数字个数不超过16好了
                    //超过后按键怎么按都是无效的
                    for (int i = 0; i < itisbignum.Text.Length; i++)
                    {
                        count += (Char.IsDigit(itisbignum.Text[i])) ? 1 : 0;
                    }

                    //为使Textbox可以显示所有的数字调节字体大小
                    if (count <= 8) itisbignum.FontSize = 64;
                    else if (count <= 9) itisbignum.FontSize = 55;
                    else if (count <= 10) itisbignum.FontSize = 50;
                    else if (count <= 11) itisbignum.FontSize = 45;
                    else if (count <= 12) itisbignum.FontSize = 42;
                    else if (count <= 13) itisbignum.FontSize = 39;
                    else if (count <= 14) itisbignum.FontSize = 36;
                    else itisbignum.FontSize = 34;

                    if (count < 16)
                    {
                        //按下按键后开始计算数位，添加千位符，如果不想添加符号，直接使用下面语句。。。。
                        //itisbignum.Text += (string)((Button)sender).Content；
                        //需要继续优化，输入小数点后本来该不再需要划分千分位，目前是每输入一个键则计算一次千分位，资源浪费
                        str = itisbignum.Text.Replace(",", "") + str;
                        string[] temp = str.Split('.');
                        if (temp.Length == 2)
                        {
                            itisbignum.Text = System.Text.RegularExpressions.Regex.Replace(temp[0], @"(\d{3})", ",$1", System.Text.RegularExpressions.RegexOptions.RightToLeft).Trim(',') + "." +temp[1];
                        }
                        else
                        {
                            itisbignum.Text = System.Text.RegularExpressions.Regex.Replace(temp[0], @"(\d{3})", ",$1", System.Text.RegularExpressions.RegexOptions.RightToLeft).Trim(',');
                        }
                    }
                }
            }
        }

    }
}
