using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Wörterbuch
{

    public partial class Wörterbuch : Form
    {
        Dictionary<string, string> germanToEnglishDict = new Dictionary<string, string>();
        string dictionaryPath = "C:\\Users\\DCV\\Desktop\\Coding\\04_Vertiefung_CSharp\\Wörterbuch\\Wörterbuch\\dictionary.txt";
  //      String[] englishCSVPath = System.IO.File.ReadAllLines(@"C:\Users\DCV\Desktop\Coding\04_Vertiefung_CSharp\Wörterbuch\Wörterbuch\dictionary.txt");
        public Wörterbuch()
        {
            InitializeComponent();
            importDictionaryTxt();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var germanWord = tbGermanWord.Text; //.Text -> liefert den Inhalt der Textbox
            var englishWord = tbEnglishWord.Text;

            if(!string.IsNullOrEmpty(germanWord) && !string.IsNullOrEmpty(englishWord))  //schat, ob die Felder nicht leer sind
            {
                germanToEnglishDict.Add(germanWord, englishWord);
                UpdateTranslations();
                tbGermanWord.Clear();
                tbEnglishWord.Clear();
            }
        }
        private void UpdateTranslations()
        {
            lBoxGermanWords.DataSource = germanToEnglishDict.Keys.ToList();
        }
        private void lBoxGermanWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedWord = lBoxGermanWords.SelectedItem as string; 

            if (!string.IsNullOrEmpty(selectedWord) && germanToEnglishDict.ContainsKey(selectedWord))
            {
                tbTranslation.Text = germanToEnglishDict[selectedWord];
            }   
        }

        private void btnExportToTxt_Click(object sender, EventArgs e)
        {
            using (var writer = new StreamWriter(dictionaryPath))
            {
                foreach (var pair in germanToEnglishDict)
                {
                    writer.WriteLine("{0};{1};", pair.Key, pair.Value);
                }
            }
        }

        public void importDictionaryTxt()
        {
            var vocs = File.ReadLines(dictionaryPath)
                   .Select(line => line.Split(';'))
                   .Select(x => new { german = x[0], others = x[1] });

            germanToEnglishDict.Clear();

            vocs.ToList().ForEach(x => germanToEnglishDict.Add(x.german, x.others));
            

                UpdateTranslations();
        }

        private void tbSearchGerman_TextChanged(object sender, EventArgs e)
        {
            Dictionary<string, string> searchGermanWordsDict = new Dictionary<string, string>();
            
        //wordToSearch
            string searchLetters = tbSearchGerman.Text;

            foreach (var pair in this.germanToEnglishDict)
            {
                if (pair.Key.ToLower().Contains(searchLetters.ToLower()))
                {
                    searchGermanWordsDict.Add(pair.Key, pair.Value);
                }
            }
            updateSearchBox(searchGermanWordsDict);
        }

        public void updateSearchBox(Dictionary<string, string> germanToEnglishDict)
        {
            lBoxGermanWords.DataSource = germanToEnglishDict.Keys.ToList();
        }


    }
}
