namespace WorstUrlShortener.Interfaces
{
    public interface IResizeImage
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
