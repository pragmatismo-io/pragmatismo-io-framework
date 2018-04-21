/*****************************************************************************\
*
* Copyright (c) Pragmatismo.io. All rights reserved.
* Licensed under the MIT license.
*
* Pragmatismo.io: http://pragmatismo.io
*
* MIT License:
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
* 
\*****************************************************************************/

namespace Pragmatismo.Io.Framework.Helpers
{
    /// <summary>
    /// Class for manipulating emoticons.
    /// </summary>
    public static class Emoticons
    {
        
        /// <summary>
        /// Emoticons
        /// </summary>
        enum Emoticon
        {
            Telephone,
            Computer,
            OK,
            Beer,
            Sunglasses,
            Smile,
            BigSmile,
            Wink,
            TongeOut,
            Embarassed,
            GottaRun,
            Nerd,
            BrokenHeart,
            Sleepy,
            Sun,
            HalfMoon,
            Clock,
            Thinking,
            Exclamation,
            Sad,
            WorkItem,
            Girl,
            Boy,
            Snail,
            Bye,
            Online,
            Busy,
            QuestionMark,
            Gift,
            Agree,
            Email,
            Umbrella,
            Storm,
            Money,
            CoffeeCup,
            Idea,
            Star,
            Surprised,
            HotSmile,
            AngrySmile,
            ConfusedSmile,
            Crying,
            Disappointed,
            BaringTeeth,
            Sick,
            Party,
            DontTellAnyone,
            Secret,
            Sarcastic,
            Heart,
            Drinks,
            Catface,
            Dogface,
            BlackSheep,
            Rainbow,
            LeftHug,
            RightHug,
            RedLips,
            RedRose,
            WiltedRose,
            BirthdayCake,
            Camera,
            MobilePhone,
            Auto,
            Airplane,
            Filmstrip,
            Note,
            Pizza,
            SoccerBall,
            IslandWithPalmTree,
            Stop,
            Confidential,
            Disagree,
            WorkFromHome,
            HoldOn,
            GoodLuck,
            CanICallYou,
            DoNotDisturb,
            Games,
            EyeRolling,
            LetsMeet,
        }


        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Emoticons"/> are enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Initializes the <see cref="Emoticons"/> class.
        /// </summary>
        static Emoticons()
        {
            Emoticons.Enabled = true;
        }

        /// <summary>
        /// Gets the telephone emoticon.
        /// </summary>
        /// <value>The telephone emoticon.</value>
        public static string Telephone
        {
            get
            {
                return GetEmoticon(Emoticon.Telephone);
            }
        }

        /// <summary>
        /// Gets the broken heart emoticon.
        /// </summary>
        /// <value>The broken heart emoticon.</value>
        public static string BrokenHeart
        {
            get
            {
                return GetEmoticon(Emoticon.BrokenHeart);
            }
        }

        /// <summary>
        /// Gets the OK emoticon.
        /// </summary>
        /// <value>The OK emoticon.</value>
        public static string OK
        {
            get
            {
                return GetEmoticon(Emoticon.OK);
            }
        }

        /// <summary>
        /// Gets the gotta run emoticon.
        /// </summary>
        /// <value>The gotta run emoticon.</value>
        public static string GottaRun
        {
            get
            {
                return GetEmoticon(Emoticon.GottaRun);
            }
        }


        /// <summary>
        /// Gets the nerd emoticon.
        /// </summary>
        /// <value>The nerd emoticon.</value>
        public static string Nerd
        {
            get
            {
                return GetEmoticon(Emoticon.Nerd);
            }
        }

        /// <summary>
        /// Gets the clock emoticon.
        /// </summary>
        /// <value>The clock emoticon.</value>
        public static string Clock
        {
            get
            {
                return GetEmoticon(Emoticon.Clock);
            }
        }


        /// <summary>
        /// Gets the beer emoticon.
        /// </summary>
        /// <value>The beer emoticon.</value>
        public static string Beer
        {
            get
            {
                return GetEmoticon(Emoticon.Beer);
            }
        }

        /// <summary>
        /// Gets the sunglasses emoticon.
        /// </summary>
        /// <value>The sunglasses emoticon.</value>
        public static string Sunglasses
        {
            get
            {
                return GetEmoticon(Emoticon.Sunglasses);
            }
        }


