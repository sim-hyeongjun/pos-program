using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
        }

        //콤포박스 값 추가
        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("사원");
            comboBox1.Items.Add("매니저");
        }
        //취소버튼
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login showForm1 = new Login();
            showForm1.ShowDialog();
        }
        //회원가입 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                string constring = "Server=localhost;Port=3306;Database = pos_database;Uid=root;Pwd=0000";
                string Query ="INSERT INTO pos_database.user_tb (id, password, name, position) value ('" 
                    + this.textBox1.Text + "','" + this.textBox2.Text + "','" + this.textBox3.Text + "','" + this.comboBox1.Text + "')";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("계정 생성 완료");

                    while (myReader.Read())
                    {

                    }
                    this.Visible = false;
                    Login showForm1 = new Login();
                    showForm1.ShowDialog();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("항목을 정확히 입력해주세요");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
