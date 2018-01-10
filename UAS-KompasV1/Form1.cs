using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace UAS_KompasV1
{
    public partial class Form1 : Form
    {
        public int currentActiveItemIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser.Navigate("https://www.google.com/search?q=site%3Akompas.com+" + txtQuery.Text); 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument document = webBrowser.Document;
            if (document != null)
            {
                HtmlElement searchResultsContainer = document.GetElementById("res");
                if (searchResultsContainer != null)
                {
                    HtmlElementCollection searchResults = searchResultsContainer.GetElementsByTagName("h3");

                    foreach (HtmlElement searchResult in searchResults)
                    {
                        HtmlElementCollection links = searchResult.GetElementsByTagName("a");
                        if (links.Count > 0)
                        {
                            HtmlElement link = links[0];
                            String url = link.GetAttribute("href");

                            linkNews.Items.Add(url);
                        }
                    }

                    timer1.Enabled = true;
                }
                
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            linkNews.SelectedIndex = currentActiveItemIndex;
            String url = linkNews.SelectedItem.ToString();

            WebClient client = new WebClient();

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            textBox.AppendText(s);
            currentActiveItemIndex++;

            if (currentActiveItemIndex == linkNews.Items.Count)
            {
                timer1.Enabled = false;

                MessageBox.Show("Finish!");
            }
        }
    }
}
