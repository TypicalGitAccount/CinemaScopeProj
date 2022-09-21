using System.Web;

namespace Identity.Interfaces
{
    public interface IImageService
    {
        byte[] DefaultImage { get; }

        void CreateImage(int id, HttpFileCollectionBase files);

        void UpdateImage(int id, HttpFileCollectionBase files);

        byte[] GetImage(int id);

        void DeleteImage(int id);
    }
}
