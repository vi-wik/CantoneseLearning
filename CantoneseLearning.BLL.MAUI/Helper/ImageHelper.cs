namespace viwik.CantoneseLearning.BLL.MAUI.Helper
{
    public class ImageHelper
    {
        public static bool IsImagehubImage(string url)
        {
            return url?.ToLower().Contains("imagehub.cc") == true;
        }

        public static async Task<ImageSource> GetImageSource(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            if (!IsImagehubImage(url))
            {
                return url;
            }

            try
            {
                var handler = new HttpClientHandler();

                using (var client = new HttpClient(handler))
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    var response = await client.GetAsync(url);

                    var data = await response.Content.ReadAsStreamAsync();

                    return ImageSource.FromStream(() => data);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
