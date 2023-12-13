using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public static string chk;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("사원");
            comboBox1.Items.Add("매니저");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnention = "Server=localhost; Port=3306; Database = pos_database;Uid=root;Pwd=0000";
                MySqlConnection myConn = new MySqlConnection(myConnention);
                

                // 각항목을 db와 대조
                MySqlCommand selectCommand = new MySqlCommand("select * from pos_database.user_tb where id ='" + this.textBox1.Text + "' and password='" + this.textBox2.Text + "' and position='" + this.comboBox1.Text + "'", myConn);
                MySqlDataReader myReader;

                myConn.Open();
                myReader = selectCommand.ExecuteReader();

                int count = 0;

                while (myReader.Read())
                {
                    count = count + 1;
                }
                if (count == 1)
                {
                    if (comboBox1.Text == "매니저")
                    {
                        chk = "0";
                    }
                    else
                    {
                        chk = "1";
                    }

                    MessageBox.Show("로그인 되었습니다.");
                    this.Visible = false;
                    Form1 showForm1 = new Form1();
                    showForm1.ShowDialog();
                }
                else if (count > 1)
                {
                    MessageBox.Show("중복된 유저가 존재합니다.");
                }
                else
                {
                    MessageBox.Show("아이디,비밀번호나 직책이 일치하지 않습니다.");
                }
                myConn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Account dlg = new Account();
            dlg.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
