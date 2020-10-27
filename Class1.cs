using System;
using System.Collections.Generic;
using System.Text;

namespace poker.exe._2._0
{
    class Card
    {
        public int power { get; private set; }
        public char suit { get; private set; }
        public Card(int tempPow, char tempSuit)
        {
            power = tempPow;
            suit = tempSuit;
        }
        public Card() { }
    }

    class HandCards
    {
        public int len { get; } = 7;
        Card[] my = new Card[2];
        static Card[] all = new Card[5];
        public Card this[int index]
        {
            get
            {
                if (index < 2 && index > -1)
                    return my[index];
                else if (index > 1 && index < 7)
                    return all[index - 2];
                else return null;
            }
            set
            {
                if (index < 2 && index > -1)
                    my[index] = value;
                else if (index > 1 && index < 7)
                    all[index - 2] = value;
            }
        }
    }

    static class Deck
    {
        static int numb;
        static int kolCard = 54;
        static Card[] cards = new Card[kolCard];

        static int start = 0;
        public static void detDeck()
        {
            cheet();
            numb = 0;
            for (int i = start; i < kolCard - 2; i++)
            {
                char tempch = tempchar(i);
                Card tempCard = new Card(2 + i / 4, tempch);
                cards[i] = tempCard;
            }
            Card tempCard1 = new Card(15, 'r');
            cards[kolCard - 2] = tempCard1;
            tempCard1 = new Card(15, 'd');
            cards[kolCard - 1] = tempCard1;

            for (int i = 0; i < 100; i++)
            {
                Random rnd = new Random();
                int value = rnd.Next(start, kolCard);
                int val2 = rnd.Next(start, kolCard);
                (cards[value], cards[val2]) = (cards[val2], cards[value]);
            }
        }
        static void cheet()
        {
            //start = 7;
            //Card tempCard1 = new Card(7, 'k');
            //cards[0] = tempCard1;
            //Card tempCard2 = new Card(15, 'r');
            //cards[1] = tempCard2;
            //Card tempCard3 = new Card(15, 'd');
            //cards[2] = tempCard3;
            //Card tempCard4 = new Card(5, 'b');
            //cards[3] = tempCard4;
            //Card tempCard5 = new Card(14, 'c');
            //cards[4] = tempCard5;
            //Card tempCard6 = new Card(3, 'p');
            //cards[5] = tempCard6;
            //Card tempCard7 = new Card(10, 'k');
            //cards[6] = tempCard7;
        }
        static char tempchar(int i)
        {
            int temp = i % 4;
            if (temp == 0) return 'b';
            else if (temp == 1) return 'k';
            else if (temp == 2) return 'c';
            else return 'p';
        }
        public static Card getdeck()
        {
            return cards[numb++];
        }
    }

