string[] lines = File.ReadAllLines("../input.txt");

List<Hand> hands = new List<Hand>();

foreach (string l in lines)
{
    string[] hand = l.Split(' ');
    hands.Add(new Hand(hand[0], int.Parse(hand[1])));
}

hands = SortHands(hands);
hands.Reverse();

int result = 0;
int rank = 1;
foreach(Hand h in hands)
{   
    result += h.bidAmount * rank;
    rank++;
}

Console.WriteLine("Result: " + result);

List<Hand> SortHands(List<Hand> hands)
{
    List<Hand> sortedHands = new List<Hand>();
    sortedHands = hands.OrderByDescending(x => (int)x.type).ToList();

    for(int i = 0; i < sortedHands.Count - 1; i++)
    {
        for(int x = i + 1; x < sortedHands.Count; x++)
        {
            if(sortedHands[i].type == sortedHands[x].type)
            {
                for(int j = 0; j < sortedHands[i].cards.Count; j++)
                {
                    if(sortedHands[i].cards[j] < sortedHands[x].cards[j])
                    {
                        var temp = sortedHands[x];
                        sortedHands[x] = sortedHands[i];
                        sortedHands[i] = temp;
                        break;
                    }
                    else if(sortedHands[i].cards[j] > sortedHands[x].cards[j])
                    {
                        break;
                    }
                }
            }
        }
    }
    return sortedHands;
}

class Hand
{
    public PokerHands type;

    public List<int> cards;
    public int bidAmount;

    public Hand(string hand, int bid)
    {
        cards = new List<int>();
        bidAmount = bid;
        AddCards(hand);
        DetermineHand();
    }

    void AddCards(string cardsInHand)
    {
        foreach(char c in cardsInHand)
        {
            int card;
            if (int.TryParse(c.ToString(), out card))
            {
                cards.Add(card);
            }
            else
            {
                switch(c)
                {
                    case 'T': cards.Add(10); break;
                    case 'J': cards.Add(1); break;
                    case 'Q': cards.Add(11); break;
                    case 'K': cards.Add(12); break;
                    case 'A': cards.Add(13); break;
                    default: Console.WriteLine("This should not happen"); break;
                }
            }
        }
    }

    void DetermineHand()
    {
        PokerHands highestCurrentType = PokerHands.HighCard;
        for(int i = 1; i < 14; i++)
        {
            List<int> cardsTemp = new List<int>();

            foreach(int c in cards)
            {
                cardsTemp.Add(c);
            }

            for(int j = 0; j < cardsTemp.Count; j++)
            {
                if(cardsTemp[j] == 1)
                {
                    cardsTemp[j] = i;
                }
            }

            if(cardsTemp.All(x => x == cardsTemp.First()))
            {
                if(PokerHands.FiveOfAKind > highestCurrentType)
                {
                    highestCurrentType = PokerHands.FiveOfAKind;
                }
            }
            else if(cardsTemp.GroupBy(x => x).Any(group => group.Count() == 4))
            {
                if(PokerHands.FourOfAKind > highestCurrentType)
                {
                    highestCurrentType = PokerHands.FourOfAKind;
                }
            }
            else if(cardsTemp.GroupBy(x => x).Any(group => group.Count() == 3))
            {
                var grouped = cardsTemp.GroupBy(x => x);
                var groupsWithThreeOrMore = grouped.Where(group => group.Count() == 3);
                var valuesNotInGroup = cardsTemp.Except(groupsWithThreeOrMore.SelectMany(group => group));

                if(valuesNotInGroup.Distinct().Count() == 1)
                {
                    if(PokerHands.FullHouse > highestCurrentType)
                    {
                        highestCurrentType = PokerHands.FullHouse;
                    }
                }
                else
                {
                    if(PokerHands.ThreeOfAKind > highestCurrentType)
                    {
                        highestCurrentType = PokerHands.ThreeOfAKind;
                    }
                }
            }
            else if(cardsTemp.GroupBy(x => x).Any(group => group.Count() == 2))
            {
                var grouped = cardsTemp.GroupBy(x => x);
                var groupsWithTwoOrMore = grouped.Where(group => group.Count() == 2);

                if(groupsWithTwoOrMore.Count() == 1)
                {
                    if(PokerHands.OnePair > highestCurrentType)
                    {
                        highestCurrentType = PokerHands.OnePair;
                    }
                }
                else if(groupsWithTwoOrMore.Count() == 2)
                {
                    if(PokerHands.TwoPair > highestCurrentType)
                    {
                        highestCurrentType = PokerHands.TwoPair;
                    }
                }
            }
        }
        type = highestCurrentType;
    }
}

enum PokerHands
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}