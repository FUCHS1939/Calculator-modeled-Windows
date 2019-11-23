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
        private double? op1 = null;
        private int Operator;
        private bool isresult = true;
        private string memory;
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
            if(isresult)
            {
                itisbignum.Text = "";
                isresult = false;
            }
            if (itisbignum.Text != "")
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
        /// CE按键的点击事件，清空当前输入框内内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CE_Button_Click(object sender, RoutedEventArgs e)
        {
            itisbignum.Text = "";
            isresult = false;
        }

        /// <summary>
        /// C按键的点击事件，清空输入框内容和所有输入存储数据，包括为清算的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void C_Button_Click(object sender, RoutedEventArgs e)
        {
            itisbignum.Text = "";
            op1 = null;
            Operator = 0;
            itissmallnum.Text = "";
            isresult = false;
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
            //设置输入显示框为输入状态
            if (isresult)
            {
                isresult = false;
                itisbignum.Text = "";
            }
            int count = 0;
            string str;
            str = (string)((Button)sender).Content;
            //判断输入显示框为空时的动作
            if (itisbignum.Text == "")
            {
                itisbignum.Text = (str == ".") ? "0." : str;
            }
            //当不为空时
            else
            {

                if (str == ".")
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
                    if (itisbignum.Text.Length <= 10) itisbignum.FontSize = 64;
                    else if (itisbignum.Text.Length <= 11) itisbignum.FontSize = 55;
                    else if (itisbignum.Text.Length <= 12) itisbignum.FontSize = 50;
                    else if (itisbignum.Text.Length <= 13) itisbignum.FontSize = 45;
                    else if (itisbignum.Text.Length <= 14) itisbignum.FontSize = 42;
                    else if (itisbignum.Text.Length <= 15) itisbignum.FontSize = 39;
                    else if (itisbignum.Text.Length <= 16) itisbignum.FontSize = 36;
                    else itisbignum.FontSize = 32;

                    if (count < 16)
                    {
                        //按下按键后开始计算数位，添加千位符，如果不想添加符号，直接使用下面语句。。。。
                        //itisbignum.Text += (string)((Button)sender).Content；
                        //需要继续优化，输入小数点后本来该不再需要划分千分位，目前是每输入一个键则计算一次千分位，资源浪费
                        str = itisbignum.Text.Replace(",", "") + str;
                        string[] temp = str.Split('.');
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
            }
        }

        /// <summary>
        /// 运算符功能（不含等号）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperatorButtonClicked(object sender, EventArgs e)
        {
            //将点击的运算符转化为UTF32 
            //“+”59720   "-"59721  "*" 59719  "/" 59722
            int i = char.ConvertToUtf32((string)((Button)sender).Content, 0);
            //当输入符号时，将显示框内字符串转化为操作数
            //1.当第一操作数为空的时候
            if (op1 == null)
            {
                if (itisbignum.Text == "")
                {
                    op1 = 0;
                    itissmallnum.Text = "0" + ((i != 59720) ? ((i != 59721) ? ((i != 59722) ? "*" : "/") : "-") : "+");
                    Operator = i;
                }
                else
                {
                    op1 = Convert.ToDouble(itisbignum.Text.Replace(",", ""));
                    Operator = i;
                    itissmallnum.Text = itisbignum.Text.Replace(",", "") +
                        ((i != 59720) ? ((i != 59721) ? ((i != 59722) ? "*" : "/") : "-") : "+");
                    itisbignum.Text = "";

                }
            }
            //2.当第一个操作数不为空的时候
            else
            {
                if (itisbignum.Text == "" || isresult)
                {
                    if (!char.IsDigit(itissmallnum.Text[itissmallnum.Text.Length - 1]))
                    {
                        itissmallnum.Text = itissmallnum.Text.Substring(0, itissmallnum.Text.Length - 1);
                    }
                    itissmallnum.Text = itissmallnum.Text +
                        ((i != 59720) ? ((i != 59721) ? ((i != 59722) ? "*" : "/") : "-") : "+");
                    Operator = i;
                }
                else
                {
                    double j = Convert.ToDouble(itisbignum.Text.Replace(",", ""));
                    switch (Operator)
                    {
                        case 59720: op1 = op1 + j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "+"; break;
                        case 59721: op1 = op1 - j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "-"; break;
                        case 59719: op1 = op1 * j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "*"; break;
                        case 59722: op1 = op1 / j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "/"; break;
                        //我们受过专业训练，无论怎么样呢，我们都不会出现这种情况，除非有bug
                        default: break;
                    }
                    //设置输入显示框为输出结果并将其状态置为非输入状态
                    itisbignum.Text = op1.ToString();
                    isresult = true;
                    Operator = i;
                }
            }
        }

        /// <summary>
        /// 使用了等号的情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Equal_Button_Click(object sender, RoutedEventArgs e)
        {
            string str;
            
            if (op1 == null)
            {
                if (itisbignum.Text == "")
                {
                    op1 = 0;
                    itissmallnum.Text = "0";
                }
                else
                {
                    op1 = Convert.ToDouble(itisbignum.Text.Replace(",", ""));
                    itissmallnum.Text = itisbignum.Text.Replace(",", "");
                }
            }
            else
            {
                if(itisbignum.Text == "")
                {
                    if (!char.IsDigit(itissmallnum.Text[itissmallnum.Text.Length - 1]))
                    {
                        itissmallnum.Text = itissmallnum.Text.Substring(0, itissmallnum.Text.Length - 1);
                    }
                }
                else
                {
                    double j = Convert.ToDouble(itisbignum.Text.Replace(",", ""));
                    switch (Operator)
                    {
                        case 59720: op1 = op1 + j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "+"; break;
                        case 59721: op1 = op1 - j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "-"; break;
                        case 59719: op1 = op1 * j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "*"; break;
                        case 59722: op1 = op1 / j; itissmallnum.Text = itissmallnum.Text + itisbignum.Text.Replace(",", "") + "/"; break;
                        default: op1 = j;itissmallnum.Text = itisbignum.Text.Replace(",", ""); break;
                    }
                }
                  
            }
            str = itissmallnum.Text +" =\n" + op1.ToString();
            if (!char.IsDigit(str[str.Length - 1]))
            {
                str = str.Substring(0, str.Length - 1);
            }
            //设置输入显示框为输出结果并将其状态置为非输入状态
            itisbignum.Text = op1.ToString();
            isresult = true;
            TextBlock AtextBlock = new TextBlock();
            AtextBlock.Text = str;
            AtextBlock.TextWrapping = TextWrapping.Wrap;
            AtextBlock.Width = 246;
            AtextBlock.TextAlignment = TextAlignment.Right;
            History.Items.Add(AtextBlock);
        }

        /// <summary>
        /// 取负数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NegativeNumber_Button_Click(object sender, RoutedEventArgs e)
        {
            if(itisbignum.Text != "")
            {
                double i = Convert.ToDouble(itisbignum.Text.Replace(",", ""));
                i = i * (-1);
                string str = i.ToString();
                string[] temp = str.Split('.');
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
    }
}