    class Player
    {
        HandCards handCards = new HandCards();
        static int numbPl = 0;
        public int cont { get; private set; }
        public int sum { get; private set; }
        public bool loser { get; private set; }
        public bool give { get; private set; }
        int startSum;
        int Rjoker, Bjoker;
        public Player(int startsum)
        {
            give = false;
            startSum = startsum;
            sum = startsum;
            cont = 0;
            int len;
            if (numbPl == 0) len = 7;
            else len = 2;
            for (int i = 0; i < len; i++)
            {
                handCards[i] = Deck.getdeck();
            }
            Rjoker = 0;
            Bjoker = 0;
            for (int i = 0; i < handCards.len; i++)
            {
                if (handCards[i].power == 15)
                {
                    if (handCards[i].suit == 'r')
                        Rjoker = 1;
                    else if (handCards[i].suit == 'd')
                        Bjoker = 1;
                }
            }
            numbPl++;
        }
        public Player(int startsum, bool tloser) : this(startsum)
        {
            loser = tloser;
        }
        public int getCont()
        {
            return startSum - sum;
        }
        public int aibot(int rate, int numbCycl, int max, int motion)
        {

            if (max != 0 && numbCycl == 0 && motion != 0)
            {
                motion = 3 - motion;
                int comb = onlycomb(motion);
                if (comb == 8 && motion == 0)
                {
                    giveCards();
                    return 0;
                }
                else if (motion==0)
                {
                    int newrate = 1000 / comb;
                    if (newrate > rate)
                        rate = newrate;
                }
            }            
            rateAccept(rate);
            return rate;
        }
        public void rateAccept(int rate)
        {
            sum -= rate - cont;
            cont += rate - cont;
        }
        public void contZero() => cont = 0;
        internal int max()
        {
            int pow1 = handCards[0].power, pow2 = handCards[1].power;
            if (pow1 == 15)
                return 14;
            if (pow1 > pow2)
                return pow1;
            else
                return pow2;
        }
        (int, int) sameCard(int kol1 = 0, int kol2 = 0, int motion = 0)
        {
            int[] same = new int[15];
            int same1 = 0, same2 = 0;
            for (int i = 0; i < handCards.len - motion; i++)
            {
                if (handCards[i].power != 15)
                    same[handCards[i].power]++;
            }
            int maxSame = 0;
            int jokers = Rjoker + Bjoker;
            if (jokers > 0)
            {
                for (int i = 1; i < same.Length; i++)
                {
                    if (same[i] > same[maxSame])
                        maxSame = i;
                }
                same[maxSame] += jokers;
            }
            for (int i = 0; i < same.Length; i++)
            {
                if (same[i] >= kol1 && same[i] > same1)
                    same1 = i;
                else if (kol2 != 0 && same[i] >= kol2 && same[i] > same2)
                    same2 = i;
            }
            return (same1, same2);
        }
        char flash(int motion = 0)
        {
            int suitP = 0, suitK = 0, suitB = 0, suitC = 0;
            for (int i = 0; i < handCards.len - motion; i++)
            {
                if (handCards[i].suit == 'p')
                    suitP++;
                else if (handCards[i].suit == 'k')
                    suitK++;
                else if (handCards[i].suit == 'b')
                    suitB++;
                else if (handCards[i].suit == 'c')
                    suitC++;
            }
            if (suitP > 4 - Bjoker)
                return 'p';
            else if (suitK > 4 - Bjoker)
                return 'k';
            else if (suitB > 4 - Rjoker)
                return 'b';
            else if (suitC > 4 - Rjoker)
                return 'c';
            else return '0';
        }
        (int, char) street(int motion = 0)
        {
            char stfl = flash();
            int Iflash = 0;
            int lenStreet = Rjoker + Bjoker;
            string[] streetArr = new string[15];
            for (int i = 0; i < handCards.len - motion; i++)
            {
                if (handCards[i].power != 15)
                {
                    if (streetArr[handCards[i].power] == null && handCards[i].power != 15)
                        streetArr[handCards[i].power] = handCards[i].suit.ToString();
                    else if (handCards[i].power != 15)
                        streetArr[handCards[i].power] += handCards[i].suit.ToString();
                }
            }
            for (int i = streetArr.Length - 1; i >= 6; i--)
            {
                if (streetArr[i] != null)
                {
                    if (streetArr[i].Contains(stfl))
                        Iflash++;
                    for (int j = 1; j < 5; j++)
                    {
                        if (streetArr[i - j] != null)
                        {
                            if (streetArr[i - j].Contains(stfl))
                                Iflash++;
                            lenStreet++;
                            if (lenStreet > 3 && stfl > 3)
                                return (i, stfl);
                            else if (lenStreet > 4)
                                return (i, '0');
                        }
                    }
                }
                Iflash = 0;
                lenStreet = Rjoker + Bjoker;
            }
            return (0, '0');
        }
        int onlycomb(int hidCard)
        {
            (int, int, int, char) comb = detcomb(hidCard);
            return comb.Item1;
        }
        public (int, int, int, char) detcomb(int hidCard)
        {
            int pow1, pow2 = 0;
            char suit1;
            (pow1, suit1) = street(hidCard);
            if (suit1 != '0')
                return (1, pow1, pow2, suit1);

            (pow1, pow2) = sameCard(4, 0, hidCard);
            if (pow1 != 0)
                return (2, pow1, pow2, suit1);

            (pow1, pow2) = sameCard(3, 2, hidCard);
            if (pow1 != 0 && pow2 != 0)
                return (3, pow1, pow2, suit1);

            pow1 = 0; pow2 = 0;// suit1 = 0; 
            suit1 = flash(hidCard);
            if (suit1 != '0')
                return (4, pow1, pow2, suit1);

            pow2 = 0; //suit1 = 0;
            (pow1, suit1) = street(hidCard);
            if (pow1 != 0)
                return (5, pow1, pow2, suit1);

            (pow1, pow2) = sameCard(3, 0, hidCard);
            if (pow1 != 0)
                return (6, pow1, pow2, suit1);

            (pow1, pow2) = sameCard(2, 2, hidCard);
            if (pow1 != 0)
                return (7, pow1, pow2, suit1);

            return (8, max(), pow2, suit1);
        }
        public Card GetCard(int index)
        {
            return handCards[index];
        }
        public void surrender() => loser = true;
        public void giveCards() => give = true;
        public void endGame(int rate1)
        {
            sum += rate1;
            numbPl = 0;
        }
    }

