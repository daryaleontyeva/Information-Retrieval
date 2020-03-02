using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace InformationResearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=postgres;Password=Ira1234567890;Database=InfResearch;");
            conn.Open();
            string name = textBox1.Text;
            string year = "";
            name += " ";
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsDigit(name[i]))
                    if (year.Length <= 4)
                        if (name[i + 1] != null)
                            if (char.IsDigit(name[i + 1]) || year.Length == 3)
                                year += name[i];
                            else
                                year = null;
            }
            if (!string.IsNullOrEmpty(year))
            {
                name = name.Replace(year, "");
                name = name.Replace(" ", "");
            }
            string name1 = "%" + name + "%";
            string input = name1.Replace(" ", "% %");
            NpgsqlParameter nameParam = new NpgsqlParameter("@input", input);
            NpgsqlParameter yearParam = new NpgsqlParameter("@year", year);
            string query = "";
            if (string.IsNullOrEmpty(year))
            {
                query = "select * from movies WHERE name like @input LIMIT 10";
            }
            else if (string.IsNullOrEmpty(input))
            {
                query = "select * from movies WHERE cast(year as text) = @year LIMIT 10";
            }
            else
            {
                query = "select * from movies WHERE cast(year as text) = @year and name like @input LIMIT 10";
            }
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            if (string.IsNullOrEmpty(year))
                command.Parameters.Add(nameParam);
            else if (string.IsNullOrEmpty(input))
                command.Parameters.Add(yearParam);
            else
            {
                command.Parameters.Add(yearParam);
                command.Parameters.Add(nameParam);
            }
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    int rowNumber = dataGridView1.Rows.Add();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dataGridView1.Rows[rowNumber].Cells["ID"].Value = dr[0];
                        dataGridView1.Rows[rowNumber].Cells["name"].Value = dr[1];
                        dataGridView1.Rows[rowNumber].Cells["year"].Value = dr[2];
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