        /// <summary>
        /// Gets the smile emoticon.
        /// </summary>
        /// <value>The smile emoticon.</value>
        public static string Smile
        {
            get
            {
                return GetEmoticon(Emoticon.Smile);
            }
        }


        /// <summary>
        /// Gets the big smile emoticon.
        /// </summary>
        /// <value>The big smile emoticon.</value>
        public static string BigSmile
        {
            get
            {
                return GetEmoticon(Emoticon.BigSmile);
            }
        }


        /// <summary>
        /// Gets the wink emoticon.
        /// </summary>
        /// <value>The wink emoticon.</value>
        public static string Wink
        {
            get
            {
                return GetEmoticon(Emoticon.Wink);
            }
        }


        /// <summary>
        /// Gets the embarassed emoticon.
        /// </summary>
        /// <value>The embarassed emoticon.</value>
        public static string Embarassed
        {
            get
            {
                return GetEmoticon(Emoticon.Embarassed);
            }
        }


        /// <summary>
        /// Gets the tongue out emoticon.
        /// </summary>
        /// <value>The tongue out emoticon.</value>
        public static string TongueOut
        {
            get
            {
                return GetEmoticon(Emoticon.TongeOut);
            }
        }

        /// <summary>
        /// Gets the sleepy emoticon.
        /// </summary>
        /// <value>The sleepy emoticon.</value>
        public static string Sleepy
        {
            get
            {
                return GetEmoticon(Emoticon.Sleepy);
            }
        }

        /// <summary>
        /// Gets the sun emoticon.
        /// </summary>
        /// <value>The sun emoticon.</value>
        public static string Sun
        {
            get
            {
                return GetEmoticon(Emoticon.Sun);
            }
        }

        /// <summary>
        /// Gets the half moon emoticon.
        /// </summary>
        /// <value>The half moon emoticon.</value>
        public static string HalfMoon
        {
            get
            {
                return GetEmoticon(Emoticon.HalfMoon);
            }
        }

        /// <summary>
        /// Gets the thinking emoticon.
        /// </summary>
        /// <value>The thinking emoticon.</value>
        public static string Thinking
        {
            get
            {
                return GetEmoticon(Emoticon.Thinking);
            }
        }

        /// <summary>
        /// Gets the exclamation emoticon.
        /// </summary>
        /// <value>The exclamation emoticon.</value>
        public static string Exclamation
        {
            get
            {
                return GetEmoticon(Emoticon.Exclamation);
            }
        }

        /// <summary>
        /// Gets the sad emoticon.
        /// </summary>
        /// <value>The sad emoticon.</value>
        public static string Sad
        {
            get
            {
                return GetEmoticon(Emoticon.Sad);
            }
        }

        /// <summary>
        /// Gets the work item emoticon.
        /// </summary>
        /// <value>The work item emoticon.</value>
        public static string WorkItem
        {
            get
            {
                return GetEmoticon(Emoticon.WorkItem);
            }
        }

        /// <summary>
        /// Gets the girl emoticon.
        /// </summary>
        /// <value>The girl emoticon.</value>
        public static string Girl
        {
            get
            {
                return GetEmoticon(Emoticon.Girl);
            }
        }

        /// <summary>
        /// Gets the boy emoticon.
        /// </summary>
        /// <value>The boy emoticon.</value>
        public static string Boy
        {
            get
            {
                return GetEmoticon(Emoticon.Boy);
            }
        }

        /// <summary>
        /// Gets the snail emoticon.
        /// </summary>
        /// <value>The snail emoticon.</value>
        public static string Snail
        {
            get
            {
                return GetEmoticon(Emoticon.Snail);
            }
        }

        /// <summary>
        /// Gets the bye emoticon.
        /// </summary>
        /// <value>The bye emoticon.</value>
        public static string Bye
        {
            get
            {
                return GetEmoticon(Emoticon.Bye);
            }
        }

        /// <summary>
        /// Gets the online emoticon.
        /// </summary>
        /// <value>The online emoticon.</value>
        public static string Online
        {
            get
            {
                return GetEmoticon(Emoticon.Online);
            }
        }

