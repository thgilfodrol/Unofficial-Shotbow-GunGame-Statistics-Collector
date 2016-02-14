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

namespace SBGunGameStatisticsCollector
{
    class Player
    {
        // Private variables to store player data
        private string _name;
        private double _headshots;
        private double _normalkills;
        private double _deaths;

        // I'm a constructor
        public Player(string name)
        {
            Name = name;
            Headshots = 0;
            NormalKills = 0;
            Deaths = 0;
        }

        // Boilerplates
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the player's headshot kills.
        /// </summary>
        public double Headshots
        {
            get
            {
                return _headshots;
            }
            set
            {
                _headshots = value;
            }
        }

        /// <summary>
        /// Gets or sets the player's normal kills.
        /// </summary>
        public double NormalKills
        {
            get
            {
                return _normalkills;
            }
            set
            {
                _normalkills = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the player's deaths.
        /// </summary>
        public double Deaths
        {
            get
            {
                return _deaths;
            }
            set
            {
                _deaths = value;
            }
        }

        /// <summary>
        /// Gets the player's KDR (and prevent another shotbow related NaN issue while doing so).
        /// </summary>
        public double KDR
        {
            get
            {
                if (Deaths == 0)
                {
                    return Headshots + NormalKills;
                }
                return (Headshots + NormalKills) / Deaths;
            }            
        }

        /// <summary>
        /// Gets the player's average headshot rate (and prevent another shotbow related NaN issue while doing so).
        /// </summary>
        public double HSPercentage
        {
            get
            {
                // headshots / (headshots + normal kills)
                if ((Headshots + NormalKills) == 0)
                {
                    return Headshots * 100;
                }
                return (Headshots / (Headshots + NormalKills)) * 100;
            }
        }

        /// <summary>
        /// Gets the player's average headshot rate (and prevent another shotbow related NaN issue while doing so).
        /// </summary>
        public double NKPercentage
        {
            get
            {
                // headshots / (headshots + normal kills)
                if ((Headshots + NormalKills) == 0)
                {
                    return NormalKills * 100;
                }
                return (NormalKills / (Headshots + NormalKills)) * 100;
            }
        }
    }
}
