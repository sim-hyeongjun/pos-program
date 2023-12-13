using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        DataTable table = new DataTable();

        public Form1()
        {
            InitializeComponent();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Count", typeof(string));
            table.Columns.Add("Totla", typeof(string));

            dataGridView1.DataSource = table;
            numericUpDown1.Value = 1;

            if (Login.chk == "0")
            {
                textBox4.Text = "매니저님 반갑습니다.";
                // 직원목록과 판매내역은 매니저만 보기 가능
                users.Visible = true;
                button3.Visible = true;
            }
            else
            {
                textBox4.Text = "사원님 반갑습니다.";
                users.Visible = false;
                button3.Visible = false;
            }
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void server(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=0000;");
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SERVER.Text = "Connected";
                SERVER.ForeColor = Color.Black;
            }
            else
            {
                SERVER.Text = "DisConnected";
                SERVER.ForeColor = Color.Red;
            }
        }
        //계산하기 기능
        private void 계산_Click(object sender, EventArgs e)
        {
            //데이터베이스 연결 후 데이터 전송
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=pos_database;Uid=root;Pwd=0000"))
            {
                conn.Open();
                //각행의 정보를 반복문으로 불러온다.
                for(int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string Price = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string Count = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    string Total = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    // inser 쿼리문으로 받아온 정보를 db에 전송
                    string sql = string.Format("INSERT INTO sales_db(name,price,count,total,c_num) VALUES ('{0}',{1},{2},{3},{4})", @Name, @Price, @Count, @Total, @i);

                    // 위 쿼리문으로 이름을 찾고 i_count의 수량에서 판매된  개수만큼 빼준다.
                    string sql_count = string.Format("update item_tb set i_count = i_count - {0} where i_name = '{1}'", @Count, @Name);

                    try
                    {
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.ExecuteNonQuery();

                        MySqlCommand c_command = new MySqlCommand(sql_count, conn);
                        c_command.ExecuteNonQuery();

                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            MessageBox.Show("계산되었습니다.");
            //데이터 그리드뷰 초기화
            int rowCount = dataGridView1.Rows.Count;
            for (int n = 0; n < rowCount; n++)
            {
                if (dataGridView1.Rows[0].IsNewRow == false)
                    dataGridView1.Rows.RemoveAt(0);
            }
            //합계창 초기화
            textBox3.Text = "0";
        }

        //취소 기능
        private void 취소_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                //RemoveAt = 인덱스에 해당하는 행 데이터 삭제
                dataGridView1.Rows.RemoveAt(item.Index);
            }

            decimal all = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }
            textBox3.Text = all.ToString();

        }

        private void 담기_Click(object sender, EventArgs e)
        {
            // textBox1 = 상품명 , textBox2 = 가격
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox1.Clear(); //상품명 입력칸 리셋
                textBox2.Clear(); //가격 입력칸 리셋
            }
            else
            {
                decimal price = decimal.Parse(textBox2.Text);
                decimal count = numericUpDown1.Value;
                decimal total = price * count;

                // 데이터 화면에 추가
                table.Rows.Add(textBox1.Text, textBox2.Text, numericUpDown1.Value, total);
                dataGridView1.DataSource  = table;

                textBox1.Clear();
                textBox2.Clear();

                //모든 가격 합계
                decimal all = 0;
                for(int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //cell 은 열은 뜻하는듯
                    all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                }
                textBox3.Text = all.ToString();


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            dlg.ShowDialog();
        }

        // 재고 현황 창 열기
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 dlg = new Form3();
            dlg.ShowDialog();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login dlg = new Login();
            dlg.ShowDialog();
        }
        
    }
}