        /// <summary>
        /// Gets the busy emoticon.
        /// </summary>
        /// <value>The busy emoticon.</value>
        public static string Busy
        {
            get
            {
                return GetEmoticon(Emoticon.Busy);
            }
        }

        /// <summary>
        /// Gets the question mark emoticon.
        /// </summary>
        /// <value>The question mark emoticon.</value>
        public static string QuestionMark
        {
            get
            {
                return GetEmoticon(Emoticon.QuestionMark);
            }
        }

        /// <summary>
        /// Gets the gift emoticon.
        /// </summary>
        /// <value>The gift emoticon.</value>
        public static string Gift
        {
            get
            {
                return GetEmoticon(Emoticon.Gift);
            }
        }


        /// <summary>
        /// Gets the agree emoticon.
        /// </summary>
        /// <value>The agree emoticon.</value>
        public static string Agree
        {
            get
            {
                return GetEmoticon(Emoticon.Agree);
            }
        }

        /// <summary>
        /// Gets the email emoticon.
        /// </summary>
        /// <value>The email emoticon.</value>
        public static string Email
        {
            get
            {
                return GetEmoticon(Emoticon.Email);
            }
        }

        /// <summary>
        /// Gets the umbrella emoticon.
        /// </summary>
        /// <value>The umbrella emoticon.</value>
        public static string Umbrella
        {
            get
            {
                return GetEmoticon(Emoticon.Umbrella);
            }
        }

        /// <summary>
        /// Gets the storm emoticon.
        /// </summary>
        /// <value>The storm emoticon.</value>
        public static string Storm
        {
            get
            {
                return GetEmoticon(Emoticon.Storm);
            }
        }

        /// <summary>
        /// Gets the money emoticon.
        /// </summary>
        /// <value>The money emoticon.</value>
        public static string Money
        {
            get
            {
                return GetEmoticon(Emoticon.Money);
            }
        }

        /// <summary>
        /// Gets the coffee cup emoticon.
        /// </summary>
        /// <value>The coffee cup emoticon.</value>
        public static string CoffeeCup
        {
            get
            {
                return GetEmoticon(Emoticon.CoffeeCup);
            }
        }

        /// <summary>
        /// Gets the idea emoticon.
        /// </summary>
        /// <value>The idea emoticon.</value>
        public static string Idea
        {
            get
            {
                return GetEmoticon(Emoticon.Idea);
            }
        }

        /// <summary>
        /// Gets the computer emoticon.
        /// </summary>
        /// <value>The computer emoticon.</value>
        public static string Computer
        {
            get
            {
                return GetEmoticon(Emoticon.Computer);
            }
        }

        /// <summary>
        /// Gets the star emoticon.
        /// </summary>
        /// <value>The star emoticon.</value>
        public static string Star
        {
            get
            {
                return GetEmoticon(Emoticon.Star);
            }
        }

        /// <summary>
        /// Gets the surprised emoticon.
        /// </summary>
        /// <value>The surprised emoticon.</value>
        public static string Surprised
        {
            get
            {
                return GetEmoticon(Emoticon.Surprised);
            }
        }

        /// <summary>
        /// Gets the hot smiley emoticon.
        /// </summary>
        /// <value>The hot smiley emoticon.</value>
        public static string HotSmile
        {
            get
            {
                return GetEmoticon(Emoticon.HotSmile);
            }
        }

        /// <summary>
        /// Gets the angry smiley emoticon.
        /// </summary>
        /// <value>The angry smiley emoticon.</value>
        public static string AngrySmile
        {
            get
            {
                return GetEmoticon(Emoticon.AngrySmile);
            }
        }

        /// <summary>
        /// Gets the confused smiley emoticon.
        /// </summary>
        /// <value>The confused smiley emoticon.</value>
        public static string ConfusedSmile
        {
            get
            {
                return GetEmoticon(Emoticon.ConfusedSmile);
            }
        }

        /// <summary>
        /// Gets the crying emoticon.
        /// </summary>
        /// <value>The crying emoticon.</value>
        public static string Crying
        {
            get
            {
                return GetEmoticon(Emoticon.Crying);
            }
        }

