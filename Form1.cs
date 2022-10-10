using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleWebBrowser
{
    public partial class WebBrowser : Form
    {
        public WebBrowser()
        {
            InitializeComponent();
        }

        // define public variables for future use
        public static HttpClient Client = new HttpClient();
        public static string LastUsedLink = "";
        public static string CurrentlyUsedLink = "";

        private void WebBrowser_Load(object sender, EventArgs e)
        {
            // check if preference.cfg exists
            if (System.IO.File.Exists("preference.cfg"))
            {
                // read preference.cfg
                string[] lines = System.IO.File.ReadAllLines("preference.cfg");
                // check if preference.cfg has a link
                if (lines.Length > 0)
                {
                    // set home page if it exists
                    if (lines[0].Length > 0)
                    {
                        // assign the home page
                        UrlBar.Text = lines[0];
                        // remember the home page
                        CurrentlyUsedLink = lines[0];
                    }
                }
            }
            else
            {
                // create preference.cfg
                System.IO.File.Create("preference.cfg");
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            // if currentlyusedlink is not empty set it to the lastusedlink
            if (CurrentlyUsedLink.Length > 0)
            {
                LastUsedLink = CurrentlyUsedLink;
            }
            // set currentlyusedlink to the urlbar text
            CurrentlyUsedLink = UrlBar.Text;
            // if the urlbar text is not empty Navigate the url
            if (UrlBar.Text.Length > 0)
            {
                Navigate(UrlBar.Text);
            }
        }

        // defining a navigate function
        public void Navigate(string url)
        {
            // send http request
            HttpResponseMessage response = Client.GetAsync(UrlBar.Text).Result;
            // display the response
            HTMLPreview.Text = response.Content.ReadAsStringAsync().Result;
            // change the tab name to the title of the web page from the html element
            tabControl1.SelectedTab.Text = HTMLPreview.Text.Substring(HTMLPreview.Text.IndexOf("<title>") + 7, HTMLPreview.Text.IndexOf("</title>") - HTMLPreview.Text.IndexOf("<title>") - 7);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // load the previous link
            UrlBar.Text = LastUsedLink;
            // navigate to previous link
            Navigate(LastUsedLink);
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            // load the current link
            UrlBar.Text = CurrentlyUsedLink;
            // navigate to current link
            Navigate(CurrentlyUsedLink);
        }
    }
}