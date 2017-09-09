using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MangaGrid
{
    public partial class Form1 : Form
    {
        private string[] dataSplit;
        private string[] url;
        private string[] read;

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            string data;
            var fileStream = new FileStream(@"C:\Users\Jacob\Desktop\Manga.txt", FileMode.OpenOrCreate, FileAccess.Read);
            if (fileStream.Length > 0)
            {
                File.Copy(@"C:\Users\Jacob\Desktop\Manga.txt", @"C:\Users\Jacob\Desktop\MangaBackUp.txt", true);
            }
            var fileStreamBackUp = new FileStream(@"C:\Users\Jacob\Desktop\MangaBackUp.txt", FileMode.OpenOrCreate, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                data = streamReader.ReadToEnd();
            }

            dataSplit = data.Split('#');
            url = new string[dataSplit.Length];
            read = new string[dataSplit.Length];

            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string[] split = dataSplit[i].Split('|');
                url[i] = split[0];
                read[i] = split[1];
            }

            for (int i = 0; i < dataSplit.Length - 1; i++)
            {
                string Url;
                HtmlWeb web;
                HtmlAgilityPack.HtmlDocument doc;

                string[] nameA;
                string last;
                string[] pictureA;
                string date;
                string name;
                string picture;

                Url = url[i];
                web = new HtmlWeb();
                doc = web.Load(Url);

                nameA = Url.Split('/');
                last = doc.DocumentNode.SelectNodes("//*[@id=\"chapters\"]/ul/li[1]/div/h3/a")[0].InnerText;
                last = Regex.Replace(last, "[^0-9.]+", string.Empty);
                pictureA = doc.DocumentNode.SelectNodes("//*[@id=\"series_info\"]/div[1]/img")[0].OuterHtml.Split('"');
                date = doc.DocumentNode.SelectNodes("//*[@id=\"chapters\"]/ul/li[1]/div/span")[0].InnerText;
                name = (nameA[4].Replace("_", " "));
                picture = pictureA[3];

                dataGridView1.Rows.Add();
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i];
                row.Cells[0].Value = (name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()));
                row.Cells[1].Value = read[i];
                row.Cells[2].Value = last;
                row.Cells[3].Value = date;
                row.Cells[4].Value = picture;

                if (i == 0)
                {
                    textBox1.Text = (name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()));
                    textBox2.Text = read[0];
                    textBox3.Text = last;
                    textBox4.Text = date;
                    pictureBox1.Load(picture);
                }
            }

            if (dataGridView1.Rows[0].Cells[0].Value == null)
            {
                dataGridView1.Rows[0].Cells[0].Value = "No Data";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var test = e.RowIndex;
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[test];
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            pictureBox1.Load(row.Cells[4].Value.ToString());

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string link = textBox1.Text.Replace(" ", "_").ToLower();
            Process.Start("http://m.mangafox.me/manga/" + link);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = textBox1.Text.Replace(" ", "_").ToLower();
            Process.Start("http://mangafox.me/manga/" + link);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView1.CurrentRow.Cells[1].Value = textBox2.Text;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FileStream fileStream = File.Open(@"C:\Users\Jacob\Desktop\Manga.txt", FileMode.Open, FileAccess.Write);
                
                StreamWriter fileWriter = new StreamWriter(fileStream);
                for (int i = 0; i < dataSplit.Length - 1; i++)
                {
                    fileWriter.Write(url[i] + "|" + dataGridView1.Rows[i].Cells[1].Value + "#");
                }
                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe);
            }
        }
    }
}    
