using viwik.CantoneseLearning.BLL.MAUI.Manager;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.BLL.MAUI.Helper
{
    public class ImageHelper
    {
        public static bool IsImagehubImage(string url)
        {
            return url?.ToLower().Contains("imagehub.cc") == true;
        }

        public static async Task<ImageSource> GetImageSource(V_CantoneseMedia media)
        {
            if (media == null)
            {
                return null;
            }

            string url = media.ImageUrl;

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                string cacheFilePath = CacheManager.GetMediaImageCacheFilePath(media);

                if (File.Exists(cacheFilePath))
                {
                    return cacheFilePath;
                }

                using (var handler = new HttpClientHandler())
                {
                    var client = new HttpClient(handler);

                    if (IsImagehubImage(url))
                    {
                        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    }

                    var response = await client.GetAsync(url);

                    var data = await response.Content.ReadAsByteArrayAsync();

                    File.WriteAllBytes(cacheFilePath, data);

                    return ImageSource.FromStream(() => new MemoryStream(data));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
