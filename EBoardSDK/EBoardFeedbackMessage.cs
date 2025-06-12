// <copyright file="EBoardFeedbackMessage.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK
{
    public class EBoardFeedbackMessage
    {
        public required EBoardTaskResult TaskResult { get; set; }

        public required string ResultMessage { get; set; }

        public Exception? Exception { get; set; }

        public override string ToString()
        {
            var s = this.Exception?.ToString() ?? string.Empty;

            return $"task result: {this.TaskResult} _ details: {this.ResultMessage} _ {s}";
        }
    }
}
