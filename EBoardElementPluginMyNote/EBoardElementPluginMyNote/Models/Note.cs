/*  MyNote (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Note
 * 
 *  serializable data model class
 */
namespace EBoardElementPluginMyNote.Models
{
    [Serializable]
    public class Note
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime DateTime_Created { get; set; }
        public DateTime DateTime_Edited { get; set; }

        public string Content { get; set; }

        public Note()
        {
            ID = -1;
            DateTime_Created = DateTime.Now;

            Title = "title";
            Content = "note";
        }

        public Note(int id, string title, DateTime dateTime, string content)
        {
            ID = id;
            Title = title;
            DateTime_Created = dateTime;
            Content = content;
        }
    }
}
// EOF