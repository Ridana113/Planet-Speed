using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace попытка
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        public const double G = 0.00000000006743014;
        public static double V1;
        public static double V2;
        public static double V3;
        public static double M;
        public static double R;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Darina\source\repos\попытка\попытка\Database.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connetionString);
            await sqlConnection.OpenAsync(); // открытие асинхроно позволит не тормозить пользовательский интервейс
            SqlDataReader sqlReader = null; // представляет БД в виде таблицы
            SqlCommand comand = new SqlCommand("SELECT * FROM [SPACE_BD]", sqlConnection); // запрос
            try
            {
                sqlReader = await comand.ExecuteReaderAsync(); //считывает таблицу
                while (await sqlReader.ReadAsync()) //возвращает к следующей записи
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + " " + Convert.ToString(sqlReader["Name"]) + "     " + Convert.ToString(sqlReader["Mass"]) + "      " + Convert.ToString(sqlReader["Space_Speed1"]) + "     " + Convert.ToString(sqlReader["Space_Speed2"]) + "     " + Convert.ToString(sqlReader["Space_Speed3"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private async void Insert_Click(object sender, EventArgs e)
        {
            if(label11.Visible)
                label11.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text)&& !string.IsNullOrWhiteSpace(textBox1.Text)&&
               !string.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox7.Text) &&
               !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
               !string.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox10.Text) &&
               !string.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrWhiteSpace(textBox9.Text) &&
               !string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox8.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [SPACE_BD] (Name, Mass, Radius, Space_Speed1, Space_Speed2, Space_Speed3) VALUES(@Name, @Mass, @Radius, @Space_Speed1, @Space_Speed2, @Space_Speed3)", sqlConnection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Mass", textBox7.Text);
                command.Parameters.AddWithValue("Radius", textBox2.Text);
                command.Parameters.AddWithValue("Space_Speed1", textBox10.Text);
                command.Parameters.AddWithValue("Space_Speed2", textBox9.Text);
                command.Parameters.AddWithValue("Space_Speed3", textBox8.Text);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                label11.Visible = true;
                label11.Text = "!!! ВСЕ ПОЛЯ ДОЛЖНЫ БЫТЬ ЗАПОЛНЕНЫ !!!";
            }
            
        }

        private async void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            SqlDataReader sqlReader = null; 
            SqlCommand comand = new SqlCommand("SELECT * FROM [SPACE_BD]", sqlConnection); 
            try
            {
                sqlReader = await comand.ExecuteReaderAsync(); 
                while (await sqlReader.ReadAsync()) 
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + " " + Convert.ToString(sqlReader["Name"]) + "     " + Convert.ToString(sqlReader["Mass"]) + "      " + Convert.ToString(sqlReader["Space_Speed1"]) + "     " + Convert.ToString(sqlReader["Space_Speed2"]) + "     " + Convert.ToString(sqlReader["Space_Speed3"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            if (label3.Visible)
                label3.Visible = false;
            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                SqlCommand command = new SqlCommand("DELETE FROM [SPACE_BD] WHERE [Id] = @Id", sqlConnection);
                command.Parameters.AddWithValue("Id", textBox6.Text);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                label3.Visible = true;
                label3.Text = "!!! ЗАПОЛНИТЕ ПОЛЕ !!!";
            }
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                R = Convert.ToDouble(textBox2.Text);  //начало расчета скоростей
                M = Convert.ToDouble(textBox7.Text);
                if (R < 0)
                {
                    MessageBox.Show("Уважаемый пользователь, пожалуйста, введите скорость Ахиллеса в виде положительного числа");
                }
                if (M < 0)
                {
                    MessageBox.Show("Уважаемый пользователь, пожалуйста, введите скорость черепахи в виде положительного числа");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Уважаемый пользователь, пожалуйста, введите ЧИСЛО!");
                textBox2.Select();
                textBox7.Select();
                return;
            }
            V1 = (G * M) / R;
            V1 = Math.Sqrt(V1);
            V2 = V1 * Math.Sqrt(2);
            V3 = (Math.Sqrt(2) - 1) * (Math.Sqrt(2) - 1) * V1 * V1 + (V2 * V2);
            V3 = Math.Sqrt(V3);
            textBox10.Text = V1.ToString();
            textBox9.Text = V2.ToString();
            textBox8.Text = V3.ToString();
        }
    }
    }

