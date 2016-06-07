using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public class postToDatabase : MonoBehaviour {

    //.................................>8.......................................
    // Credit goes to:  Answer by naglers · Nov 01, 2013 at 04:36 PM
    // URL: http://answers.unity3d.com/questions/567497/how-to-100-check-internet-availability.html
    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess) {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream())) {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach(char ch in cs) {
                            html +=ch;
                        }
                    }
                }
            }
        }
        catch {
            return "";
        }
        return html;
    }

    //.................................>8.......................................
    public static void Start() {
        /*
        // Check for internet connectivity.
        string HtmlTextCheck = GetHtmlFromUri("http://sqrlab.ca");

        if (HtmlTextCheck == "") {
            // No connection.
        }
        else if (!HtmlTextCheck.Contains("schema.org/WebPage")) {
            // Redirecting.
        }
        else {
            // Success, web connectivity.
            // Credit to: Evan Mulawski
            // URL:http://stackoverflow.com/questions/4015324/http-request-with-post
            HttpClient client = new HttpClient();
            Dictionary values = new Dictionary<string, string>();

            // Push everything into the dictionary.
            // Bookmark: https://msdn.microsoft.com/en-us/library/xfhwa508%28v=vs.110%29.aspx

            // Add key-value pairs to the dictionary
            // Try this: https://docs.unity3d.com/ScriptReference/WWWForm.html

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            Task<HttpResponseMessage> response = client.PostAsync("http://www.sqrlab.com/robocollector.aspx", content);
            var responseString = response.Content.ReadAsStringAsync();


        }

        */
    }

    //.................................>8.......................................
}
