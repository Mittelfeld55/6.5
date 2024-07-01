using System.Security;
using System.Linq;

class QuartettGame 
{
    #pragma warning disable CS0162
    public static string blatt = "A1A2A3A4B1B2B3B4C1C2C3C4D1D2D3D4E1E2E3E4F1F2F3F4G1G2G3G4H1H2H3H4";
    public static string kartenDesSpielers = "";
    public static int SPunkte = 0;
    public static string kartenDesGegners = "";
    public static int CPunkte = 0;
    public static int TurnCounter = 15; //Maximale Züge bevor auswertung


    static void Main()
    {
        blatt = Helper.shuffle(blatt, 3000);
        PullC(4);
        HandOut(7);
        Console.WriteLine(blatt);
        for (int i = 0; i < TurnCounter; i++)
        {
            Turn(0); // 0 = Player, 1 = Gegner
            Turn(1);
            List <string> Letters = new List<string> {"A", "B", "C", "D", "E", "F", "G", "H"};
            foreach (string Letter in Letters)
            {
                WinGetValue(0, Letter);
                WinGetValue(1, Letter);
            }        
            if (SPunkte == 4 ^ CPunkte == 4) // Ich mache das so, damit ich ein Xor Gate verwende
            {
                Console.WriteLine("Der Punktestand ist: " + SPunkte + " : " + CPunkte);
                if (SPunkte == 4)
                {
                    Console.WriteLine("Du hast gewonnen");
                }
                else
                {
                    Console.WriteLine("Der Gegner hat gewonnen");
                }
            }
            else 
            {
                Console.WriteLine("Der Punktestand ist: " + SPunkte + " : " + CPunkte + " Willst du weiterspielen? (Ja/Nein)");
                string Answer = Console.ReadLine()?.ToUpper() ?? "";
                if (Answer == "Ja")
                {
                    AddTurns(5); //Anpassbar auf gewünschte Anzahl an extra Zügen
                }
                else if (Answer == "Nein")
                {
                    if (SPunkte > CPunkte)
                    {
                        Console.WriteLine("Du hast gewonnen");
                    }
                    else if (SPunkte < CPunkte)
                    {
                        Console.WriteLine("Der Gegner hat gewonnen");
                    }
                }
                else
                {
                    Console.WriteLine("Bitte gib Ja oder Nein ein");
                    if (SPunkte > CPunkte)
                    {
                        Console.WriteLine("Du hast gewonnen");
                    }
                    else if (SPunkte < CPunkte)
                    {
                        Console.WriteLine("Der Gegner hat gewonnen");
                    }
                }
            }
        }   
    }
    public static string PullC(int NumberOfCards) 
    {
        
        string pulledCard = blatt.Substring(0, NumberOfCards * 2);
        blatt = blatt.Substring(NumberOfCards * 2);
        blatt.Replace(pulledCard, "");
        return pulledCard;
    }

    public static string HandOut(int NumberOfCards) 
    {
        for (int i = 0; i < NumberOfCards; i++)
        {
            kartenDesSpielers += PullC(1);
            kartenDesGegners += PullC(1);
        }
        return kartenDesSpielers + kartenDesGegners;
    }

    public static string AddTurns(int NOTurns)
    {
        TurnCounter += NOTurns;
        return "Das Spiel geht noch weiter. Noch " + NOTurns + " Züge.";
    }

