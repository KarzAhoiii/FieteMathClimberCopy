using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.IO;

public class JF_Utility : MonoBehaviour
{

    public static float convertRange(
    float originalStart, float originalEnd, // original range
    float newStart, float newEnd, // desired range
    float value) // value to convert
    {
        float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }

    public static float convertRangeClamped(
    float originalStart, float originalEnd, // original range
    float newStart, float newEnd, // desired range
    float value) // value to convert
    {
        value = Mathf.Clamp(value, originalStart, originalEnd);
        float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }

    public static float angleBetween2Lines(float l1x1, float l1y1, float l1x2, float l1y2, float l2x1, float l2y1, float l2x2, float l2y2)
    {
        float angle1 = Mathf.Atan2(l1y1 - l1y2, l1x1 - l1x2);
        float angle2 = Mathf.Atan2(l2y1 - l2y2, l2x1 - l2x2);
        return angle1 - angle2;
    }

    public static int randomRangeInt (System.Random random, int start, int end)
    {
        return random.Next(start, end+1);
    }

    public static float randomRangeFloat (System.Random random, float start, float end)
    {
        //double value = random.NextDouble();
        return convertRange(0.0f, 1.0f, start, end, (float)random.NextDouble());
    }

    public static Vector3 getPerpendicular (Vector3 original)
    {
        return new Vector3(original.y, -original.x, original.z);
    }

    public static bool getRectangleCircleIntersection (Vector3 circlePosition, float circleRadius, Vector3 rectanglePosition, float rectangleWidth, float rectangleHeight)
    {
        float circleDistanceX = Mathf.Abs(circlePosition.x - rectanglePosition.x);
        float circleDistanceY = Mathf.Abs(circlePosition.y - rectanglePosition.y);

        if (circleDistanceX > rectangleWidth / 2 + circleRadius) return false;
        if (circleDistanceY > rectangleHeight / 2 + circleRadius) return false;

        return false;
    }

    public static float calculateTextHeightInWorldSpace (Text text)
    {
        return (text.font.lineHeight * text.lineSpacing) * (text.fontSize / text.font.fontSize * text.rectTransform.localScale.x);
    }

    public static float calculateLineHeight(Text text)
    {
        Vector2 extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
        return text.cachedTextGeneratorForLayout.GetPreferredHeight(text.text, text.GetGenerationSettings(extents));

        //return lineHeight;
    }

    public static string convertTimeSpanToString(TimeSpan timeSpan)
    {
        string output = "";

        if (timeSpan.Days != 0)
            output += timeSpan.Days + "d ";
        if (timeSpan.Hours != 0)
            output += timeSpan.Hours + "h ";
        if (timeSpan.Minutes != 0)
            output += timeSpan.Minutes + "m ";
        if (timeSpan.Seconds != 0 && timeSpan.Hours == 0 && timeSpan.Days == 0)
            output += timeSpan.Seconds + "s ";

        if (output == "")
            output = "0" + "s";

        return output;
    }

    public static string convertAgeTypeToString (PS_InputField.ageTypes ageType)
    {
        if (ageType == PS_InputField.ageTypes.age5)
            return "5";
        else if (ageType == PS_InputField.ageTypes.age6)
            return "6";
        else if (ageType == PS_InputField.ageTypes.age7)
            return "7";
        else if (ageType == PS_InputField.ageTypes.age8)
            return "8";
        else if (ageType == PS_InputField.ageTypes.age9)
            return "9";
        else if (ageType == PS_InputField.ageTypes.age10)
            return "10+";
        else
        {
            Debug.LogWarning("No matching age type found.");
            return "?";
        }
    }

    /* ABO
    public static bool isExpired (DateTime expirationDateInUtc)
    {
        // Expiration
        //TimeSpan utcOffset = DateTime.Now - DateTime.UtcNow;
        //DateTime newDateToCheck = expirationDateInUtc + utcOffset;
        if (FMC_GameDataController.instance)
        {

            TimeSpan timeToExpiry = expirationDateInUtc - FMC_GameDataController.instance.getCurrentInternetTime();
            FMC_GameDataController.instance.writeToReceiptLog("isExpired: Time to Expiry: " + timeToExpiry);
            Debug.Log("isExpired: Time to Expiry: " + timeToExpiry);

            if (timeToExpiry.TotalSeconds < 0)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public static float getExpirationTime (DateTime expirationDate)
    {
		if (FMC_GameDataController.instance)
		{
			TimeSpan timeToExpiry = expirationDate - FMC_GameDataController.instance.getCurrentInternetTime();
			return (float)timeToExpiry.TotalSeconds;
		}
		else
			return -1;
    }

    public static DateTime GetFastestNISTDate()
    {
        var result = DateTime.MinValue;

        // Initialize the list of NIST time servers
        // http://tf.nist.gov/tf-cgi/servers.cgi
        string[] servers = new string[] 
        {
        "time-a-g.nist.gov",
        "time-b-g.nist.gov",
        "time-c-g.nist.gov",
        "time-d-g.nist.gov",
        "time-a-wwv.nist.gov",
        "time-b-wwv.nist.gov",
        "time-c-wwv.nist.gov",
        "time-d-wwv.nist.gov",
        "time-a-b.nist.gov",
        "time-b-b.nist.gov"
        };

        //Debug.Log("1");
        
        // Try 5 servers in random order to spread the load
        System.Random rnd = new System.Random();
        foreach (string server in servers.OrderBy(s => rnd.NextDouble()).Take(5))
        {
            //Debug.Log("2");
            try
            {
                // Connect to the server (at port 13) and get the response
                string serverResponse = string.Empty;
                using (var reader = new StreamReader(new System.Net.Sockets.TcpClient(server, 13).GetStream()))
                {
                    serverResponse = reader.ReadToEnd();
                }
                //Debug.Log("3");
                // If a response was received
                if (!string.IsNullOrEmpty(serverResponse))
                {
                    //Debug.Log("4");
                    // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                    string[] tokens = serverResponse.Split(' ');

                    // Check the number of tokens
                    if (tokens.Length >= 6)
                    {
                        //Debug.Log("5");
                        // Check the health status
                        string health = tokens[5];
                        if (health == "0")
                        {
                            // Get date and time parts from the server response
                            string[] dateParts = tokens[1].Split('-');
                            string[] timeParts = tokens[2].Split(':');

                            // Create a DateTime instance
                            DateTime utcDateTime = new DateTime(
                                Convert.ToInt32(dateParts[0]) + 2000,
                                Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                Convert.ToInt32(timeParts[2]));

                            // Convert received (UTC) DateTime value to the local timezone
                            result = utcDateTime.ToLocalTime();
                            Debug.Log("Local: " + result);

                            return result;
                            // Response successfully received; exit the loop

                        }
                    }
                }
            }
            catch
            {
                // Ignore exception and try the next server
            }
        }
        Debug.Log("Could not find correct internet time.");
        //return result;
        return DateTime.MaxValue;
    }
    */
}
