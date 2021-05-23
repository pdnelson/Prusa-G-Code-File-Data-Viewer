using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrusaGCodeFileDataViewer
{
    public class GCodeFile
    {
        // File data
        public string FileName { get; set; }

        // Specifications
        public double FilamentSpoolCost { get; set; }
        public double FilamentUsed { get; set; }
        public double FilamentUsedCost { get; set; }

        public GCodeFile() { }

        /// <summary>
        /// This will tell us if all the fields are populated in a GCodeFile.
        /// </summary>
        /// <returns>True if all fields are populated, false otherwise.</returns>
        public bool AllFieldsPopulated()
        {
            return FilamentUsed != 0 && FilamentUsedCost != 0 && FilamentSpoolCost != 0;
        }
    }
}