        /// <summary>
        /// Gets the disappointed emoticon.
        /// </summary>
        /// <value>The disappointed emoticon.</value>
        public static string Disappointed
        {
            get
            {
                return GetEmoticon(Emoticon.Disappointed);
            }
        }

        /// <summary>
        /// Gets the baring teeth emoticon.
        /// </summary>
        /// <value>The baring teeth emoticon.</value>
        public static string BaringTeeth
        {
            get
            {
                return GetEmoticon(Emoticon.BaringTeeth);
            }
        }

        /// <summary>
        /// Gets the sick smiley emoticon.
        /// </summary>
        /// <value>The sick smiley emoticon.</value>
        public static string Sick
        {
            get
            {
                return GetEmoticon(Emoticon.Sick);
            }
        }

        /// <summary>
        /// Gets the party smiley emoticon.
        /// </summary>
        /// <value>The party smiley emoticon.</value>
        public static string Party
        {
            get
            {
                return GetEmoticon(Emoticon.Party);
            }
        }

        /// <summary>
        /// Gets the don't tell anyone smiley emoticon.
        /// </summary>
        /// <value>The don't tell anyone smiley emoticon.</value>
        public static string DontTellAnyone
        {
            get
            {
                return GetEmoticon(Emoticon.DontTellAnyone);
            }
        }

        /// <summary>
        /// Gets the secret telling smiley emoticon.
        /// </summary>
        /// <value>The secret telling smiley emoticon.</value>      
        public static string Secret
        {
            get
            {
                return GetEmoticon(Emoticon.Secret);
            }
        }

        /// <summary>
        /// Gets the sarcastic smiley emoticon.
        /// </summary>
        /// <value>The sarcastic smiley emoticon.</value>
        public static string Sarcastic
        {
            get
            {
                return GetEmoticon(Emoticon.Sarcastic);
            }
        }

        /// <summary>
        /// Gets the red heart emoticon.
        /// </summary>
        /// <value>The red heart emoticon.</value>
        public static string Heart
        {
            get
            {
                return GetEmoticon(Emoticon.Heart);
            }
        }

        /// <summary>
        /// Gets the drinks emoticon.
        /// </summary>
        /// <value>The drinks emoticon.</value>
        public static string Drinks
        {
            get
            {
                return GetEmoticon(Emoticon.Drinks);
            }
        }

        /// <summary>
        /// Gets the cat face smiley emoticon.
        /// </summary>
        /// <value>The cat face smiley emoticon.</value>
        public static string Catface
        {
            get
            {
                return GetEmoticon(Emoticon.Catface);
            }
        }

        /// <summary>
        /// Gets the dog face smiley emoticon.
        /// </summary>
        /// <value>The dog face smiley emoticon.</value>
        public static string Dogface
        {
            get
            {
                return GetEmoticon(Emoticon.Dogface);
            }
        }

        /// <summary>
        /// Gets the black sheep emoticon.
        /// </summary>
        /// <value>The black sheep smiley emoticon.</value>
        public static string BlackSheep
        {
            get
            {
                return GetEmoticon(Emoticon.BlackSheep);
            }
        }

        /// <summary>
        /// Gets the rainbow emoticon.
        /// </summary>
        /// <value>The rainbow emoticon.</value>
        public static string Rainbow
        {
            get
            {
                return GetEmoticon(Emoticon.Rainbow);
            }
        }
        
        /// <summary>
        /// Gets the left hug emoticon.
        /// </summary>
        /// <value>The left hug emoticon.</value>
        public static string LeftHug
        {
            get
            {
                return GetEmoticon(Emoticon.LeftHug);
            }
        }

        /// <summary>
        /// Gets the right hug emoticon.
        /// </summary>
        /// <value>The right hug emoticon.</value>
        public static string RightHug
        {
            get
            {
                return GetEmoticon(Emoticon.RightHug);
            }
        }

        /// <summary>
        /// Gets the red lips emoticon.
        /// </summary>
        /// <value>The red lips emoticon.</value>
        public static string RedLips
        {
            get
            {
                return GetEmoticon(Emoticon.RedLips);
            }
        }

