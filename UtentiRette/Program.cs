using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace UtentiRette
{
    internal class Program
    {
        static string Join(string[] array)
        {
            string ris = "";
            for (int i = 0; i < array.Length - 1; i++)
                ris += array[i] + ";";
            ris += array[array.Length - 1];
            return ris;
        }
        static string JoinString(string source, string s)
        {
            return source + ";" + s;
        }
        static void Main(string[] args)
        {
            string path = "./utenti_rette.xml";
            string utentiPath = "./UTENTI.txt";
            string legamiPath = "./LEGAMI.txt";
            string tipirettePath = "./TIPI_RETTE.txt";
            List<string> righeComplete = new List<string>();
            XmlDocument xml = new XmlDocument();
            XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "windows-1252",string.Empty);
            //metto la dichiarazione
            xml.AppendChild(declaration);
            if (File.Exists(path) == false)
                File.Create(path);

            XmlNode root = xml.CreateElement("utenti_rette");
            xml.AppendChild(root);//utenti_rette

            //foreach che legge la riga del file utenti uno per uno ed estrae il primo campo che verrà usato per fare dei controlli nel file dei legami e così via
            foreach (string line in File.ReadAllLines(utentiPath))
            {

                string[] arr=line.Split(';');
                string rigaCompleta = "";//utente+servizi
                string riga = "";//riga solo con l'utente
                List<string> lines = new List<string>();
                foreach(string line2 in File.ReadAllLines(legamiPath))
                {
                    string[] arr2 = line2.Split(';');
                    if (arr2[0]==arr[0])
                    {
                        string[] subs = line2.Split(';');
                        lines.Add(subs[1]);
                    }
                }
                //converto il primo campo in un int per usarlo come codice (uno dei nodi)
                float n = float.Parse(arr[0]);
                int n2 = (int)n;
                arr[0] = n2.ToString();
                riga = Join(arr);
                rigaCompleta = riga;
                foreach(string line3 in File.ReadAllLines(tipirettePath))
                {
                    for(int i = 0; i < lines.Count; i++)
                    {
                        string[] ss;
                        string[] arr3 = line3.Split(';');
                        if (arr3[0]==lines[i])
                        {
                            ss= line3.Split(';');
                            rigaCompleta = JoinString(riga, ss[1]);
                            riga = rigaCompleta;//faccio questo per tenere aggiornata la riga in caso un utente abbia più servizi 
                        }
                    }
                }
                //aggiungo le righe complete(utenti+servizi) in una lista
                righeComplete.Add(rigaCompleta); 
            }
            //ciclo che per ogni elemento della lista(rigaCompleta) mi creerà un nodo nella struttura
            for (int i = 0; i < righeComplete.Count; i++)
            {

                XmlNode user = xml.CreateElement("utente");
                XmlNode services = xml.CreateElement("servizi");
                root.AppendChild(user);
                //creo gli elementi di ogni utente
                string[] nodes = righeComplete[i].Split(';');
                XmlNode code = xml.CreateElement("codice"); 
                code.InnerText = nodes[0];
                XmlNode surname = xml.CreateElement("cognome");
                surname.InnerText = nodes[1];
                XmlNode name = xml.CreateElement("nome");
                name.InnerText = nodes[2];
                XmlNode bday = xml.CreateElement("data_nascita");
                bday.InnerText = nodes[3];
                XmlNode sex = xml.CreateElement("sesso");
                sex.InnerText = nodes[4];
                XmlNode school = xml.CreateElement("scuola");
                school.InnerText = nodes[5];
                XmlNode Class = xml.CreateElement("classe");
                Class.InnerText = nodes[6];
                XmlNode section = xml.CreateElement("sezione");
                section.InnerText = nodes[7];
                XmlNode psurname = xml.CreateElement("cog_geni");
                psurname.InnerText = nodes[8];
                XmlNode pname = xml.CreateElement("nom_geni");
                pname.InnerText = nodes[9];
                //li metto nella struttura
                root.ChildNodes[i].AppendChild(code);
                root.ChildNodes[i].AppendChild(surname);
                root.ChildNodes[i].AppendChild(name);
                root.ChildNodes[i].AppendChild(sex);
                root.ChildNodes[i].AppendChild(school);
                root.ChildNodes[i].AppendChild(Class);
                root.ChildNodes[i].AppendChild(section);
                root.ChildNodes[i].AppendChild(psurname);
                root.ChildNodes[i].AppendChild(pname);

                root.ChildNodes[i].AppendChild(services);
                for(int x = 10; x < nodes.Length; x++)
                { 
                    //metto i servizi nella struttura
                    XmlNode service = xml.CreateElement("servizio");
                    service.InnerText = nodes[x];
                    root.ChildNodes[i].LastChild.AppendChild(service);
                }
                xml.Save(path);
            }
            
            Console.WriteLine("file xml creato");
        }
    }
}
