using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Utility;
using ITGAccounts.Dto;
using ITGAdmin.DTO;
using LCMS.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LCMS.Utility
{
    public static class ApplicationWrapper
    {
        public static IActionResult GetErrorResponse(this ControllerBase controllerBase, Exception exception)
        {
            throw exception;
        }

        public static IActionResult ok<T>(this ControllerBase controller, T obj)
        {
            if (obj is null) return controller.BadRequest("Response object is null.");
            if (TryGetAppMessage(obj, out var appMessage) && appMessage != null && (appMessage.ErrorMessage?.Count > 0 || appMessage.HasError))
            {
                return controller.BadRequest(appMessage);
            }

            return controller.Ok(obj);
        }

        public static DateTime? ConvertToDateTime(string input)
        {
            if (DateTime.TryParse(input, out DateTime result))
            {
                return result;
            }

            return null;
        }

        public static DateOnly? ConvertToDateOnly(string input)
        {
            if (DateOnly.TryParse(input, out DateOnly result))
            {
                return result;
            }

            return null;
        }

        public static IActionResult GetFileResponse(this ControllerBase controller, ByteResponse ByteResponse)
        {
            if (ByteResponse is null) return controller.BadRequest("ByteResponse is null.");

            if (ByteResponse.Msg?.ErrorMessage == null || ByteResponse.Msg.ErrorMessage.Count == 0
                    && ByteResponse.ByteData?.Length > 0 && !string.IsNullOrWhiteSpace(ByteResponse.FileName)
                    && !string.IsNullOrWhiteSpace(ByteResponse.ContentType))
            {
                controller.HttpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename={ByteResponse.FileName}");
                controller.HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
                return controller.File(ByteResponse.ByteData, ByteResponse.ContentType, ByteResponse.FileName);
            }

            return controller.Ok(ByteResponse);
        }

        private static bool TryGetAppMessage<T>(T obj, out AppMessage? appMessage)
        {
            appMessage = obj?.GetType().GetProperty("Msg")?.GetValue(obj) as AppMessage;

            return appMessage != null;
        }

        public static void SerializeExistingData<T>(T data) where T : class
        {
            if (data == null) return;
            if (data is IList<dynamic> list)
            {
                foreach (var obj in list)
                {
                    obj.ExistingData = SerializeObjectToJson(obj);
                }
            }
            else
            {
                dynamic obj = data;
                obj.ExistingData = SerializeObjectToJson(obj);
            }
        }

        public static string SerializeObjectToJson(object obj)
        {
            if (obj == null) return string.Empty;

            try
            {
                return JsonConvert.SerializeObject(obj);
            }

            catch (JsonException ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex);
                throw;
            }
        }

        public static List<DDLClass> FilterConfigValuesById(List<ConfigValueDto> configValues)
        {
            try
            {
                return configValues.Select((ConfigValueDto cv) => new DDLClass
                {
                    Constant = (cv.ConfigConstant ?? string.Empty),
                    Id = cv.ConfigId,
                    Description = (cv.Description ?? string.Empty)
                }).ToList();
            }

            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex.Message);
                throw;
            }
        }

        public static List<DDLClass> PopulateConfigValuesToDDLClass(List<ConfigValue> configValues)
        {
            try
            {
                if (configValues != null && configValues.Count > 0)
                {
                    return configValues.Select(configValue => new DDLClass
                    {
                        Id = Convert.ToInt64(configValue.ConfigId),
                        Constant = configValue.ConfigConstant ?? string.Empty,
                        Description = configValue.Description ?? string.Empty
                    }).ToList();
                }
                return new List<DDLClass>();
            }

            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex);
                throw;
            }
        }

        public static DateTime ToLocalTimeZone(string timeZone, DateTime utcDate)
        {
            DateTime timeZoneDate = utcDate;
            var (isvalidTimeZone, timeZoneInfo) = IsValidTimeZone(timeZone);
            if (isvalidTimeZone)
                timeZoneDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZoneInfo!);
            return timeZoneDate;
        }

        public static DateTime ToLocalTimeZone(string clientTimeZone, DateOnly date)
        {
            var utc = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            return ToLocalTimeZone(clientTimeZone, utc);
        }

        private static (bool, TimeZoneInfo?) IsValidTimeZone(string timeZoneId)
        {
            try
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return (true, tz);
            }

            catch (TimeZoneNotFoundException)
            {
                return (false, null);
            }

            catch (Exception)
            {
                return (false, null);
            }
        }

        public static bool IsFirstDayOfMonth(DateTime date) => date.Date == new DateTime(date.Year, date.Month, 1);

        public static bool IsDateValid(DateTime date) => date != DateTime.MinValue && date != DateTime.MaxValue;

        public static DateTime? SearchFromDate(DateTime? dateTime, string timeZoneId)
        {
            if (dateTime == null || dateTime == DateTime.MinValue)
                return dateTime;

            var (isValidTimeZone, timeZoneInfo) = IsValidTimeZone(timeZoneId);

            if (isValidTimeZone && timeZoneInfo != null)
            {
                DateTime fromDateIST = TimeZoneInfo.ConvertTimeFromUtc(dateTime.Value, timeZoneInfo);
                dateTime = TimeZoneInfo.ConvertTimeToUtc(fromDateIST, timeZoneInfo);
            }

            return dateTime;
        }

        public static DateTime? SearchToDate(DateTime? dateTime, string timeZoneId)
        {
            if (dateTime == null || dateTime == DateTime.MinValue)
                return dateTime;

            var (isValidTimeZone, timeZoneInfo) = IsValidTimeZone(timeZoneId);
            if (isValidTimeZone && timeZoneInfo != null)
            {
                DateTime toDateIST = TimeZoneInfo.ConvertTimeFromUtc(dateTime.Value, timeZoneInfo);
                dateTime = TimeZoneInfo.ConvertTimeToUtc(toDateIST.AddDays(1), timeZoneInfo);
            }

            return dateTime;
        }

        public static string GetAttachmentFolderPath()
        {
            string folderpath = string.Empty;
            try
            {
                //AdminS2SLogic.GetAttachmentFolderPath();
            }

            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex);
            }

            return folderpath;
        }

        public static AccApplicationInfo GetCompanyInfoFromToken(string token)
        {
            var existingData = new AccApplicationInfo();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var commonInfoJson = new Galaxy.Utility.JsonToken().GetUserData(token);
                existingData = JsonUtility.DeserializeObject<AccApplicationInfo>(commonInfoJson.Properties);
            }
            return existingData;
        }
    }
}

