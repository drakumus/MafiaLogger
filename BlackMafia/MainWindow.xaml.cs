using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackMafia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, int> players;
        Dictionary<string, int> retiredPlayers;

        List<ListViewItem> NameItem;
        List<ListViewItem> NumberItem;

        int gamesPlayed;

        public MainWindow()
        {
            players = new Dictionary<string, int>();
            gamesPlayed = 0;

            NameItem = new List<ListViewItem>();
            NumberItem = new List<ListViewItem>();
            retiredPlayers = new Dictionary<string, int>();

            InitializeComponent();
        }

        private void populateLists()
        {
            PlayerNamesList.Items.Clear();
            PlayerGamesPlayedList.Items.Clear();

            int count = 0;
            ListViewItem tempName;
            ListBoxItem tempGamesPlayed;

            var bc = new BrushConverter();

            foreach(string s in players.Keys)
            {
                count++;
                
                //Populate Player Name List View
                tempName = new ListViewItem();
                tempName.AddHandler(ListViewItem.SelectedEvent, new RoutedEventHandler(OnSelectEvent));
                if (count % 2 == 0)
                    tempName.Background = (Brush)bc.ConvertFrom("#FFCDE5E1");

                tempName.Content = count.ToString() + ".\t" + s;
                tempName.HorizontalAlignment = HorizontalAlignment.Stretch;
                PlayerNamesList.Items.Add(tempName);
                NameItem.Add(tempName);

                //populate games played ListView
                tempGamesPlayed = new ListViewItem();
                if (count % 2 == 0)
                    tempGamesPlayed.Background = (Brush)bc.ConvertFrom("#FFCDE5E1");

                tempGamesPlayed.Content = players[s].ToString();
                tempGamesPlayed.HorizontalAlignment = HorizontalAlignment.Stretch;
                PlayerGamesPlayedList.Items.Add(tempGamesPlayed);
                NumberItem.Add((ListViewItem)tempGamesPlayed);
            }

            int numPlayers = players.Count + retiredPlayers.Count;
            TotalPlayersBox.Text = numPlayers.ToString();
        }

        private void populateRetired()
        {
            RetiredPlayerNamesList.Items.Clear();
            RetiredPlayerGamesPlayedList.Items.Clear();

            int count = 0;
            ListViewItem tempName;
            ListBoxItem tempGamesPlayed;

            var bc = new BrushConverter();

            foreach (string s in retiredPlayers.Keys)
            {
                count++;

                //Populate Player Name List View
                tempName = new ListViewItem();
                tempName.AddHandler(ListViewItem.SelectedEvent, new RoutedEventHandler(OnSelectEvent));
                if (count % 2 == 0)
                    tempName.Background = (Brush)bc.ConvertFrom("#FFCDE5E1");

                tempName.Content = count.ToString() + ".\t" + s;
                tempName.HorizontalAlignment = HorizontalAlignment.Stretch;
                RetiredPlayerNamesList.Items.Add(tempName);

                //populate games played ListView
                tempGamesPlayed = new ListViewItem();
                if (count % 2 == 0)
                    tempGamesPlayed.Background = (Brush)bc.ConvertFrom("#FFCDE5E1");

                tempGamesPlayed.Content = retiredPlayers[s].ToString();
                tempGamesPlayed.HorizontalAlignment = HorizontalAlignment.Stretch;
                RetiredPlayerGamesPlayedList.Items.Add(tempGamesPlayed);
            }

            int numPlayers = players.Count + retiredPlayers.Count;
            TotalPlayersBox.Text = numPlayers.ToString();
        }

        private void AddName(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            if (NameBox.Text.Length != 0 && !players.ContainsKey(NameBox.Text) && !retiredPlayers.ContainsKey(NameBox.Text)) {
                if (Int32.TryParse(NumBox.Text, out temp))
                {
                    players.Add(NameBox.Text, temp);
                }
                if (NumBox.Text.Length == 0)
                {
                    players.Add(NameBox.Text, 0);

                }

            } else if (players.ContainsKey(NameBox.Text) && NumBox.Text.Length != 0){
                if(NumBox.Text != players[NameBox.Text].ToString())
                {
                    if(Int32.TryParse(NumBox.Text, out temp))
                        players[NameBox.Text] = temp;
                }
            }
            if (retiredPlayers.ContainsKey(NameBox.Text) && NameBox.Text.Length != 0)
            {
                if(Int32.TryParse(NumBox.Text, out temp))
                    retiredPlayers[NameBox.Text] = temp;
                populateRetired();
            }
            NameBox.Clear();
            NumBox.Clear();
            populateLists();
        }

        private void RemoveName(object sender, RoutedEventArgs e)
        {
            if (players.Remove(NameBox.Text))
            {
                NameBox.Clear();
                NumBox.Clear();
            }
            else if(retiredPlayers.Remove(NameBox.Text))
            {
                NameBox.Clear();
                NumBox.Clear();
            }

            populateRetired();

            populateLists();
            
        }

        private void Retire(object sender, RoutedEventArgs e)
        {
            if (players.ContainsKey(NameBox.Text)) { 
                retiredPlayers.Add(NameBox.Text, players[NameBox.Text]);
                players.Remove(NameBox.Text);
            } else if (retiredPlayers.ContainsKey(NameBox.Text))
            {
                players.Add(NameBox.Text, retiredPlayers[NameBox.Text]);
                retiredPlayers.Remove(NameBox.Text);
            }
            populateLists();
            populateRetired();
        }

        private void OnSelectEvent(object sender, RoutedEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Source;
            string myContent = item.Content.ToString();
            NameBox.Text = myContent.Split(new char[] { '\t' }, 2)[1];
            foreach (string s in players.Keys)
            {
                if (s == NameBox.Text)
                    NumBox.Text = players[s].ToString();
            }
        }

        private void IncrementGames(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            gamesPlayed++;
            if(players.Count != 0)
            {
                foreach (string s in players.Keys.ToArray<string>())
                {
                    temp = players[s];
                    players[s] = temp + 1;
                }
            }

            if (players.Count != 0)
                TotalGamesPlayedBox.Text = gamesPlayed.ToString();

            populateLists();
        }

        private void DecrementGames(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            if (gamesPlayed > 0)
            {
                gamesPlayed--;
                foreach (string s in players.Keys.ToArray<string>())
                {
                    if (players[s] > 0)
                    {
                        temp = players[s];
                        players[s] = temp - 1;
                    }
                }

                if (players.Keys.ToArray<string>().Length != 0)
                    TotalGamesPlayedBox.Text = gamesPlayed.ToString();

                populateLists();
            }
        }

        private void EnterEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddName(sender, e);
                e.Handled = true;
            }
        }

        private void LoadTS_Click(object sender, RoutedEventArgs e)
        {
            Stream stream;
            StreamReader reader;

            txtEditor.Clear();

            List<string> lines = new List<string>();
            List<string> nameList = new List<string>();

            string line;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            var fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TS3Client\\chats");
            openFileDialog.InitialDirectory = fileName;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                reader = new StreamReader(stream);
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                FormatText(ref lines);

                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(":"))
                    {
                        string[] s = lines[i].Split(new char[] { ':' }, 2);
                        if (s[1].Contains("x"))
                            nameList.Add(s[0]);
                    }
                }

                foreach (string l in nameList)
                {
                    txtEditor.Text += l + "\n";
                }
                    
            }

            foreach(string s in players.Keys)
            {
                retiredPlayers.Add(s, players[s]);
            }

            players.Clear();

            foreach(string s in nameList)
            {
                if (!players.ContainsKey(s))
                {
                    if (retiredPlayers.ContainsKey(s))
                    {
                        players.Add(s, retiredPlayers[s]);
                        retiredPlayers.Remove(s);
                    }
                    else
                    {
                        players.Add(s, 0);
                    }
                }
            }
            populateLists();
            populateRetired();

            File.WriteAllText(openFileDialog.FileName, String.Empty);
        }

        private void FormatText(ref List<string> lines)
        {

            for(int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length > 0 && lines[i].Length > 11)
                {
                    lines[i] = lines[i].Substring(11, lines[i].Length-11);
                }
            }
        }
    }
}