        /// <summary>
        /// Gets the red rose emoticon.
        /// </summary>
        /// <value>The red rose emoticon.</value>
        public static string RedRose
        {
            get
            {
                return GetEmoticon(Emoticon.RedRose);
            }
        }

        /// <summary>
        /// Gets the wilted rose emoticon.
        /// </summary>
        /// <value>The wilted rose emoticon.</value>
        public static string WiltedRose
        {
            get
            {
                return GetEmoticon(Emoticon.WiltedRose);
            }
        }

        /// <summary>
        /// Gets the birthday cake emoticon.
        /// </summary>
        /// <value>The birthday cake emoticon.</value>
        public static string BirthdayCake
        {
            get
            {
                return GetEmoticon(Emoticon.BirthdayCake);
            }
        }

        /// <summary>
        /// Gets the camera emoticon.
        /// </summary>
        /// <value>The camera emoticon.</value>
        public static string Camera
        {
            get
            {
                return GetEmoticon(Emoticon.Camera);
            }
        }

        /// <summary>
        /// Gets the mobile phone emoticon.
        /// </summary>
        /// <value>The mobile phone emoticon.</value>
        public static string MobilePhone
        {
            get
            {
                return GetEmoticon(Emoticon.MobilePhone);
            }
        }

        /// <summary>
        /// Gets the auto emoticon.
        /// </summary>
        /// <value>The auto emoticon.</value>
        public static string Auto
        {
            get
            {
                return GetEmoticon(Emoticon.Auto);
            }
        }

        /// <summary>
        /// Gets the airplane emoticon.
        /// </summary>
        /// <value>The airplane emoticon.</value>
        public static string Airplane
        {
            get
            {
                return GetEmoticon(Emoticon.Airplane);
            }
        }

        /// <summary>
        /// Gets the film strip emoticon.
        /// </summary>
        /// <value>The film strip emoticon.</value>
        public static string Filmstrip
        {
            get
            {
                return GetEmoticon(Emoticon.Filmstrip);
            }
        }

        /// <summary>
        /// Gets the note emoticon.
        /// </summary>
        /// <value>The note emoticon.</value>
        public static string Note
        {
            get
            {
                return GetEmoticon(Emoticon.Note);
            }
        }

        /// <summary>
        /// Gets the pizza emoticon.
        /// </summary>
        /// <value>The pizza emoticon.</value>
        public static string Pizza
        {
            get
            {
                return GetEmoticon(Emoticon.Pizza);
            }
        }

        /// <summary>
        /// Gets the soccer ball emoticon.
        /// </summary>
        /// <value>The soccer ball emoticon.</value>
        public static string SoccerBall
        {
            get
            {
                return GetEmoticon(Emoticon.SoccerBall);
            }
        }

        /// <summary>
        /// Gets the island with palm tree emoticon.
        /// </summary>
        /// <value>The island with palm tree emoticon.</value>
        public static string IslandWithPalmTree
        {
            get
            {
                return GetEmoticon(Emoticon.IslandWithPalmTree);
            }
        }
        
        /// <summary>
        /// Gets the stop emoticon.
        /// </summary>
        /// <value>The stop emoticon.</value>
        public static string Stop
        {
            get
            {
                return GetEmoticon(Emoticon.Stop);
            }
        }
        
        /// <summary>
        /// Gets the confidential emoticon.
        /// </summary>
        /// <value>The confidential emoticon.</value>
        public static string Confidential
        {
            get
            {
                return GetEmoticon(Emoticon.Confidential);
            }
        }


        /// <summary>
        /// Gets the disagree emoticon.
        /// </summary>
        /// <value>The disagree emoticon.</value>
        public static string Disagree
        {
            get
            {
                return GetEmoticon(Emoticon.Disagree);
            }
        }
        
        /// <summary>
        /// Gets the work from home emoticon.
        /// </summary>
        /// <value>The work from home emoticon.</value>
        public static string WorkFromHome
        {
            get
            {
                return GetEmoticon(Emoticon.WorkFromHome);
            }
        }
        
        /// <summary>
        /// Gets the hold on emoticon.
        /// </summary>
        /// <value>The hold on emoticon.</value>
        public static string HoldOn
        {
            get
            {
                return GetEmoticon(Emoticon.HoldOn);
            }
        }

