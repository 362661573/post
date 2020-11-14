using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public delegate void ChangeFormColor(bool topmost);
    
    public partial class Form2 : Form
    {
        public static Hashtable table = new Hashtable();
        public Form2()
        {
            InitializeComponent();
        }
        public event ChangeFormColor ChangeColor;
        private void button1_Click(object sender, EventArgs e)
        {
            table.Add("xiaomao", "xiaogou");
            string s = (string)table["xiaomao"];
            this.Text = s;
            ChangeColor(true);//执行委托实例
            
        }
    }

}
