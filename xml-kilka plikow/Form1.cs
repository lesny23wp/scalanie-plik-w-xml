using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace xml_kilka_plikow
{
    public partial class Form1 : Form
    {
        string sciezka1;
        string sciezka2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
 
            FolderBrowserDialog folderbrowser1 = new FolderBrowserDialog();

            folderbrowser1.ShowDialog();

            textBox1.Text = folderbrowser1.SelectedPath.ToString();

            sciezka1 = textBox1.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                sciezka1 = textBox1.Text;
                var downloadfolder = sciezka1;
                string[] files = Directory.GetFiles(downloadfolder);
                var masterfile = new XDocument();

                XElement dokument = new XElement("ProfitsCosts");


                masterfile.Add(dokument);

                foreach (var file in files)
                {

                    XDocument xdoc = XDocument.Load(file);

                    masterfile.Root.Add(xdoc.Descendants("ProfitsCostsEntry"));

                }


                //zapis
                sciezka2 = textBox2.Text;

                masterfile.Save(sciezka2);


                int dlugoscTablicy;
                dlugoscTablicy = files.Length;

                string dd = files[0];
                string ddostatni = files[dlugoscTablicy - 1];


                string calosc1 = System.IO.File.ReadAllText(dd);

                Match datap = Regex.Match(calosc1, "<From.*From>");

                string calosc2 = System.IO.File.ReadAllText(ddostatni);

                Match datak = Regex.Match(calosc2, "<To.*To>");


                string wynik_bez_dat = System.IO.File.ReadAllText(sciezka2);
                string zastap = "<AccountingProfitsCostsReport>";

                string wynik_z_dat = Regex.Replace(wynik_bez_dat, "<ProfitsCosts>", zastap + Environment.NewLine + "\t" + datap + Environment.NewLine + "\t" + datak + Environment.NewLine + "<ProfitsCosts>");

                string koncowka = "\r\n</AccountingProfitsCostsReport>";
                string wyn = wynik_z_dat + koncowka;

                string plik = (sciezka2);
                
                //string plik = sciezka3;
                StreamWriter sw = new StreamWriter(plik, false);
                sw.Write(wyn);
                sw.Close();

                label1.Text = "Zakończono łączenie!";

                int n = 0;


                while (n != dlugoscTablicy)
                {


                    richTextBox1.AppendText(files[n] + "\r\n");

                    n++;
                }


            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Nie wybrano pliku!");

            }

            catch (Exception)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Nie podano sciezki!");
                }
                else
                {
                    MessageBox.Show("Błędna ściezka lub wybrano folder bez plików xml!");
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox2.Text = saveFileDialog1.FileName;
        }
    }
}
