using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Translate
{
    public partial class Form1 : Form
    {
        // Gemini 2.5 Flash model endpoint - correct API v1beta
        private readonly string defaultEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent";

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            var source = txtSource.Text?.Trim();
            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("Çevirmek için metin giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sourceSel = cmbSource.SelectedItem?.ToString();
            var targetSel = cmbTarget.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(sourceSel) || string.IsNullOrEmpty(targetSel))
            {
                MessageBox.Show("Lütfen kaynak ve hedef dili seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sourceCode = ExtractLangCode(sourceSel);
            string targetCode = ExtractLangCode(targetSel);

            if (string.Equals(sourceCode, targetCode, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Kaynak ve hedef diller aynı. Farklı bir hedef dil seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnTranslate.Enabled = false;
            txtResult.Text = "Çeviriliyor...";

            try
            {
                // Read api credential from App.config manually
                var apiCredential = GetAppSetting("ApiCredential");

                if (string.IsNullOrEmpty(apiCredential))
                {
                    MessageBox.Show("API kimlik bilgisi bulunamadı. Lütfen App.config içinde 'ApiCredential' anahtarını ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtResult.Text = "API kimlik bilgisi bulunamadı. Ayarlara bakın.";
                    return;
                }

                using (var client = new HttpClient())
                {
                    bool usedBearer = false;
                    if (apiCredential.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = apiCredential.Substring(apiCredential.IndexOf(' ') + 1);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        usedBearer = true;
                    }
                    else if (apiCredential.StartsWith("ya29.", StringComparison.OrdinalIgnoreCase) || apiCredential.StartsWith("ya29_"))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiCredential);
                        usedBearer = true;
                    }

                    var sourcePart = sourceCode == "auto" ? "kaynak dil algılanarak" : "kaynak dil: " + sourceCode;

                    var prompt = new StringBuilder();
                    prompt.AppendLine($"Aşağıdaki metni {sourcePart} hedef dile çeviriniz: {targetCode}.");
                    prompt.AppendLine("Sadece çevrilmiş metni döndürün, ek açıklama yapmayın.");
                    prompt.AppendLine("---");
                    prompt.AppendLine(source);

                    // Correct Gemini API request format
                    var requestJson = $@"{{
  ""contents"": [{{
    ""parts"": [{{
      ""text"": ""{EscapeJsonString(prompt.ToString())}""
    }}]
  }}],
  ""generationConfig"": {{
    ""temperature"": 0.1,
    ""maxOutputTokens"": 2048
  }}
}}";
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    var uriBuilder = new UriBuilder(defaultEndpoint);
                    if (!usedBearer)
                    {
                        // treat apiCredential as API key and append
                        var qp = uriBuilder.Query;
                        if (!string.IsNullOrEmpty(qp) && qp.StartsWith("?")) qp = qp.Substring(1);
                        var newQuery = string.IsNullOrEmpty(qp) ? $"key={Uri.EscapeDataString(apiCredential)}" : qp + "&" + $"key={Uri.EscapeDataString(apiCredential)}";
                        uriBuilder.Query = newQuery;
                    }

                    var requestUri = uriBuilder.Uri;

                    // show path only for debugging (do not reveal key)
                    txtResult.Text = "İstek gönderildi: " + requestUri.GetLeftPart(UriPartial.Path) + " ...";

                    var resp = await client.PostAsync(requestUri, content);
                    var respText = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode)
                    {
                        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            txtResult.Text = "HTTP 404 - Endpoint bulunamadı. Lütfen:\r\n- Projenizde Generative Language API etkin mi kontrol edin.\r\n- Model ismi veya path doğru mu.\r\n- API erişim tipiniz (API key vs OAuth Bearer) doğru şekilde iletiliyor mu?";
                        }
                        else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized || resp.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            txtResult.Text = $"HTTP {(int)resp.StatusCode} - {resp.ReasonPhrase}\r\n{respText}\r\n\r\nNot: Gemini 2.5 Flash genellikle OAuth/Bearer token ile kullanılır. Eğer sadece API key kullanıyorsanız, erişim kısıtlı olabilir.";
                        }
                        else
                        {
                            txtResult.Text = $"HTTP {(int)resp.StatusCode} - {resp.ReasonPhrase}\r\n{respText}";
                        }
                    }
                    else
                    {
                        var extracted = ExtractTextFromResponse(respText) ?? respText;
                        if (!usedBearer)
                            txtResult.Text = extracted + "\r\n\r\n(Uyarı: API key kullanıldı. OAuth Bearer token tercih edilir.)";
                        else
                            txtResult.Text = extracted;
                    }
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = "Hata: " + ex.Message;
            }
            finally
            {
                btnTranslate.Enabled = true;
            }
        }

        private static string GetAppSetting(string key)
        {
            try
            {
                var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                if (string.IsNullOrEmpty(configPath)) return null;
                var doc = new XmlDocument();
                doc.Load(configPath);
                var node = doc.SelectSingleNode($"/configuration/appSettings/add[@key='{key}']");
                if (node?.Attributes != null)
                {
                    var attr = node.Attributes["value"];
                    if (attr != null) return attr.Value;
                }
            }
            catch
            {
                // ignore
            }
            return null;
        }

        private static string ExtractLangCode(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            var parts = s.Split(new[] { '-' }, 2);
            if (parts.Length == 0) return s.Trim();
            return parts[0].Trim();
        }

        private static string ExtractTextFromResponse(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                // Gemini API v1beta response format: candidates[0].content.parts[0].text
                var candidatesKey = "\"candidates\"";
                var idx = json.IndexOf(candidatesKey, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    // Look for "text" field inside parts
                    var textKey = "\"text\"";
                    var tidx = json.IndexOf(textKey, idx, StringComparison.OrdinalIgnoreCase);
                    if (tidx >= 0)
                    {
                        var colon = json.IndexOf(':', tidx);
                        if (colon >= 0)
                        {
                            var start = json.IndexOf('"', colon + 1);
                            if (start >= 0)
                            {
                                start++;
                                var end = start;
                                var escapeNext = false;
                                // Find the closing quote, handling escaped quotes
                                while (end < json.Length)
                                {
                                    if (escapeNext)
                                    {
                                        escapeNext = false;
                                        end++;
                                        continue;
                                    }
                                    if (json[end] == '\\')
                                    {
                                        escapeNext = true;
                                        end++;
                                        continue;
                                    }
                                    if (json[end] == '"')
                                    {
                                        break;
                                    }
                                    end++;
                                }
                                if (end > start)
                                {
                                    var val = json.Substring(start, end - start);
                                    return UnescapeJsonString(val);
                                }
                            }
                        }
                    }
                }

                // Check outputs -> text
                var outputsKey = "\"outputs\"";
                idx = json.IndexOf(outputsKey, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var textKey = "\"text\"";
                    var tidx = json.IndexOf(textKey, idx, StringComparison.OrdinalIgnoreCase);
                    if (tidx >= 0)
                    {
                        var colon = json.IndexOf(':', tidx);
                        if (colon >= 0)
                        {
                            var start = json.IndexOf('"', colon + 1);
                            if (start >= 0)
                            {
                                start++;
                                var end = json.IndexOf('"', start);
                                if (end > start)
                                {
                                    var val = json.Substring(start, end - start);
                                    return UnescapeJsonString(val);
                                }
                            }
                        }
                    }
                }

                // fallback checks for content or text anywhere
                var key = "\"content\"";
                idx = json.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var colon = json.IndexOf(':', idx);
                    if (colon >= 0)
                    {
                        var start = json.IndexOf('"', colon + 1);
                        if (start >= 0)
                        {
                            start++;
                            var end = json.IndexOf('"', start);
                            if (end > start)
                            {
                                var val = json.Substring(start, end - start);
                                return UnescapeJsonString(val);
                            }
                        }
                    }
                }

                key = "\"text\"";
                idx = json.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var colon = json.IndexOf(':', idx);
                    if (colon >= 0)
                    {
                        var start = json.IndexOf('"', colon + 1);
                        if (start >= 0)
                        {
                            start++;
                            var end = json.IndexOf('"', start);
                            if (end > start)
                            {
                                var val = json.Substring(start, end - start);
                                return UnescapeJsonString(val);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        private static string UnescapeJsonString(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Replace("\\\"", "\"").Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");
        }

        private static string SimpleJsonSerialize(object obj)
        {
            if (obj == null) return "null";

            var type = obj.GetType();
            var props = type.GetProperties();
            var sb = new StringBuilder();
            sb.Append('{');
            var first = true;
            foreach (var p in props)
            {
                if (!first) sb.Append(',');
                first = false;
                var name = p.Name;
                var val = p.GetValue(obj);
                sb.Append('"').Append(name).Append('"').Append(':');

                if (val == null)
                {
                    sb.Append("null");
                }
                else if (val is string)
                {
                    sb.Append('"').Append(EscapeJsonString((string)val)).Append('"');
                }
                else
                {
                    var vtype = val.GetType();
                    if (vtype.IsPrimitive)
                    {
                        sb.Append(Convert.ToString(val, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        var innerProps = vtype.GetProperties();
                        sb.Append('{');
                        var firstInner = true;
                        foreach (var ip in innerProps)
                        {
                            if (!firstInner) sb.Append(',');
                            firstInner = false;
                            sb.Append('"').Append(ip.Name).Append('"').Append(':');
                            var ival = ip.GetValue(val);
                            if (ival == null) sb.Append("null");
                            else sb.Append('"').Append(EscapeJsonString(ival.ToString())).Append('"');
                        }
                        sb.Append('}');
                    }
                }
            }
            sb.Append('}');
            return sb.ToString();
        }

        private static string EscapeJsonString(string s)
        {
            if (s == null) return null;
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Added to match designer event subscription
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblTarget_Click(object sender, EventArgs e)
        {

        }
    }
}
