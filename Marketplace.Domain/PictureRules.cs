namespace Marketplace.Domain
{
    public static class PictureRules
    {
        public static bool HasCorrectSize(this Picture picture) => picture != null
                                                                   && picture.Size.Height >= 600
                                                                   && picture.Size.Width > 800;
    }
}