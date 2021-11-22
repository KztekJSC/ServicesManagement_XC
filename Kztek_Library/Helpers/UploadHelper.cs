using System.IO;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Library.Extensions;
using Microsoft.AspNetCore.Http;

namespace Kztek_Library.Helpers
{
    public class UploadHelper
    {
        public static async Task<MessageReport> UploadFile(IFormFile file, string path)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var fileStream = new FileStream(path + "/" + file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                result = new MessageReport(true, "Upload thành công");
            }
            catch (System.Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        public static async Task<string> GetFileNameNormalize(IFormFile file, string folderpath = "",string setting = "")
        {
            var extension = Path.GetExtension(file.FileName) ?? "";

            var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtilHelper.RemoveSpecialCharactersVn(file.FileName.Replace(extension, "")).GetNormalizeString(), extension));

            var folder = await AppSettingHelper.GetStringFromAppSetting(setting);

            var path = string.Format("{0}{1}/{2}", folder, folderpath, fileName);

            return path;
        }

        public static async Task<string> SaveAvatar(IFormFile file, string customerid)
        {
            var avatarPath = file != null ? await GetFileNameNormalize(file, customerid,"FileUpload:CustomerFolder") : "";

            var path = string.Format("{0}{1}{2}", Directory.GetCurrentDirectory(), await AppSettingHelper.GetStringFromAppSetting("FileUpload:CustomerFolder"), customerid);

            await UploadFile(file, path);

            return avatarPath;
        }

        public static async Task<string> SaveFile(IFormFile file,string folder)
        {
            var avatarPath = file != null ? await GetFileNameNormalize(file, folder, "FileUpload:Download") : "";

            var path = string.Format("{0}{1}{2}", Directory.GetCurrentDirectory(), await AppSettingHelper.GetStringFromAppSetting("FileUpload:Download"), folder);

            await UploadFile(file, path);

            return avatarPath;
        }
    }
}