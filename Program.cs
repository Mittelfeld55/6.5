using System.Security;
using System.Linq;
using System.Reflection.PortableExecutable;

class QuartettGame 
{
    #pragma warning disable
    public static string blatt = "A1A2A3A4B1B2B3B4C1C2C3C4D1D2D3D4E1E2E3E4F1F2F3F4G1G2G3G4H1H2H3H4";
    public static string kartenDesSpielers = "";
    public static int sPunkte = 0;
    public static string kartenDesGegners = "";
    public static int cPunkte = 0;
    public static int regTurnCounter = 15; //Maximale Züge bevor auswertung
    public static int extraTurns = 5;
    static void Main()
    {
        List <string> Letters = new List<string> {"A", "B", "C", "D", "E", "F", "G", "H"};
        blatt = Helper.shuffle(blatt, 3000);
        PullC(4);
        HandOut(7);
        Console.WriteLine(blatt);
        for (int i = 0; i < regTurnCounter; i++)
        {
            Turn(0); // 0 = Player, 1 = Gegner
            foreach (string Letter in Letters)
            {
                WinGetScore(0, Letter);
            } 
            Turn(1);
            foreach (string Letter in Letters)
            {
                WinGetScore(1, Letter);
            }   
        }
        Console.WriteLine("Der Punktestand ist: " + sPunkte + " : " + cPunkte + " Willst du weiterspielen? (Ja/Nein) Keine Eingabe wird als Nein gewertet.");
        string Answer = Console.ReadLine()?.ToUpper() ?? "";
        if (Answer == "Ja")
        {
            AddTurns(extraTurns); //Anpassbar auf gewünschte Anzahl an extra Zügen
        }
        else if (Answer == "Nein")
        {
            Winner();
        }
        else
        {
            Winner();
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
        regTurnCounter += NOTurns;
        return "Das Spiel geht noch weiter. Noch " + NOTurns + " Züge.";
    }

    public static string Turn(int TurnID)
    {
        if (TurnID == 0)
        {
            Console.WriteLine("\nDeine Karten: " + kartenDesSpielers + "   " + kartenDesSpielers.Length/2 + " Karten. \nWelche Karte brauchst du? (Z.B. A1) Versuche nicht zwei Karten zu nennen.");
            string input = Console.ReadLine()?.ToUpper() ?? "";
            if (kartenDesSpielers.Contains(input))
            {
                Console.WriteLine("Du hast die Karte bereits");
                Turn(0);
            }
            string benoetigteKarte = input.Length >= 2 ? input.Substring(0, 2) : "";
            if (kartenDesSpielers.Contains(benoetigteKarte.Substring(0,1))) 
            {
                if (kartenDesGegners.Contains(benoetigteKarte))
                {
                    Console.WriteLine("Der Gegner hat die Karte");
                    kartenDesSpielers += benoetigteKarte;
                    kartenDesGegners = kartenDesGegners.Replace(benoetigteKarte, "");
                    Console.WriteLine("Deine Karten: " + kartenDesSpielers + "   " + kartenDesSpielers.Length/2 + " Karten");
                    return "PsslayerTurn";
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

    public static string WinGetScore(int TurnID, string LetterToCheck)
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
                sPunkte++;
                Console.WriteLine("Du hast einen Punkt bekommen. Aktueller Punktestand: " + sPunkte + " : " + cPunkte);
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
                cPunkte++;
                Console.WriteLine("Der Gegner hat einen Punkt bekommen. Aktueller Punktestand: " + sPunkte + " : " + cPunkte);
            }
            return "Gegner Points";
        }
        else
        {
            Console.WriteLine("TurnID not valid");
            return "TurnID not valid";
        }
    }
    public static void Winner()
    {
        if (sPunkte == 4 || cPunkte == 4) 
        {
        Console.WriteLine("Der Punktestand ist: " + sPunkte + " : " + cPunkte);
        if (sPunkte == 4)
        {
            Console.WriteLine("Du hast gewonnen");
        }
        else
        {
            Console.WriteLine("Der Gegner hat gewonnen");
        }
        }
    }
    #pragma warning enable
}
class Helper 
{ //Shuffle the Cards
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