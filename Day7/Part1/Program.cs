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

    //T = 10, J = 11, Q = 12, K = 13, A = 14
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
                    case 'J': cards.Add(11); break;
                    case 'Q': cards.Add(12); break;
                    case 'K': cards.Add(13); break;
                    case 'A': cards.Add(14); break;
                    default: Console.WriteLine("This should not happen"); break;
                }
            }
        }
    }

    void DetermineHand()
    {
        if(cards.All(x => x == cards.First()))
        {
            type = PokerHands.FiveOfAKind;
            return;
        }
        else if(cards.GroupBy(x => x).Any(group => group.Count() == 4))
        {
            type = PokerHands.FourOfAKind;
            return;
        }
        else if(cards.GroupBy(x => x).Any(group => group.Count() == 3))
        {
            var grouped = cards.GroupBy(x => x);
            var groupsWithThreeOrMore = grouped.Where(group => group.Count() == 3);
            var valuesNotInGroup = cards.Except(groupsWithThreeOrMore.SelectMany(group => group));

            if(valuesNotInGroup.Distinct().Count() == 1)
            {
                type = PokerHands.FullHouse;
                return;
            }
            else
            {
                type = PokerHands.ThreeOfAKind;
                return;
            }
        }
        else if(cards.GroupBy(x => x).Any(group => group.Count() == 2))
        {
            var grouped = cards.GroupBy(x => x);
            var groupsWithTwoOrMore = grouped.Where(group => group.Count() == 2);

            if(groupsWithTwoOrMore.Count() == 1)
            {
                type = PokerHands.OnePair;
                return;
            }
            else if(groupsWithTwoOrMore.Count() == 2)
            {
                type = PokerHands.TwoPair;
                return;
            }
        }
        else
        {
            type = PokerHands.HighCard;
            return;
        }
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
    FiveOfAKind  = 6
}