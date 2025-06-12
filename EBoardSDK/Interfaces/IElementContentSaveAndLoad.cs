// <copyright file="IElementContentSaveAndLoad.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces
{
    public interface IElementContentSaveAndLoad
    {
        public Task Load(string path, IElementDataSet elementDataSet);

        public Task Save(string path, IElementDataSet elementDataSet);
    }
}
