using System.Security.Cryptography;
using System.Text;

namespace CourtBooking.Utils
{
    public static class AppUtils
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IWebHostEnvironment _environment;
        private static IConfiguration _config;

        public static void Configure(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _environment = webHostEnvironment;
            _config = config;
        }
        public static string MD5Enscrypt(string msg)
        {
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            using (var alg = MD5.Create())
            {
                byte[] hashBytes = alg.ComputeHash(msgBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static string HmacSha256Encrypt(string msg)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_config.GetValue<string>("AppSettings:AppSecrect"));
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            using (var alg = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = alg.ComputeHash(msgBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static void WriteBug(string? bugMessage)
        {
            if (string.IsNullOrEmpty(bugMessage))
            {
                return;
            }
            string timetxt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string path = _environment.ContentRootPath + "/logs/Bugs_" + DateTime.Now.ToString("dd_MM_yyyy") + ".log";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(timetxt + ": " + bugMessage);
            }
            return;
        }
        public static async Task<string> UploadedFileAsync(IFormFile? item, string productId)
        {
            if (item == null)
            {
                return "";
            }
            string upFolder = Path.Combine(_environment.WebRootPath, "Resources", productId, "Images");
            string fileName = Guid.NewGuid().ToString() + '_' + item.FileName;
            try
            {
                if (!Directory.Exists(upFolder))
                {
                    Directory.CreateDirectory(upFolder);
                }
                using (var stream = new FileStream(Path.Combine(upFolder, fileName), FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                    string finalPath = Path.Combine(upFolder.Replace(_environment.WebRootPath + @"\", ""), fileName);
                    return finalPath.Replace(@"\", @"/");
                }
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