        /// <summary>
        /// Gets the good luck emoticon.
        /// </summary>
        /// <value>The good luck emoticon.</value>
        public static string GoodLuck
        {
            get
            {
                return GetEmoticon(Emoticon.GoodLuck);
            }
        }

        /// <summary>
        /// Gets the can I call you emoticon.
        /// </summary>
        /// <value>The can I call you emoticon.</value>
        public static string CanICallYou
        {
            get
            {
                return GetEmoticon(Emoticon.CanICallYou);
            }
        }

        /// <summary>
        /// Gets the do not disturb emoticon.
        /// </summary>
        /// <value>The do not disturb emoticon.</value>
        public static string DoNotDisturb
        {
            get
            {
                return GetEmoticon(Emoticon.DoNotDisturb);
            }
        }

        /// <summary>
        /// Gets the games emoticon.
        /// </summary>
        /// <value>The games emoticon.</value>
        public static string Games
        {
            get
            {
                return GetEmoticon(Emoticon.Games);
            }
        }

        /// <summary>
        /// Gets the eye rolling emoticon.
        /// </summary>
        /// <value>The eye rolling emoticon.</value>
        public static string EyeRolling
        {
            get
            {
                return GetEmoticon(Emoticon.EyeRolling);
            }
        }

        /// <summary>
        /// Gets the let's meet emoticon.
        /// </summary>
        /// <value>The let's meet emoticon.</value>
        public static string LetsMeet
        {
            get
            {
                return GetEmoticon(Emoticon.LetsMeet);
            }
        }

