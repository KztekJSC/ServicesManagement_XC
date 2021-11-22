using Kztek.Security;
using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Library.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kztek_Library.Helpers
{
    public class FunctionHelper
    {
        /// <summary>
        /// remove vietnamese sign
        /// </summary>
        private static readonly string[] VietnameseSigns = new[]
                                                               {
                                                                   "aAeEoOuUiIdDyY",
                                                                   "áàạảãâấầậẩẫăắằặẳẵ",
                                                                   "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                                                                   "éèẹẻẽêếềệểễ",
                                                                   "ÉÈẸẺẼÊẾỀỆỂỄ",
                                                                   "óòọỏõôốồộổỗơớờợởỡ",
                                                                   "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                                                                   "úùụủũưứừựửữ",
                                                                   "ÚÙỤỦŨƯỨỪỰỬỮ",
                                                                   "íìịỉĩ",
                                                                   "ÍÌỊỈĨ",
                                                                   "đ",
                                                                   "Đ",
                                                                   "ýỳỵỷỹ",
                                                                   "ÝỲỴỶỸ"
                                                               };
        public static string GetRandomNumericCharacters(int length)
        {
            // Note: i, o, l, 0, and 1 have been removed to reduce 
            // chances of user typos and mis-communication of passwords.
            char[] allowedCharacters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            // Create a byte array to hold the random bytes.
            byte[] randomNumber = new byte[length];

            // Create a new instance of the RNGCryptoServiceProvider.
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            string result = "";

            foreach (byte b in randomNumber)
            {
                // Convert the byte to an integer value to make the modulus operation easier.
                int rand = Convert.ToInt32(b);

                // Return the random number mod'ed.
                // This yeilds a possible value for each character in the allowable range.
                int value = rand % allowedCharacters.Length;

                char thisChar = allowedCharacters[value];

                result += thisChar;
            }

            return result;
        }
        public static string GetRandomAlphanumericCharacters(int length)
        {
            // Note: i, o, l, 0, and 1 have been removed to reduce 
            // chances of user typos and mis-communication of passwords.
            char[] allowedCharacters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            // Create a byte array to hold the random bytes.
            byte[] randomNumber = new byte[length];

            // Create a new instance of the RNGCryptoServiceProvider.
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            string result = "";

            foreach (byte b in randomNumber)
            {
                // Convert the byte to an integer value to make the modulus operation easier.
                int rand = Convert.ToInt32(b);

                // Return the random number mod'ed.
                // This yeilds a possible value for each character in the allowable range.
                int value = rand % allowedCharacters.Length;

                char thisChar = allowedCharacters[value];

                result += thisChar;
            }

            return result;
        }

        public static string GetLayout(string code)
        {
            var layout = "~/Views/Shared/_Layout.cshtml";

            if (!string.IsNullOrWhiteSpace(code))
            {
                layout = "~/Areas/Admin/Views/Shared/_Layout.cshtml";
            }

            return layout;
        }

        public static string GetStatusDateByDay(DateTime date)
        {
            var time = DateTime.Now;
            var status = "";


            if (time < date)
            {
                var newDate = date.AddDays(-7);
                if (newDate > time)
                {
                    status = string.Format("<span>{0}</span>", date.ToString("dd/MM/yyyy"));
                }
                else
                {
                    status = string.Format("<span class='label label-warning label-white middle'>{0}</span>", date.ToString("dd/MM/yyyy"));
                }
            }
            else
            {
                status = string.Format("<span class='label label-danger label-white middle'>{0}</span>", date.ToString("dd/MM/yyyy"));
            }

            return status;
        }

        public static async Task<string> ConvertImgFileUploadToBase64(IFormFile FilesUpload)
        {
            string ImageUrl = "";

            using (var memoryStream = new MemoryStream())
            {
                await FilesUpload.CopyToAsync(memoryStream);
                Byte[] bytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                ImageUrl = "data:image/png;base64," + base64String;

            }

            return ImageUrl;
        }

        public static async Task<string> RemoveSpecialCharactersVn(string str)
        {
            var strNotVn = await RemoveSign4VietnameseString(str);
            var sb = new StringBuilder();
            foreach (char c in strNotVn)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            var text = await ReplaceSpaceToPlus(sb.ToString());
            return text;
        }

        public static async Task<string> RemoveSign4VietnameseString(string str)
        {
            //remove wildcard
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return await Task.FromResult(str);
        }

        public static async Task<string> ReplaceSpaceToPlus(string text)
        {
            return await Task.FromResult(Regex.Replace(text, @"\s+", "-").Trim());
        }

        public static async Task<string> GetComputerName(string clientIP)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(clientIP);
                return await Task.FromResult(hostEntry.HostName);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(string.Empty);
            }
        }

        public static string getCurrentGroup(string group)
        {
            var name = "";

            switch (group)
            {
                case "67810176":
                    break;
                case "98818976":
                    name = AreaConfig.AC_Area;
                    break;
                case "12878956":
                    name = AreaConfig.PK_Area;
                    break;
                case "61119719":
                    name = AreaConfig.LK_Area;
                    break;
                default:
                    break;
            }

            return name;
        }

        public static string FormatDate(string strdate, bool isHour = false)
        {
            var date = strdate.Split(" ");

            var arr = date[0].Split("/");

            string newdate = arr[2] + "/" + arr[1] + "/" + arr[0];

            return newdate;
        }
        public async static Task<string> FtpImage(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                string[] fileitem = filename.Split(Convert.ToChar(@"\"));
                if (fileitem != null && fileitem.Length == 6)
                {
                    string _fileonftp = "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5];

                    return await Task.FromResult(_fileonftp);
                }
                else if (fileitem != null && fileitem.Length == 7)
                {
                    string _fileonftp = "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5] + "/" + fileitem[6];

                    return await Task.FromResult(_fileonftp);
                }
            }
            else
            {
                //
                string username = await AppSettingHelper.GetStringFromAppSetting("FTP:Username");
                string pass = await AppSettingHelper.GetStringFromAppSetting("FTP:Password");

                string[] fileitem = filename.Split(Convert.ToChar(@"\"));

                if (fileitem != null && fileitem.Length == 6)
                {
                    string _fileonftp = "ftp://" + username + ":" + pass + "@" + fileitem[2] + "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5];

                    if (await CheckIfFileExistsOnServer(_fileonftp) == true)
                    {
                        WebClient web = new WebClient();
                        var t = web.DownloadData(_fileonftp);
                        if (t != null)
                        {
                            var k = ConvertByteToBase64(t);

                            return await Task.FromResult(k);
                        }
                    }
                }
            }

            return await Task.FromResult("");
        }

        private async static Task<bool> CheckIfFileExistsOnServer(string fileName)
        {
            bool isSuccess = false;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileName);
            string username = await AppSettingHelper.GetStringFromAppSetting("FTP:Username");
            string pass = await AppSettingHelper.GetStringFromAppSetting("FTP:Password");
            request.Credentials = new NetworkCredential(username, pass);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {

                Thread thre = new Thread(() =>
                {
                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    //Thread.Sleep(1000);
                });

                thre.Start();
                //thre.Join();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;

                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)

                    isSuccess = false;
            }

            return await Task.FromResult(isSuccess);
        }

        public static string ConvertByteToBase64(Byte[] bytes)
        {
            string ImageUrl = "";

            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            ImageUrl = "data:image/png;base64," + base64String;

            return ImageUrl;
        }

        public static string ConvertPathToBase64(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public async static Task<string> CalculateTimeDiff(string startDate, string endDate)
        {
            var _startDate = System.Convert.ToDateTime(startDate);
            var _endDate = System.Convert.ToDateTime(endDate);

            var timeDiff = _endDate - _startDate;

            int hour = timeDiff.Days > 0 ? (timeDiff.Hours + timeDiff.Days * 24) : timeDiff.Hours;

            return await Task.FromResult(string.Format("{0}h{1}m{2}s", hour, timeDiff.Minutes, timeDiff.Seconds));
        }
        public static string GetCgiByCameraType(string CameraType, string FrameRate, string Resolution, string SDK, string Username = "", string Password = "")
        {
            string Cgi = "";
            switch (CameraType)
            {
                case "Panasonic i-Pro":
                    Cgi = "/cgi-bin/mjpeg?framerate=" + FrameRate + "&resolution=" + Resolution;
                    break;
                case "Axis":
                    Cgi = "/axis-cgi/mjpg/video.cgi?resolution = " + Resolution;
                    break;
                case "Sony":
                    Cgi = "/image";
                    break;
                case "Shany-Stream1":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Shany";
                        break;
                    }
                    Cgi = "/live/stream1.cgi";
                    break;
                case "Shany-Stream2":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Shany";
                        break;
                    }
                    Cgi = "/live/stream2.cgi";
                    break;
                case "Secus":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/stream1";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "/cgi-bin/image/mjpeg.cgi";

                    break;
                case "Bosch":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/rtsp_tunnel";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "/rtsp_tunnel?resolution=" + Resolution;
                        break;
                    }

                    Cgi = "/snap.jpg?";
                    break;
                case "Vantech":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Vantech";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Vantech?resolution=" + Resolution;
                    }
                    break;
                case "SecusFFMPEG":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Secus";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "Secus";
                    break;
                case "CNB":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "CNB";
                        break;
                    }
                    Cgi = "CNB";
                    break;
                case "HIK":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                        Cgi = "HIK";
                    Cgi = "/api/mjpegvideo.cgi?InputNumber=1&StreamNumber=1";
                    break;
                case "Enster":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                        Cgi = "Enster";
                    break;
                case "Afidus":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Afidus";
                        break;
                    }
                    Cgi = "/cgi-bin/jpg/image.cgi";
                    break;
                case "Dahua":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Dahua";
                        break;
                    }

                    break;
                case "ITX":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "ITX";
                        break;
                    }
                    break;

                case "Hanse":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/stream1";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "/cgi-bin/image/mjpeg.cgi?" + "id=" + Username + "&passwd=" + Password;

                    break;
                case "Samsung":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Samsung";
                        break;
                    }
                    break;
                default:

                    break;
            }
            return Cgi;
        }

        public static async Task<MessageReport> FetchLicenseData(string feename = "")
        {
            var message = new MessageReport();
            //var webUrl = ConfigurationManager.AppSettings["KztekAPI"];
            var webUrl = await AppSettingHelper.GetStringFromAppSetting("LicenseUrl");

            try
            {
                var reqUri = new Uri(webUrl + "api/config/license/" + feename);

                var response = await ApiHelper.HttpGet(reqUri.ToString());

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    using (var fs = new FileStream(Directory.GetCurrentDirectory() + @"\uploads\lic.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (var sr = new StreamWriter(fs))
                        {
                            var licData = Security.CryptoProvider.SimpleEncryptWithPassword(content, SecurityModel.License_Key);

                            sr.Write(licData);
                            sr.Flush();
                        }
                    }

                    message.isSuccess = true;
                    message.Message = "Cập nhật thành công";
                }
                else
                {
                    message.isSuccess = false;
                    message.Message = "Cập nhật thất bại";
                }
            }
            catch (Exception ex)
            {
                message.isSuccess = false;
                message.Message = ex.Message;
            }

            return message;
        }

        #region Fee calculator
        public enum DateInterval
        {
            Year,
            Month,
            Weekday,
            Day,
            Hour,
            Minute,
            Second
        }

        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            int datediff = 0;
            for (int i = 1; i < 5000; i++)
            {
                if (date1.AddDays(i).ToString("yyyy/MM/dd") == date2.ToString("yyyy/MM/dd"))
                {
                    datediff = i;
                    break;
                }
            }
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return datediff / 7;
                case DateInterval.Day:
                    return datediff; // Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }

        public static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        public static int SubCalculate(int time, int timeBlock, int moneyBlock)
        {
            int temp = 0;
            if (timeBlock == 0 || time <= 0)
            {
                return 0;
            }
            else
            {
                temp = (time / timeBlock + (time % timeBlock == 0 ? 0 : 1)) * moneyBlock;
            }
            return temp;
        }

        public static DateTime GetDateTime(DateTime dtime1, DateTime dtime2)
        {
            try
            {
                string st = dtime1.ToString("yyyy/MM/dd") + " " + dtime2.ToString("HH:mm");
                return Convert.ToDateTime(dtime1.ToString("yyyy/MM/dd") + " " + dtime2.ToString("HH:mm"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtin"></param>
        /// <param name="dtout"></param>
        /// <param name="period"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public static int CalculateByPeriod(DateTime dtin, DateTime dtout, string[] period, string[] cost)
        {
            try
            {

                if (period == null || cost == null || period.Length == 0 || cost.Length == 0 || (period.Length - cost.Length) != 1)
                {
                    return 0;
                }
                int[] temp = new int[period.Length - 1];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = 0;

                TimeRecognized[] T = new TimeRecognized[period.Length - 1];
                for (int i = 0; i < T.Length; i++)
                {
                    TimeRecognized _t = new TimeRecognized();
                    T[i] = _t;
                }

                for (DateTime dtime = dtin; dtime <= dtout; dtime = dtime.AddMinutes(1))
                {
                    for (int i = 0; i < period.Length - 1; i++)
                    {

                        if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])) && dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])) ||
                            (i == (period.Length - 2) && (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])) || dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))))
                        {
                            if (T[i].HaveOverPoint == "")
                            {
                                T[i].Times++;
                                if (i == (period.Length - 2))
                                {

                                    if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])))
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime.AddDays(1), Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                    else if (dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                }
                                else
                                {
                                    T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                }
                            }
                            else
                            {

                                if (dtime > DateTime.Parse(T[i].HaveOverPoint))
                                {
                                    T[i].Times++;
                                    if (i == (period.Length - 2))
                                    {
                                        if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])))
                                        {
                                            T[i].HaveOverPoint = GetDateTime(dtime.AddDays(1), Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                        }
                                        else if (dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))
                                        {
                                            T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                        }
                                    }
                                    else
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                }

                            }
                        }
                    }
                }

                int _money = 0;

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = T[i].Times;
                    _money = _money + temp[i] * int.Parse(cost[i]);
                }

                return _money;


                //return temp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// hàm xác định xem có thời gian ban đêm không
        /// </summary>
        /// <param name="datetimein">giờ vào </param>
        /// <param name="datetimeout">giờ ra</param>
        /// <param name="dayTimeFrom">giờ bắt đầu ban ngày (thường khoảng 6h00,...)</param>
        /// <param name="dayTimeTo">giờ bắt đầu ban đêb (thường khoảng 18h00 hoặc 22h00, ...)</param>
        /// <returns></returns>
        public static bool IsHaveNightTime(DateTime datetimein, DateTime datetimeout, DateTime dayTimeFrom, DateTime dayTimeTo)
        {
            try
            {
                for (DateTime dtimes = datetimein; dtimes <= datetimeout; dtimes = dtimes.AddMinutes(1))
                {
                    if (dtimes >= GetDateTime(dtimes, dayTimeTo) || dtimes <= GetDateTime(dtimes, dayTimeFrom))
                        return true;
                }
                return false;
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// hàm xác định xem có thời gian ban ngày không
        /// </summary>
        /// <param name="datetimein">giờ vào </param>
        /// <param name="datetimeout">giờ ra</param>
        /// <param name="dayTimeFrom">giờ bắt đầu ban ngày (thường khoảng 6h00,...)</param>
        /// <param name="dayTimeTo">giờ bắt đầu ban đêb (thường khoảng 18h00 hoặc 22h00, ...)</param>
        /// <returns></returns>
        public static bool IsHaveDayTime(DateTime datetimein, DateTime datetimeout, DateTime dayTimeFrom, DateTime dayTimeTo)
        {
            try
            {
                for (DateTime dtimes = datetimein; dtimes <= datetimeout; dtimes = dtimes.AddMinutes(1))
                {
                    if (dtimes < GetDateTime(dtimes, dayTimeTo) && dtimes > GetDateTime(dtimes, dayTimeFrom))
                        return true;
                }
                return false;
            }
            catch
            { }

            return false;
        }


        /// <summary>
        /// hàm tính tổng số tiền ô tô trong bãi không chia đoạn
        /// </summary>
        /// <param name="datetimein"> thời gian vào </param>
        /// <param name="datetimeout"> thời gian ra </param>
        /// <param name="DayTimeFrom">bắt đầu giờ ban ngày</param>
        /// <param name="DayTimeTo">bắt đầu giờ ban đêm</param>
        /// <param name="MoneyBlock">số tiền block </param>
        /// <param name="TimeBlock">thời gian block </param>
        /// <param name="mdiff">tổng thời gian xe trong bãi </param>
        /// <returns></returns>
        public static int Payment_MoneyCar(DateTime datetimein, DateTime datetimeout, DateTime DayTimeFrom, DateTime DayTimeTo, List<int> MoneyBlock, List<int> TimeBlock, int mdiff)
        {
            int money = 0;
            int _daytimes = 0, _nighttimes = 0;
            int _daymoneys = 0, _nightmoneys = 0;

            for (DateTime _time = mdiff <= 24 * 60 ? datetimein : datetimein.AddDays((double)(mdiff / (24 * 60))); _time <= datetimeout; _time = _time.AddMinutes(1))
            {
                if (_time >= GetDateTime(_time, DayTimeFrom) && _time <= GetDateTime(_time, DayTimeTo))
                {
                    _daytimes = _daytimes + 1;
                }
                else
                {
                    _nighttimes = _nighttimes + 1;
                }
            }
            if (_daytimes > 0)
            {
                if (_daytimes <= TimeBlock[0])// less than 2h
                {
                    _daymoneys = MoneyBlock[0];
                }
                else //more than 2h
                {
                    _daymoneys = MoneyBlock[0] + SubCalculate(_daytimes - TimeBlock[0], TimeBlock[1], MoneyBlock[1]);
                }
            }
            if (_nighttimes > 0)
            {
                if (_nighttimes <= TimeBlock[2])// less than 2h
                {
                    _nightmoneys = MoneyBlock[2];
                }
                else //more than 2h
                {
                    _nightmoneys = MoneyBlock[2] + SubCalculate(_nighttimes - TimeBlock[2], TimeBlock[3], MoneyBlock[3]);
                }
            }
            money = _daymoneys + _nightmoneys;
            if (money > MoneyBlock[4]) // maximum in 24 hours is 120K || 140K
                money = MoneyBlock[4];
            return money;
        }
        #endregion


        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }

        public static int GenerateNumber(int max = 1000000)
        {
            Random rnd = new Random();

            int number = rnd.Next(max);

            return number;
        }

        public static string GetMinutes(DateTime from, DateTime to)
        {
            string minutes = "";

            if(from != null && to != null&& to >= from && to != DateTime.MinValue && from != DateTime.MinValue)
            {
                var timespan = to - from;

                minutes = Math.Floor(timespan.TotalMinutes).ToString();
            }

            return minutes;
        }
    }
}