using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MangaGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
            
        }

        public void LoadData()
        {
            string data;
            var fileStream = new FileStream(@"C:\Users\%USERNAME%\Desktop\Manga.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                data = streamReader.ReadToEnd();
            }

            string[] url = data.Split('#');

            for (int i = 0; i < url.Length - 1; i++)
            {
                string Url = url[i];
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(Url);

                string[] name = Url.Split('/');
                string last = doc.DocumentNode.SelectNodes("//*[@id=\"chapters\"]/ul/li[1]/div/h3/a")[0].InnerText;
                string[] picture = doc.DocumentNode.SelectNodes("//*[@id=\"series_info\"]/div[1]/img")[0].OuterHtml.Split('"');
                string date = doc.DocumentNode.SelectNodes("//*[@id=\"chapters\"]/ul/li[1]/div/span")[0].InnerText;
                

                last = Regex.Replace(last, "[^0-9]+", string.Empty);

                dataGridView1.Rows.Add();
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i];
                row.Cells[0].Value = (name[4].Replace("_", " "));
                //row.Cells[1].Value =
                row.Cells[2].Value = last;
                row.Cells[3].Value = date;
                row.Cells[4].Value = picture[3];

                if (i == 0)
                {
                    textBox1.Text = (name[4].Replace("_", " "));
                    //textBox2.Text = 
                    textBox3.Text = last;
                    textBox4.Text = date;
                    pictureBox1.Load(picture[3]);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var test = e.RowIndex;
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[test];
            textBox1.Text = row.Cells[0].Value.ToString();
            //textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            pictureBox1.Load(row.Cells[4].Value.ToString());

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
            string link = textBox1.Text.Replace(" ", "_");
            Process.Start("http://m.mangafox.me/manga/" + link);
        }
    }

}    
