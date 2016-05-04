using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SharedModels.GameComponents;

namespace ClientGUI
{
    public class Deck
    {
        public static Dictionary<CardSuit, Dictionary<CardRank, Bitmap>> CardImageMap = new Dictionary<CardSuit, Dictionary<CardRank, Bitmap>>
        {
            { 
                CardSuit.Clubs, new Dictionary<CardRank, Bitmap>
                {
                    { CardRank.Ace, Properties.Resources.ace_of_clubs     },
                    { CardRank.Two, Properties.Resources._2_of_clubs      },
                    { CardRank.Three, Properties.Resources._3_of_clubs    },
                    { CardRank.Four, Properties.Resources._4_of_clubs     },
                    { CardRank.Five, Properties.Resources._5_of_clubs     },
                    { CardRank.Six, Properties.Resources._6_of_clubs      },
                    { CardRank.Seven, Properties.Resources._7_of_clubs    },
                    { CardRank.Eight, Properties.Resources._8_of_clubs    },
                    { CardRank.Nine, Properties.Resources._9_of_clubs     },
                    { CardRank.Ten, Properties.Resources._10_of_clubs     },
                    { CardRank.Jack, Properties.Resources.jack_of_clubs   }, 
                    { CardRank.Queen, Properties.Resources.queen_of_clubs },
                    { CardRank.King, Properties.Resources.king_of_clubs   }
                }
            },
            {
                CardSuit.Spades, new Dictionary<CardRank, Bitmap>
                {
                    { CardRank.Ace, Properties.Resources.ace_of_spades     },
                    { CardRank.Two, Properties.Resources._2_of_spades      },
                    { CardRank.Three, Properties.Resources._3_of_spades    },
                    { CardRank.Four, Properties.Resources._4_of_spades     },
                    { CardRank.Five, Properties.Resources._5_of_spades     },
                    { CardRank.Six, Properties.Resources._6_of_spades      },
                    { CardRank.Seven, Properties.Resources._7_of_spades    },
                    { CardRank.Eight, Properties.Resources._8_of_spades    },
                    { CardRank.Nine, Properties.Resources._9_of_spades     },
                    { CardRank.Ten, Properties.Resources._10_of_spades     },
                    { CardRank.Jack, Properties.Resources.jack_of_spades   },
                    { CardRank.Queen, Properties.Resources.queen_of_spades },
                    { CardRank.King, Properties.Resources.king_of_spades   }
                }
            },
            {
                CardSuit.Diamonds, new Dictionary<CardRank, Bitmap>
                {
                    { CardRank.Ace, Properties.Resources.ace_of_diamonds     },
                    { CardRank.Two, Properties.Resources._2_of_diamonds      },
                    { CardRank.Three, Properties.Resources._3_of_diamonds    },
                    { CardRank.Four, Properties.Resources._4_of_diamonds     },
                    { CardRank.Five, Properties.Resources._5_of_diamonds     },
                    { CardRank.Six, Properties.Resources._6_of_diamonds      },
                    { CardRank.Seven, Properties.Resources._7_of_diamonds    },
                    { CardRank.Eight, Properties.Resources._8_of_diamonds    },
                    { CardRank.Nine, Properties.Resources._9_of_diamonds     },
                    { CardRank.Ten, Properties.Resources._10_of_diamonds     },
                    { CardRank.Jack, Properties.Resources.jack_of_diamonds   },
                    { CardRank.Queen, Properties.Resources.queen_of_diamonds },
                    { CardRank.King, Properties.Resources.king_of_diamonds   }
                }
            },
            {
                CardSuit.Hearts, new Dictionary<CardRank, Bitmap>
                {
                    { CardRank.Ace, Properties.Resources.ace_of_hearts     },
                    { CardRank.Two, Properties.Resources._2_of_hearts      },
                    { CardRank.Three, Properties.Resources._3_of_hearts    },
                    { CardRank.Four, Properties.Resources._4_of_hearts     },
                    { CardRank.Five, Properties.Resources._5_of_hearts     },
                    { CardRank.Six, Properties.Resources._6_of_hearts      },
                    { CardRank.Seven, Properties.Resources._7_of_hearts    },
                    { CardRank.Eight, Properties.Resources._8_of_hearts    },
                    { CardRank.Nine, Properties.Resources._9_of_hearts     },
                    { CardRank.Ten, Properties.Resources._10_of_hearts     },
                    { CardRank.Jack, Properties.Resources.jack_of_hearts   },
                    { CardRank.Queen, Properties.Resources.queen_of_hearts },
                    { CardRank.King, Properties.Resources.king_of_hearts   }
                }
            }
        };

        public static Bitmap CardImage(CardSuit s, CardRank r)
        {
            Bitmap b;
            Dictionary<CardRank, Bitmap> d = new Dictionary<CardRank, Bitmap>();

            if(CardImageMap.TryGetValue(s, out d))
            {
                if(d.TryGetValue(r, out b))
                {
                    return b;
                }
            }

            return null;
        }
    }
}
