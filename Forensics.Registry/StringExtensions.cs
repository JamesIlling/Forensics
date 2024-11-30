using System.Globalization;
using System.Text.RegularExpressions;

namespace Forensics.Registry;

public static partial class StringExtensions
{
    public static ushort? ParseVendorId(this string? value)
    {
        if (value == null)
        {
            return null;
        }

        var match = VendorIdRegex().Match(value);
        if (match.Success)
        {
            var vendorId = match.Groups["vendorId"].Value;
            return ushort.Parse(vendorId, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return null;
    }

    public static ushort? ParseProductId(this string? value)
    {
        if (value == null)
        {
            return null;
        }

        var match = ProductIdRegex().Match(value);
        if (match.Success)
        {
            var productId = match.Groups["productId"].Value;
            return ushort.Parse(productId, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return null;
    }

    public static byte? ParseInterface(this string? value)
    {
        if (value == null)
        {
            return null;
        }

        var match = InterfaceRegex().Match(value);
        if (match.Success)
        {
            var interfaceId = match.Groups["interfaceId"].Value;
            return byte.Parse(interfaceId, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return null;
    }

    public static ushort? ParseRevision(this string? value)
    {
        if (value == null)
        {
            return null;
        }

        var match = RevisionRegex().Match(value);
        if (match.Success)
        {
            var revision = match.Groups["revision"].Value;
            return ushort.Parse(revision, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return null;
    }

    [GeneratedRegex("REV(_)?(?<revision>[0-9A-F]{4})", RegexOptions.Compiled)]
    private static partial Regex RevisionRegex();


    [GeneratedRegex("MI(_)?(?<interfaceId>[0-9A-F]{2})", RegexOptions.Compiled)]
    private static partial Regex InterfaceRegex();

    [GeneratedRegex("PID(_)?(?<productId>[0-9A-F]{4})", RegexOptions.Compiled)]
    private static partial Regex ProductIdRegex();

    [GeneratedRegex("VID(_)?(?<vendorId>[0-9A-F]{4})", RegexOptions.Compiled)]
    private static partial Regex VendorIdRegex();
}