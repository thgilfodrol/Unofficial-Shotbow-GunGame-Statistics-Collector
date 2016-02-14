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
    class Gun
    {
        
        // Private variables to store individual gun data
        private string _name;
        private double _headshots;
        private double _normalKills;
        
        // I'm another constructor
        public Gun(string name)
        {
            Name = name;
            Headshots = 0;
            NormalKills = 0;
        }

        // Moar boilerplates
        /// <summary>
        /// Gets the name of the gun.
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
        /// Gets or sets the total headshots obtained through the gun.
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
        /// Gets or sets the total non-headshot kills obtained through the gun.
        /// </summary>
        public double NormalKills
        {            
            get
            {
                return _normalKills;
            }
            set
            {
                _normalKills = value;
            }
        }

        /// <summary>
        /// Gets the overall total kills obtained through the gun.
        /// </summary>
        public double TotalKills
        {
            get
            {
                return NormalKills + Headshots;
            }
        }

        /// <summary>
        /// Gets the percentage rate of headshots obtained through the gun.
        /// </summary>
        public double HSPercentage
        {
            get
            {
                if (TotalKills == 0) // avoid the ugly NaN monster
                {
                    return TotalKills; // always 0
                }
                return Headshots / TotalKills * 100; // percentage value 
            }
        }

        /// <summary>
        /// Gets the percentage rate of headshots obtained through the gun.
        /// </summary>
        public double NKPercentage
        {
            get
            {
                if (TotalKills == 0) // avoid the ugly NaN monster
                {
                    return TotalKills; // always 0
                }
                return NormalKills / TotalKills * 100; // percentage value (xx.yyyyyyyy%)
            }
        }
    }
}
