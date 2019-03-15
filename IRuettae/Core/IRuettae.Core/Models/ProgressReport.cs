using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    public class ProgressReport
    {
        private double progress;

        /// <summary>
        /// Progress from 0 to 1
        /// </summary>
        public double Progress
        {
            get => progress;
            set
            {
                if (value <= 1 && value >= 0)
                {
                    progress = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(progress), value, "must be between 0 and 1");
                }
            }
        }

        public ProgressReport()
        {

        }

        public ProgressReport(double progress)
        {
            this.progress = progress;
        }

        public override string ToString()
        {
            return progress.ToString("P0");
        }
    }
}