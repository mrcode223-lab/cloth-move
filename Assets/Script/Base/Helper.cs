using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static class Helper
{
    public static string ProcessText(string input)
    {
        string cleaned = input
    .Replace("\n", "")
    .Replace("\r", "")
    .Replace("\u200B", "")
    .Replace("\u00A0", "")
    .Trim();
        return cleaned;
    }
    public static bool DeviceIsIpad()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (Mathf.Approximately(aspectRatio, 3.0f / 4.0f)
            || (Screen.width == 1668 && Screen.height == 2388)
            || (Screen.width == 2048 && Screen.height == 2732)
            || (Screen.width == 1640 && Screen.height == 2360))
        {
            //Debug.Log("This is an iPad screen.");
            return true;
        }
        else
        {
            //Debug.Log("This is not an iPad screen.");
            return false;
        }
    }
    public static bool DeviceIsTablet()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio >= 1.3f && aspectRatio <= 2.0f
            || (Screen.width == 1536 && Screen.height == 2152)
            || (Screen.width == 1768 && Screen.height == 2208)
            || (Screen.width == 1812 && Screen.height == 2176)
            || (Screen.width == 2160 && Screen.height == 1856)
            || (Screen.width == 1812 && Screen.height == 2208)
            || (Screen.width == 1856 && Screen.height == 2160)
            || (Screen.width == 1200 && Screen.height == 1920)
            || (Screen.width == 1600 && Screen.height == 2560))
        {
            //Debug.Log("This is a tablet screen.");
            return true;
        }
        else
        {
            //Debug.Log("This is not a tablet screen.");
            return false;
        }
    }
    public static string FormatTimeSpanWithoutMilliseconds(TimeSpan timeSpan)
    {
        string formattedString = timeSpan.ToString(@"hh\:mm\:ss");
        return formattedString;
    }
    public static TimeSpan GetTimeRemainingUntilNextDay(DateTime current)
    {
        DateTime nextDay = current.Date.AddDays(1);
        TimeSpan remainingTime = nextDay - current;
        return remainingTime;
    }
    public static string ChangeSecondToTime(double s)
    {
        var timeSpan = TimeSpan.FromSeconds(s);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
           timeSpan.Hours,
           timeSpan.Minutes,
           timeSpan.Seconds,
           timeSpan.Milliseconds / 10);

        Console.WriteLine(formattedTime); // In ra màn hình
        return formattedTime;
    }
    public static string ChangeSecondToTimeSpan(float s)
    {
        var timeSpan = TimeSpan.FromSeconds(s);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}",
           timeSpan.Hours,
           timeSpan.Minutes,
           timeSpan.Seconds);

        Console.WriteLine(formattedTime); // In ra màn hình
        return formattedTime;
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public static string ChangeIntToTimeString(int time)
    {
        if (time >= 10)
        {
            return time.ToString();
        }
        else
        {
            return "0" + time;
        }
    }
    public static string CurrencyString(double value)
    {
        return $"{value:#,###0}";
    }
    public static string CurrencyStringBigInterger(BigInteger value)
    {
        return $"{value:#,###0}";
    }
    public static string BigIntergerCurrencyString(System.Numerics.BigInteger value)
    {
        return $"{value:#,###0}";
    }
    public static string StatString(float value)
    {
        return $"{value:#,###0}";
    }
    public static string UpgradeStatString(double value)
    {
        return $"{value.ToString("N2"):#,###0}";
    }
    public static string RichTextGreenSkillStat(float value)
    {
        return UpgradeStatString(value);
    }
    public static string DifficultyStringCore(int difficulty)
    {
        if (difficulty < 10)
        {
            return "00" + difficulty;
        }
        else if (difficulty >= 10 && difficulty < 100)
        {
            return "0" + difficulty;
        }
        else
        {
            return difficulty.ToString();
        }
    }
    public static string AreaStringCore(int areaID)
    {
        return areaID + "0000";
        //if (areaID < 10)
        //{
        //    return areaID + "0000";
        //}
        //else if (areaID >= 10 && areaID < 100)
        //{
        //    return areaID + "000";
        //}
        //else if (areaID >= 100 && areaID < 1000)
        //{
        //    return areaID + "00";
        //}
        //else if (areaID >= 1000 && areaID < 10000)
        //{
        //    return areaID + "0";
        //}
        //else
        //{
        //    return areaID.ToString();
        //}
    }
    public static int GetWeekOfYear(DateTime dateTime)
    {
        return new GregorianCalendar().GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
    public static string AmountString(double score)
    {
        string[] scoreNames =
        {
            "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an",
            "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf",
            "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx",
            "by", "bz",
        };
        if (score < 10000)
        {
            return $"{Math.Floor(score):#,###0}";
        }
        int i;
        for (i = 0; i < scoreNames.Length; i++)
        {
            if (score < 10000)
                break;

            score /= 1000;
        }
        return $"{Math.Floor(score):#,###0}{scoreNames[i]}";
    }
    public static long Clamp(long value, long min, long max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    public static float RoundingStat(float number)
    {
        float roundedNumber;

        if ((number * 100) % 1 >= 0.5f)
        {
            roundedNumber = Mathf.Ceil(number * 100f) / 100f;
        }
        else
        {
            roundedNumber = Mathf.Floor(number * 100f) / 100f;
        }

        return roundedNumber;
    }
    public static bool IsUIElementVisible(Camera cam, RectTransform uiElement)
    {
        if (cam == null) return false;
        Vector3[] worldCorners = new Vector3[4];
        uiElement.GetWorldCorners(worldCorners);

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(corner);
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                viewportPoint.z >= 0)
            {
                return true;
            }
        }
        return false;
    }
}
