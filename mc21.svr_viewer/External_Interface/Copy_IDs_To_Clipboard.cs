using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mc21.svr_viewer.External_Interface
{
    static public class Copy_IDs_To_Clipboard
    {
        internal static void Action(List<string> Procedures_IDs)
        {
            IEnumerable<string> input_data = Procedures_IDs.OrderBy(x => x);

            StringBuilder output_text = new StringBuilder();
            foreach (string item in input_data)
                output_text.AppendLine(item);

            if (output_text.Length != 0)
                Clipboard.SetText(output_text.ToString());
        }
    }
}
