using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Tdd
{
    public class Frame
    {
        private List<int> rolls = new List<int>();

        public bool IsStrike => rolls[0] == 10;

        public bool IsSpare => rolls.Count > 1 && rolls.Sum() == 10;

        public bool IsFinished => rolls.Count == 2 || IsStrike;

        private int bonus = 0;

        public void AddBonus(int bonus)
        {
            this.bonus += bonus;
        }

        public int Score => rolls.Sum() + bonus;

        public void Roll(int pins)
        {
            rolls.Add(pins);
        }

        public int NeedBonuses { get; set; }
    }

    
	public class Game
	{
	    private readonly List<Frame> frames = new List<Frame>();
	    private int score = 0;
		public void Roll(int pins)
		{
            CheckSpareOrStrike();
            var currentFrame = frames.LastOrDefault();
		    if (currentFrame == null || currentFrame.IsFinished)
		    {
		        currentFrame = new Frame();
                frames.Add(currentFrame);
		    }

            currentFrame.Roll(pins);
		    score += pins;
		    foreach (var frame in frames.Where(x => x.NeedBonuses > 0))
		    {
		        frame.AddBonus(pins);
		        frame.NeedBonuses--;
		    }
		}

	    private void CheckSpareOrStrike()
	    {
	        var previousFrame = frames.LastOrDefault(x => x.IsFinished);
	        if (previousFrame?.IsSpare == true)
	        {
	            previousFrame.NeedBonuses += 1;	            
	        }
	        else if (previousFrame?.IsStrike == true)
	        {
                previousFrame.NeedBonuses += 2;
	        }
	    }

	    public int GetScore()
		{
			return frames.Sum(x => x.Score);
		}
	}

	[TestFixture]
	public class BowlingGame_GetScore_should
	{
		[Test]
		public void returnZero_beforeAnyRolls()
		{
			var game = new Game();
		    var score = game.GetScore();
            Assert.AreEqual(0, score);
		}


        [Test]
	    public void returnScore_afterFirstRoll()
	    {
	        var game = new Game();
            game.Roll(5);
            Assert.AreEqual(5, game.GetScore());
	    }

        [Test]
	    public void returnCorrectScore_afterSpare()
	    {
	        var game = new Game();
            game.Roll(5);
            game.Roll(5);
            game.Roll(1);
            Assert.AreEqual(12, game.GetScore());
	    }

        [Test]
	    public void returnCorrectScore_afterStrike()
	    {
	        var game = new Game();
            game.Roll(10);
            game.Roll(5);
            game.Roll(1);
            Assert.AreEqual(22, game.GetScore());
	    }

        [Test]
	    public void returnCorrentScore_afterDoubleSpare()
	    {
	        var game = new Game();
            game.Roll(1);
            game.Roll(4);
            game.Roll(4);
            game.Roll(5);
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(5);
            game.Roll(1);
            Assert.AreEqual(41, game.GetScore());
	    }

        [Test]
	    public void returnCorrectScore_afterDoubleSpareAndStrike()
	    {
	        var game = new Game();
            game.Roll(1);
            game.Roll(4);
            game.Roll(4);
            game.Roll(5);
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(5);
            game.Roll(10);
            game.Roll(0);
            game.Roll(1);
            Assert.AreEqual(61, game.GetScore());
        }

        [Test]
	    public void returnCorrectScore_afterWholeGame()
	    {
            var game = new Game();
            game.Roll(1);
            game.Roll(4);
            game.Roll(4);
            game.Roll(5);
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(5);
            game.Roll(10);
            game.Roll(0);
            game.Roll(1);
            game.Roll(7);
            game.Roll(3);
            game.Roll(6);
            game.Roll(4);
            game.Roll(10);
            game.Roll(2);
            game.Roll(8);
            game.Roll(6);
            Assert.AreEqual(133, game.GetScore());
        }

        [Test]
	    public void returnCorrectScore_afterStrikeAndSpare()
	    {
            var game = new Game();
            game.Roll(1);
            game.Roll(4);
            game.Roll(4);
            game.Roll(5);
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(5);
            game.Roll(10);
            game.Roll(0);
            game.Roll(1);
            game.Roll(7);
            game.Roll(3);
            game.Roll(6);
            game.Roll(4);
            game.Roll(10);
            game.Roll(10);
            game.Roll(8);
            game.Roll(6);
            Assert.AreEqual(141, game.GetScore());
        }
        [Test]
	    public void returnCorrectScore_after12strikes()
	    {
	        var game = new Game();
            for (int i = 0; i < 12; i++)
            {
                game.Roll(10);
            }
            Assert.AreEqual(300, game.GetScore());
	    }
        [Test]
	    public void afterTwoStrikeAnd0()
	    {
	        var game = new Game();
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            game.Roll(0);
            game.Roll(0);
            Assert.AreEqual(60, game.GetScore());
	    }
	}
}
