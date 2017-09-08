using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string Url = "http://mangafox.me/manga/maou_no_hisho";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(Url);

            string data = doc.DocumentNode.SelectNodes("//*[@id=\"chapters\"]/ul/li[1]/div/h3/a")[0].InnerText;
            string dataP = doc.DocumentNode.SelectNodes("//*[@id=\"series_info\"]/div[1]/img")[0].InnerText;
            
            data = Regex.Replace(data, "[^0-9]+", string.Empty);

            textBox1.Text = data;0
            //pictureBox1.Load(dataP);
        }
    }
}
