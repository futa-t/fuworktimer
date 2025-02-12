using System;
using System.Security.Cryptography;
using System.Text;

namespace fuworktimer;

internal static class ColorUtil
{
    public static Color GetColorFromText(string text)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        int hue = hash[0] % 360;
        int saturation = 50 + hash[1] % 30;
        int brightness = 50 + hash[2] % 40;
        return HslToRgb(hue, saturation, brightness);
    }

    // https://www.peko-step.com/tool/hslrgb.html
    public static Color HslToRgb(int h, int s, int l)
    {
        float _max, _min;
        float _s = s / 100f;

        if (l < 50)
        {
            _max = 2.55f * (l + l * _s);
            _min = 2.55f * (l - l * _s);
        }
        else
        {
            _max = 2.55f * (l + (100 - l) * _s);
            _min = 2.55f * (l - (100 - l) * _s);
        }

        int max = (int)Math.Round(_max);
        int min = (int)Math.Round(_min);
        int m = (int)Math.Round(_max - _min);

        return h switch
        {
            _ when h <= 60 => Color.FromArgb(max, (int)((h / 60f) * m + min), min),
            _ when h <= 120 => Color.FromArgb((int)(((120f - h) / 60f) * m + min), max, min),
            _ when h <= 180 => Color.FromArgb(min, max, (int)(((h - 120f) / 60f) * m + min)),
            _ when h <= 240 => Color.FromArgb(min, (int)(((240f - h) / 60f) * m + min), max),
            _ when h <= 300 => Color.FromArgb((int)(((h - 240f) / 60f) * m + min), min, max),
            _ when h <= 360 => Color.FromArgb(max, min, (int)(((360f - h) / 60f) * m + min)),
            _ => Color.White
        };
    }
}

internal static class TimeFormat
{
    internal static string fmt_hms(int sec)
    {
        int h, m, s = 0;

        (h, s) = divmod(sec, 3600);
        (m, s) = divmod(s, 60);

        return $"{h:D2}:{m:D2}:{s:D2}";
    }

    static (int, int) divmod(int a, int b)
    {
        int c = a / b;
        int d = a % b;
        return (c, d);
    }
}
