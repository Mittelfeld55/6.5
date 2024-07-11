using System.Linq;
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