        /// <summary>
        /// Gets the emoticon as string.
        /// </summary>
        /// <param name="emoticon">The emoticon.</param>
        private static string GetEmoticon(Emoticon emoticon)
        {
            string result = string.Empty;
            if (Enabled)
            {
                switch (emoticon)
                {                   
                    case Emoticon.Telephone:
                        result = "(T)";
                        break;
                    case Emoticon.OK:
                        result = "(OK)";
                        break;
                    case Emoticon.Beer:
                        result = "(b)";
                        break;
                    case Emoticon.Sunglasses:
                        result = "(H)";
                        break;
                    case Emoticon.Smile:
                        result = ":)";
                        break;
                    case Emoticon.Surprised:
                        result = ":-O";
                        break;
                    case Emoticon.BigSmile:
                        result = ":D";
                        break;
                    case Emoticon.TongeOut:
                        result = ":P";
                        break;
                    case Emoticon.Embarassed:
                        result = ":$";
                        break;
                    case Emoticon.Wink:
                        result = ";)";
                        break;
                    case Emoticon.GottaRun:
                        result = "(gtr)";
                        break;
                    case Emoticon.Nerd:
                        result = "8-|";
                        break;
                    case Emoticon.BrokenHeart:
                        result = "(U)";
                        break;
                    case Emoticon.Computer:
                        result = "(co)";
                        break;
                    case Emoticon.Sun:
                        result = "(#)";
                        break;
                    case Emoticon.HalfMoon:
                        result = "(s)";
                        break;
                    case Emoticon.Clock:
                        result = "(O)";
                        break;
                    case Emoticon.Thinking:
                        result = "*-)";
                        break;
                    case Emoticon.Exclamation:
                        result = "(!!?)";
                        break;
                    case Emoticon.Sad:
                        result = ":(";
                        break;
                    case Emoticon.WorkItem:
                        result = "(woi)";
                        break;
                    case Emoticon.Girl:
                        result = "(X)";
                        break;
                    case Emoticon.Boy:
                        result = "(Z)";
                        break;
                    case Emoticon.Snail:
                        result = "(sn)";
                        break;
                    case Emoticon.Bye:
                        result = "(BYE)";
                        break;
                    case Emoticon.Online:
                        result = "(ol)";
                        break;
                    case Emoticon.Busy:
                        result = "(busy)";
                        break;
                    case Emoticon.QuestionMark:
                        result = "(!!)";
                        break;
                    case Emoticon.Gift:
                        result = "(G)";
                        break;
                    case Emoticon.Agree:
                        result = "(Y)";
                        break;
                    case Emoticon.Email:
                        result = "(E)";
                        break;
                    case Emoticon.Sleepy:
                        result = "|-)";
                        break;
                    case Emoticon.Umbrella:
                        result = "(um)";
                        break;
                    case Emoticon.Storm:
                        result = "(ST)";
                        break;
                    case Emoticon.Money:
                        result = "(mo)";
                        break;
                    case Emoticon.CoffeeCup:
                        result = "(C)";
                        break;
                    case Emoticon.Idea:
                        result = "(I)";
                        break;
                    case Emoticon.Star:
                        result = "(*)";
                        break;
                    case Emoticon.HotSmile:
                        result = "(H)";
                        break;
                    case Emoticon.AngrySmile: 
                        result = ":@"; 
                        break;
                    case Emoticon.ConfusedSmile: 
                        result = ":S"; 
                        break;
                    case Emoticon.Crying: 
                        result = ":'("; 
                        break;
                    case Emoticon.Disappointed: 
                        result = ":|"; 
                        break;
                    case Emoticon.BaringTeeth: 
                        result = "8o|"; 
                        break;
                    case Emoticon.Sick: 
                        result = "+o("; 
                        break;
                    case Emoticon.Party: 
                        result = "<:o)"; 
                        break;
                    case Emoticon.DontTellAnyone: 
                        result = ":-#"; 
                        break;
                    case Emoticon.Secret: 
                        result = ":-*"; 
                        break;
                    case Emoticon.Sarcastic: 
                        result = "^o)"; 
                        break;
                    case Emoticon.Heart: 
                        result = "(L)"; 
                        break;
                    case Emoticon.Drinks: 
                        result = "(D)"; 
                        break;
                    case Emoticon.Catface: 
                        result = "(@)"; 
                        break;
                    case Emoticon.Dogface: 
                        result = "(&)"; 
                        break;
                    case Emoticon.BlackSheep: 
                        result = "(bah)"; 
                        break;
                    case Emoticon.Rainbow: 
                        result = "(R)"; 
                        break;
                    case Emoticon.LeftHug: 
                        result = "({)"; 
                        break;
                    case Emoticon.RightHug: 
                        result = "(})"; 
                        break;
                    case Emoticon.RedLips: 
                        result = "(K)"; 
                        break;
                    case Emoticon.RedRose: 
                        result = "(F)"; 
                        break;
                    case Emoticon.WiltedRose: 
                        result = "(W)";
                        break;
                    case Emoticon.BirthdayCake: 
                        result = "(^)"; break;
                    case Emoticon.Camera: 
                        result = "(P)"; 
                        break;
                    case Emoticon.MobilePhone: 
                        result = "(mp)"; 
                        break;
                    case Emoticon.Auto: 
                        result = "(au)"; 
                        break;
                    case Emoticon.Airplane: 
                        result = "(ap)"; 
                        break;
                    case Emoticon.Filmstrip:
                        result = "(~)";
                        break;
                    case Emoticon.Note: 
                        result = "(8)";
                        break;
                    case Emoticon.Pizza: 
                        result = "(pi)"; 
                        break;
                    case Emoticon.SoccerBall: 
                        result = "(so)";
                        break;
                    case Emoticon.IslandWithPalmTree: 
                        result = "(ip)"; 
                        break;
                    case Emoticon.Stop: 
                        result = "(!)";
                        break;
                    case Emoticon.Confidential: 
                        result = "(QT)";
                        break;
                    case Emoticon.Disagree: 
                        result = "(N)";
                        break;
                    case Emoticon.WorkFromHome: 
                        result = "(@H)";
                        break;
                    case Emoticon.HoldOn: 
                        result = "(W8)";
                        break;
                    case Emoticon.GoodLuck: 
                        result = "(gl)"; 
                        break;
                    case Emoticon.CanICallYou:
                        result = "(cic)"; 
                        break;
                    case Emoticon.DoNotDisturb:
                        result = "(dnd)";
                        break;
                    case Emoticon.Games:
                        result = "(ply)";
                        break;
                    case Emoticon.EyeRolling: 
                        result = "8-)";
                        break;
                    case Emoticon.LetsMeet: 
                        result = "(S+)";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

    }
}
