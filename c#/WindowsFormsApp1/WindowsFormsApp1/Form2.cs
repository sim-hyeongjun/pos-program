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

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        MySqlConnection connection = new MySqlConnection("Server = localhost; Port=3306;Database = pos_database;Uid=root;Pwd=0000");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();
        int selectedRow;
        public Form2()
        {
            InitializeComponent();
        }

        public void searchData(string valueToSearch)
        {   
            string query = "SELECT * FROM sales_db WHERE CONCAT(`name`, `price`, `count`, `total`) like '%" + valueToSearch + "%'";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
        public void LoadData()
        {
            string sql = "Server=localhost;Port=3306;Database = pos_database;Uid=root;Pwd=0000";
            MySqlConnection con = new MySqlConnection(sql);
            MySqlCommand cmd_db = new MySqlCommand("SELECT * FROM sales_db;",con);
           

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd_db;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            searchData("");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            textBox7.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        //검색 기능
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("검색정보를 입력해주세요");
            }
            else
            {
                string valueToSearch = textBox1.Text.ToString();
                searchData(valueToSearch);

                //텍스트 박스 초기화
                textBox1.Text = "";
            }
        }
        // 수정기능
        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database = pos_database;Uid=root;Pwd=0000";

            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해 주세요");
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            else
            {
                // 변경할 품목의 기존 데이터를 변수로 가져온다
                decimal price = decimal.Parse(textBox3.Text);
                decimal count = decimal.Parse(textBox4.Text);
                decimal total = price * count;

                
                textBox5.Text = total.ToString();
                // 업데이트된 정보를 db에 전송 no컬럼을 이용해 인덱스 탐색
                string Query = "update pos_database.sales_db set no ='" + this.textBox7.Text + "',name='" + this.textBox2.Text + "',price='" + this.textBox3.Text
                    + "',count='" + this.textBox4.Text + "',total='" + this.textBox5.Text + "'where no ='" + this.textBox7.Text + "'";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try{
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("수정완료");

                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }

        //행 데이터 삭제
        private void button3_Click(object sender, EventArgs e)
        {
            String constring  = "Server=localhost;Port=3306;Database = pos_database;Uid=root;Pwd=0000";
            if (textBox7.Text == "")
            {
                MessageBox.Show("삭제할 항목을 찾지 못했습니다.");
            }
            else
            {
                string Query = "delete from pos_database.sales_db where no ='" + this.textBox7.Text + "';";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDataBase = new MySqlCommand(Query,conDataBase);
                MySqlDataReader myReader;
                try
                {
                    conDataBase.Open();
                    myReader = cmdDataBase.ExecuteReader();
                    MessageBox.Show("삭제완료");
                    while (myReader.Read())
                    {

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                LoadData() ;
            }
        }
    }
}
