// Licensed under the MIT License.
/* Copyright (c) 2016 ThgilFoDrol.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */
// This program compiles a summary of various statistics from Shotbow's Gun Game via the Minecraft client logs.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace SBGunGameStatisticsCollector
{
    class Program
    {
        // Store pattern for kill feeds
        const string PAT_NORMAL_KILL = @"\[\d\d:\d\d:\d\d\] \[Client thread\/INFO\]: \[CHAT\] ([\w]{2,16}) was killed by ([\w]{2,16})'s ([-\w \(\).]{1,30})";
        const string PAT_HEADSHOT_KILL = @"\[\d\d:\d\d:\d\d\] \[Client thread\/INFO\]: \[CHAT\] ([\w]{2,16}) was shot in the head by ([\w]{2,16})'s ([-\w \(\).]{1,30})";
        // Player list
        static List<Player> PlayerList = new List<Player>();
        // Gun list
        static List<Gun> GunList = new List<Gun>();
        // Output file relative path
        const string OUTPUT_FILE = "output.txt";

        static void Main(string[] args)
        {
            // Local variables
            #region
            // Store input file path
            string inputfile;
            // Store various statistics
            double TotalNormalKills = 0;
            double TotalHeadshots = 0;
            #endregion
            // open source liberal MIT license header
            Console.WriteLine("Copyright (c) 2016 ThgilFoDrol.");
            Console.WriteLine("Please refer to the LICENSE.txt bundled with this software for complete documentation of the MIT License used by this software.");
            Console.WriteLine("By using this software, you consent to the terms of the MIT License.");
            Console.WriteLine("-----");
            Console.WriteLine("Welcome to the UNOFFICIAL ShotBow Gun Game Statistics Tracker!");
            Console.WriteLine("This program and its creator is not affiliated with the Shotbow Network or Mojang.");
            Console.WriteLine("You will be prompted to enter a file name until a valid one is entered.");
            do // patiently wait while I enter a valid file with my butterfingers
            {
                Console.WriteLine("Please enter the name of the .log file from the client (must be a relative path).");
                inputfile = Console.ReadLine();
                Console.WriteLine("-----");
            } while (!File.Exists(inputfile));
            
            // delete previous file that I carelessly left there
            if (File.Exists(OUTPUT_FILE))
            {
                File.Delete(OUTPUT_FILE); // bye bye!
            }

            try
            {
                if (File.Exists(inputfile)) // check if file still exists
                {
                    Console.WriteLine("File named {0} was found! :D", inputfile);
                    Console.WriteLine("Unhandled exception: NullReferenceException at im.just.trolling.myself...carry.on");
                    using (StreamReader sr = new StreamReader(inputfile))
                    {
                        int counter = 0;
                        while (sr.Peek() != -1) // waste cpu cycles
                        {
                            counter += 1;
                            if (counter >= 10)
                            {
                                // Print statistics to console
                                // Don't think I'll actually end up needing this
                                counter = 0;
                            }
                            // Store line
                            string line = sr.ReadLine();
                            // Regex stuff
                            Regex regexhs;
                            Regex regexnk;
                            Match matchhs;
                            Match matchnk;

                            // Constructing with pattern
                            regexhs = new Regex(PAT_HEADSHOT_KILL);
                            regexnk = new Regex(PAT_NORMAL_KILL);

                            if (regexhs.IsMatch(line)) // if it was a headshot kill
                            {
                                TotalHeadshots += 1; // increment 1 ---DEPRECATED---
                                matchhs = regexhs.Match(line);
                                string gun = matchhs.Groups[3].ToString(); // retrieve gun type

                                // Increment stat for killer
                                IncrementPlayerKills(matchhs.Groups[2].ToString(), 0);
                                // Increment victim death counter by 1
                                IncrementPlayerDeaths(matchhs.Groups[1].ToString());

                                // Increment headshot counter for gun
                                IncrementGunKills(matchhs.Groups[3].ToString(), 0);                                
                            }
                            else if (regexnk.IsMatch(line)) // if it was a normal kill
                            {
                                TotalNormalKills += 1; // increment 1 ---DEPRECATED---
                                matchnk = regexnk.Match(line);
                                string gun = matchnk.Groups[3].ToString(); // retrieve gun type

                                // Increment stat for killer
                                IncrementPlayerKills(matchnk.Groups[2].ToString(), 1);
                                // Increment victim death counter by 1
                                IncrementPlayerDeaths(matchnk.Groups[1].ToString());

                                // Increment normal kill counter for gun
                                IncrementGunKills(matchnk.Groups[3].ToString(), 1);
                            }
                            
                        }
                        
                        // some random whitespace here

                    }
                }
            }
            catch (Exception e) // catch stuff
            {                
                Console.WriteLine("Encountered a fatal error: {0}", e.ToString()); // print stuff
            }

            // Write stuff // TODO: proper abstraction of math to somewhere outside of the writing stuff
            Log("Total headshots: " + TotalHeadshots.ToString() + ", accounting for " + ((TotalHeadshots / (TotalNormalKills + TotalHeadshots)) * 100).ToString() + "% of kills");
            Log("Total normal kills: " + TotalNormalKills.ToString() + ", accounting for " + ((TotalNormalKills / (TotalNormalKills + TotalHeadshots)) * 100).ToString() + "% of kills");
            Log("Grand total: " + (TotalNormalKills + TotalHeadshots).ToString());
            Log(""); // a blank line
            // the detailed overall gun breakdown :D
            Log("-----[DETAILED OVERALL GUN BREAKDOWN]-----"); 
            Log("FORMAT: WEAPON|HEADSHOTS (PERCENTAGE)|NORMALKILLS (PERCENTAGE)|TOTAL");
            foreach (Gun gun in GunList) // loop through all guns
            {
                Log(gun.Name + "|Headshots: " + gun.Headshots.ToString() + "(" + gun.HSPercentage.ToString() + "%)|Normal kills: " + gun.NormalKills.ToString() + "(" + gun.NKPercentage.ToString() + "%)|Total: " + gun.TotalKills.ToString());
            }            
            // Write moar player specific stuff
            Log("");
            Log("-----[DETAILED OVERALL PLAYER STATISTICS]-----");
            Log("FORMAT: PLAYER|HEADSHOTS (PERCENTAGE)|NORMALKILLS (PERCENTAGE)|TOTAL KILLS/TOTAL DEATHS (KDR)");
            foreach (Player player in PlayerList) // loop through all players
            {
                Log(CalculateSHA1(player.Name) + "|HS: " + player.Headshots.ToString() + "(" + player.HSPercentage.ToString() + "%)|NK: " + player.NormalKills.ToString() + "(" + player.NKPercentage.ToString() + "%)|KDR: " + (player.Headshots + player.NormalKills).ToString() + "/" + player.Deaths.ToString() + " (" + player.KDR.ToString() + ")");
            }

            // All done!
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); // exit stuff
        }

        /// <summary>
        /// Write to output file and print to console.
        /// </summary>
        /// <param name="line">The line to write.</param>
        private static void Log(string line)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(OUTPUT_FILE, true))
                {
                    sw.WriteLine(line);
                }
                Console.WriteLine(line);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encountered a fatal error: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Increment the player's kill statistic by 1. If player does not exist, it is created and statistic is incremented.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="type">The statistic to increment (0 for headshots, otherwise it is a normal kill).</param>
        private static void IncrementPlayerKills(string name, int type)
        {
            // Increment if player exists
            foreach (Player player in PlayerList)
            {
                if (player.Name == name) // player exists already
                {
                    // Add the statistic
                    if (type == 0) // if headshot
                    {
                        player.Headshots += 1; // increment 1
                    }
                    else // if normal kill
                    {
                        player.NormalKills += 1; // increment 1
                    }
                    return;
                }
            }

            // If player doesn't exist
            PlayerList.Add(new Player(name)); // create player
            var newplayer = PlayerList.Last();
            // Add the statistic
            if (type == 0) // if headshot
            {
                newplayer.Headshots += 1; // increment 1
            }
            else // if normal kill
            {
                newplayer.NormalKills += 1; // increment 1
            }
            return;
        }

        /// <summary>
        /// Increment player's death counter by 1. If player does not exist, it is created and added.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        private static void IncrementPlayerDeaths(string name)
        {
            // Increment if player exists
            foreach (Player player in PlayerList)
            {
                if (player.Name == name) // player exists already
                {                    
                    player.Deaths += 1; // increment 1
                    return;
                }
            }

            // If player doesn't exist
            PlayerList.Add(new Player(name)); // create player
            var newplayer = PlayerList.Last();
            newplayer.Deaths += 1; // increment 1
            return;

        }

        /// <summary>
        /// Increment the kill statistic by 1 for the specified gun. If gun does not exist, it is created and statistic is incremented.
        /// </summary>
        /// <param name="name">The name of the gun.</param>
        /// <param name="type">The statistic to increment (0 for headshots, otherwise it is a normal kill).</param>
        private static void IncrementGunKills(string name, int type)
        {
            // Increment if gun exists
            foreach (Gun gun in GunList)
            {
                if (gun.Name == name) // gun exists already
                {
                    // Add the statistic
                    if (type == 0) // if headshot
                    {
                        gun.Headshots += 1; // increment 1
                    }
                    else // if normal kill
                    {
                        gun.NormalKills += 1; // increment 1
                    }
                    //Log("[DEBUG] Gun name: " + gun.Name + "| Gun headshots: " + gun.Headshots.ToString() + "| Gun normal kills: " + gun.NormalKills.ToString() + "|");
                    return;
                }
            }

            // If gun doesn't exist   
            GunList.Add(new Gun(name)); // create gun         
            Gun newgun = GunList.Last();
            // Add the statistic
            if (type == 0) // if headshot
            {
                newgun.Headshots += 1; // increment 1
            }
            else // if normal kill
            {
                newgun.NormalKills += 1; // increment 1
            }            
            //Log("[DEBUG] Gun name: " + newgun.Name + "| Gun headshots: " + newgun.Headshots.ToString() + "| Gun normal kills: " + newgun.NormalKills.ToString() + "|");
            return;
        }

        // Slightly modified stackoverflow code for sha1
        public static string CalculateSHA1(string text)
        {
            Encoding enc = Encoding.GetEncoding(1252);
            byte[] buffer = enc.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
        }
    }








































































































    //                                                                _-_   -----
    //                                                               / | \  |   |
    // ======================== DIAMOND ORE ========================   |    -----
    //                                                                 |      |
    //                                                                 =======|      <----- Bob the xrayer
    //                                                                        |
    //                                                                       / \























    // ==================================== BEDROCK ====================================
}