    public static string Turn(int TurnID)
    {
        if (TurnID == 0)
        {
            Console.WriteLine("\nDeine Karten: " + kartenDesSpielers + "   " + kartenDesSpielers.Length/2 + " Karten. \nWelche Karte brauchst du? (Z.B. A1) Versuche nicht zwei Karten zu nennen.");
            string input = Console.ReadLine()?.ToUpper() ?? "";
            string benoetigteKarte = input.Length >= 2 ? input.Substring(0, 2) : "";
            if (kartenDesSpielers.Contains(benoetigteKarte.Substring(0,1))) 
            {
                if (kartenDesGegners.Contains(benoetigteKarte))
                {
                    Console.WriteLine("Der Gegner hat die Karte");
                    kartenDesSpielers += benoetigteKarte;
                    kartenDesGegners = kartenDesGegners.Replace(benoetigteKarte, "");
                    Console.WriteLine("Deine Karten: " + kartenDesSpielers + "   " + kartenDesSpielers.Length/2 + " Karten");
                }
                else
                {
                    Console.WriteLine("Der Gegner hat die Karte nicht");
                    kartenDesSpielers += PullC(1);
                    Console.WriteLine("Deine Karten: " + kartenDesSpielers + "   " + kartenDesSpielers.Length/2 + " Karten");
                }
            }
            else
            {
                Console.WriteLine("Du kannst nur nach Karten fragen, die dich weiter bringen.");
                Turn(0);
            }
            return "PlayerTurn";
        }
        else if (TurnID == 1)
        {
            string blattZumCZiehen = blatt.Replace(kartenDesGegners, "");
            string benoetigteKarte = blattZumCZiehen.Substring(0, 2);
            if (kartenDesSpielers.Contains(benoetigteKarte))
            {
                Console.WriteLine("Du hattest die Karte" + benoetigteKarte);
                kartenDesGegners += benoetigteKarte;
                kartenDesSpielers = kartenDesSpielers.Replace(benoetigteKarte, "");
            }
            else
            {
                Console.WriteLine("Du hattest die " + benoetigteKarte +" Karte nicht");
                kartenDesGegners += PullC(1);
            }
            return "GegnerTurn";
        }
        else
        {
            Console.WriteLine("TurnID not valid");
            return "TurnID not valid";
        }
    }

    public static string WinGetValue(int TurnID, string LetterToCheck)
    {
        if (TurnID == 0)
        {
            int Count = kartenDesSpielers.Count(x => x == LetterToCheck[0]);
            if (Count == 4)
            {
                for (int f = 0; f < 4; f++)
                {
                    int indexOfLetter = kartenDesSpielers.IndexOf(LetterToCheck);
                    if (indexOfLetter != -1)
                    {
                        kartenDesSpielers = kartenDesSpielers.Remove(indexOfLetter, 1);
                    }
                }
                SPunkte++;
                Console.WriteLine("Du hast einen Punkt bekommen. Aktueller Punktestand: " + SPunkte + " : " + CPunkte);
            }
            return "Spieler Punkte";
        }
        else if (TurnID == 1)
        {
            int Count = kartenDesGegners.Count(x => x == LetterToCheck[0]);
            if (Count == 4)
            {
                for (int f = 0; f < 4; f++)
                {
                    int indexOfLetter = kartenDesGegners.IndexOf(LetterToCheck);
                    if (indexOfLetter != -1)
                    {
                        kartenDesGegners = kartenDesGegners.Remove(indexOfLetter, 1);
                    }
                }
                CPunkte++;
                Console.WriteLine("Der Gegner hat einen Punkt bekommen. Aktueller Punktestand: " + SPunkte + " : " + CPunkte);
            }
            return "Gegner Points";
        }
        else
        {
            Console.WriteLine("TurnID not valid");
            return "TurnID not valid";
        }
    }
}

class Helper 
{
    //Shuffle the Cards
    public static string shuffle(string blatt, int noOfShuffles)
    {
        var random = new Random();
        var noOfCards = blatt.Length / 2;
        for (int i = 0; i < noOfShuffles; i++)
        {

            var frontIndex = random.Next(0, noOfCards / 3);
            if ((frontIndex % 4 == 0) && (frontIndex != 0))
            {
                frontIndex--;
            }
            var rearIndex = random.Next(noOfCards / 3 * 2, noOfCards);
            blatt = blatt.Substring(0, frontIndex * 2) + blatt.Substring(rearIndex * 2) + blatt.Substring(frontIndex * 2, (rearIndex - frontIndex) * 2);
        }
        return blatt;
    }
}