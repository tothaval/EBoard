using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoardSDK
{
    public class EBoardFeedbackMessage
    {
        public required EBoardTaskResult TaskResult { get; set; }

        public required string ResultMessage { get; set; }

        public Exception? Exception { get; set; }
    }
}