    class Game
    {
        public int flop { get; private set; }
        public int rate { get; set; }
        public int acttualRate { get; set; }
        public Game()
        {
            flop = 0;
            rate = 0;
            acttualRate = 0;
        }
        public void getflop(int newFlop)
        {
            flop = newFlop;
        }
        public (string, int, int, char, int) detWinner(Player[] player)
        {
            (int, int, int, char)[] comb = new (int, int, int, char)[player.Length];
            int numbwin = 0;
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i].loser == false && player[i].give == false)
                    comb[i] = player[i].detcomb(0);
                else
                    comb[i] = (10, 0, 0, '0');
            }
            for (int i = 0; i < player.Length - 1; i++)
            {
                if (comb[i + 1].Item1 < comb[numbwin].Item1)
                    numbwin = i + 1;
                else if (comb[i + 1].Item1 == comb[numbwin].Item1)
                {
                    if (comb[i + 1].Item2 > comb[numbwin].Item2)
                        numbwin = i + 1;
                    else if (comb[i + 1].Item2 == comb[numbwin].Item2)
                    {
                        if (comb[i + 1].Item3 > comb[numbwin].Item3)
                            numbwin = i + 1;
                        else if (comb[i + 1].Item3 == comb[numbwin].Item3)
                        {
                            if (player[i + 1].max() > player[i].max())
                                numbwin = i + 1;
                        }
                    }
                }
            }
            string naneComb = nameComb(comb[numbwin].Item1);
            return (naneComb, comb[numbwin].Item2, comb[numbwin].Item3, comb[numbwin].Item4, numbwin + 1);
        }
        public void payment(int winner, Player[] player)
        {
            //   winner--; 
            player[winner].endGame(rate);
        }
        string nameComb(int numb)
        {
            if (numb == 1)
                return "Стрит Флеш";
            else if (numb == 2)
                return "Каре";
            else if (numb == 3)
                return "Фулл Хаус";
            else if (numb == 4)
                return "Флеш";
            else if (numb == 5)
                return "Стрит";
            else if (numb == 6)
                return "Сет";
            else if (numb == 7)
                return "Пара";
            else
                return "Старшая карта";
        }
        static public string namePow(int pow)
        {
            if (pow < 11) return pow.ToString();
            else if (pow == 11) return "валет";
            else if (pow == 12) return "дама";
            else if (pow == 13) return "кароль";
            else return "туз";
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        static public string nameSuit(char suit)
        {
            if (suit == 'p') return "пики";
            if (suit == 'k') return "крести";
            if (suit == 'c') return "черви";
            else return "буби";
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }
}
