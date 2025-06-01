using System.IO;
using System.Xml.Serialization;

namespace EBoardSDK.SharedMethods
{
    public class SharedMethod_Plugins
    {
        public Task<T>? DeserializeConfigFiles<T>(string filepath)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(filepath))
            {
                try
                {
                    var member = (T)xmlSerializer.Deserialize(reader)!;

                    if (member != null)
                    {
                        return Task.FromResult(member);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return null;
            }
        }

        public async Task<EBoardFeedbackMessage> SerializeConfigFiles<T>(T dataModel, string targetFilePath)
        {
            try
            {
                // serialize content
                var xmlSerializer = new XmlSerializer(typeof(T));

                await using (var writer = new StreamWriter(targetFilePath))
                {
                    xmlSerializer.Serialize(writer, dataModel);
                }
            }
            catch (Exception ex)
            {
                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message, Exception = ex };
            }

            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"data serialized to: {targetFilePath}" };
        }
    }
}
